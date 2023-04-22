class PoseCanvas : Control
{
    const double ZoomStep = 0.9;
    const double MinSelectDistance = 1.0;

    private static Color BackgroundColor = Color.FromArgb(40, 40, 40);
    private SolidBrush brush = new SolidBrush(Color.White);
    private Pen pen = new Pen(Color.White);
    private Pen selPen = new Pen(Color.White);

    private double offsetX = 0;
    private double offsetY = 0;
    private double scale = 1.0 / 512.0;

    private double offsetXold = 0;
    private double offsetYold = 0;
    private bool cameraMoving = false;
    private int oldx = 0;
    private int oldy = 0;

    private bool pointMoving = false;
    private double oldPointX = 0.0;
    private double oldPointY = 0.0;
    private double oldCursorX = 0.0;
    private double oldCursorY = 0.0;

    private bool initialResize = true;
    private Size oldSize = Size.Empty;

    private int selection = -1;

    public ProjectData project;

    public PoseCanvas(ProjectData data)
    {
        pen.Width = 8;
        selPen.Width = 2;
        this.DoubleBuffered = true;
        project = data;
    }

    public (double, double) ScreenToWorld(int x, int y)
    {
        return ((x - offsetX) * scale, (y - offsetY) * scale);
    }

    public (int, int) WorldToScreen(double x, double y)
    {
        return (
            (int)(x / scale + offsetX),
            (int)(y / scale + offsetY)
        );
    }

    private void AdjustOffset(int x, int y, double k)
    {
        offsetX = (x * (k - 1) + offsetX) * (1 / k);
        offsetY = (y * (k - 1) + offsetY) * (1 / k);
    }

    public void ResetCamera()
    {
        if (project.canvasFloatWidth / project.canvasFloatHeight < (double)Width / (double)(Height))
        {
            scale = project.canvasFloatHeight / (double)Height;
        }
        else
        {
            scale = project.canvasFloatWidth / (double)Width;
        }
        offsetX = Width / 2;
        offsetY = Height / 2;
        Invalidate();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (initialResize)
        {
            ResetCamera();
            initialResize = false;
            oldSize = this.Size;
        }
        else
        {
            offsetX += (double)(Width - oldSize.Width) / 2;
            offsetY += (double)(Height - oldSize.Height) / 2;
            Invalidate();
            oldSize = this.Size;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.Clear(BackgroundColor);
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        brush.Color = Color.Black;

        (int canvasOffsetX, int canvasOffsetY) = WorldToScreen(0.5 - project.canvasFloatWidth, 0.5 - project.canvasFloatHeight);
        e.Graphics.FillRectangle(
            brush,
            canvasOffsetX,
            canvasOffsetY,
            (int)(project.canvasFloatWidth / scale),
            (int)(project.canvasFloatHeight / scale));

        Figure fig = project.figure;

        for (int i = 0; i < Figure.pairs.Length; i++)
        {
            var pair = Figure.pairs[i];
            if (!fig.points[pair.Item1].enabled || !fig.points[pair.Item2].enabled)
            {
                continue;
            }
            pen.Color = Figure.pointColors[i];
            pen.Color = Color.FromArgb(153, pen.Color);
            var point1 = WorldToScreen(fig.points[pair.Item1].x, fig.points[pair.Item1].y);
            var point2 = WorldToScreen(fig.points[pair.Item2].x, fig.points[pair.Item2].y);
            e.Graphics.DrawLine(
                pen,
                point1.Item1,
                point1.Item2,
                point2.Item1,
                point2.Item2
                );
        }

        for (int i = 0; i < fig.points.Length; i++)
        {
            if (!fig.points[i].enabled)
            {
                continue;
            }
            brush.Color = Figure.pointColors[i];
            brush.Color = Color.FromArgb(153, brush.Color);
            var point = WorldToScreen(fig.points[i].x, fig.points[i].y);
            e.Graphics.FillEllipse(brush, point.Item1 - 8, point.Item2 - 8, 16, 16);
        }

        if (selection >= 0)
        {
            var selPoint = WorldToScreen(fig.points[selection].x, fig.points[selection].y);
            e.Graphics.DrawEllipse(selPen, selPoint.Item1 - 10, selPoint.Item2 - 10, 20, 20);
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button == MouseButtons.Middle)
        {
            cameraMoving = true;
            oldx = e.X;
            oldy = e.Y;
            offsetXold = offsetX;
            offsetYold = offsetY;
        }
        else if (e.Button == MouseButtons.Right && selection >= 0)
        {
            pointMoving = true;
            Cursor.Hide();
            (oldCursorX, oldCursorY) = ScreenToWorld(e.X, e.Y);
            oldPointX = project.figure.points[selection].x;
            oldPointY = project.figure.points[selection].y;
        }
        else if (e.Button == MouseButtons.Left && !pointMoving)
        {
            selection = -1;
            (double cursorX, double cursorY) = ScreenToWorld(e.X, e.Y);
            double minDist = MinSelectDistance;
            for (int i = 0; i < project.figure.points.Length; i++)
            {
                if (!project.figure.points[i].enabled)
                {
                    continue;
                }
                double x = project.figure.points[i].x;
                double y = project.figure.points[i].y;
                double dist = ((cursorX - x) * (cursorX - x) + (cursorY - y) * (cursorY - y)) / scale;
                if (dist < minDist)
                {
                    minDist = dist;
                    selection = i;
                }
            }
            Invalidate();
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (cameraMoving)
        {
            offsetX = offsetXold + e.X - oldx;
            offsetY = offsetYold + e.Y - oldy;
            Invalidate();
        }
        if (pointMoving)
        {
            (double newCursorX, double newCursorY) = ScreenToWorld(e.X, e.Y);
            project.figure.points[selection].x = oldPointX + newCursorX - oldCursorX;
            project.figure.points[selection].y = oldPointY + newCursorY - oldCursorY;
            Invalidate();
        }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        if (e.Button == MouseButtons.Middle)
        {
            cameraMoving = false;
        }
        else if (e.Button == MouseButtons.Right)
        {
            pointMoving = false;
            Cursor.Show();
        }
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
        base.OnMouseWheel(e);
        if (e.Delta > 0)
        {
            AdjustOffset(e.X, e.Y, ZoomStep);
            scale *= ZoomStep;
        }
        else
        {
            AdjustOffset(e.X, e.Y, 1 / ZoomStep);
            scale /= ZoomStep;
        }
        Invalidate();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.KeyCode == Keys.H && selection >= 0)
        {
            project.figure.points[selection].enabled = false;
            selection = -1;
            Invalidate();
        }
        else if (e.KeyCode == Keys.J)
        {
            for (int i = 0; i < project.figure.points.Length; i++)
            {
                project.figure.points[i].enabled = true;
            }
            Invalidate();
        }
    }
}