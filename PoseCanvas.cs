public delegate void VisibilityChangedEventHandler(object sender, Figure figure);
public delegate void SelectionChangedEventHandler(object sender, FigureMetadata metadata);

public struct PointMetadata
{
    public bool selected = false;
    public double oldX = 0.0;
    public double oldY = 0.0;

    public PointMetadata() { }
}

public class FigureMetadata
{
    public PointMetadata[] points = new PointMetadata[18];

    public void ToggleOne(int index)
    {
        points[index].selected = !points[index].selected;
    }

    public void ClearSelection()
    {
        for (int i = 0; i < points.Length; i++)
        {
            points[i].selected = false;
        }
    }

    public void CopyPos(Figure figure)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (!points[i].selected)
            {
                continue;
            }
            points[i].oldX = figure.points[i].x;
            points[i].oldY = figure.points[i].y;
        }
    }

    public bool HasSelection()
    {
        foreach (PointMetadata point in points)
        {
            if (point.selected)
            {
                return true;
            }
        }
        return false;
    }
}

class PoseCanvas : Control
{
    const double ZoomStep = 0.9;
    const double MinSelectDistance = 0.3;

    private static Color BackgroundColor = Color.FromArgb(40, 40, 40);
    private SolidBrush brush = new SolidBrush(Color.White);
    private Pen pen = new Pen(Color.White);
    private Pen selPen = new Pen(Color.White);
    private Pen boxPen = new Pen(Color.White, 2);

    private double offsetX = 0;
    private double offsetY = 0;
    private double scale = 1.0 / 512.0;

    private double offsetXold = 0;
    private double offsetYold = 0;
    private bool cameraMoving = false;
    private int oldx = 0;
    private int oldy = 0;

    private bool pointMoving = false;
    private double oldCursorX = 0.0;
    private double oldCursorY = 0.0;

    private bool boxSelection = false;
    private bool boxActive = false;
    private double selX1 = 0;
    private double selY1 = 0;
    private double selX2 = 0;
    private double selY2 = 0;
    private int boxTop = 0, boxLeft = 0, boxWidth = 0, boxHeight = 0;
    const int minBoxSize = 3;

    private bool initialResize = true;
    private Size oldSize = Size.Empty;

    public FigureMetadata metadata = new FigureMetadata();

    public ProjectData project;

    public event VisibilityChangedEventHandler? VisibilityChanged;
    public event SelectionChangedEventHandler? SelectionChanged;

    public PoseCanvas(ProjectData data)
    {
        pen.Width = 8;
        selPen.Width = 2;
        boxPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
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
        e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        brush.Color = Color.Black;

        (int canvasOffsetX, int canvasOffsetY) = WorldToScreen(0.5 - project.canvasFloatWidth, 0.5 - project.canvasFloatHeight);
        e.Graphics.FillRectangle(
            brush,
            canvasOffsetX,
            canvasOffsetY,
            (int)(project.canvasFloatWidth / scale),
            (int)(project.canvasFloatHeight / scale));

        if (project.haveImage)
        {
            (int imX, int imY) = WorldToScreen(
                project.imageOffsetX - project.imageFloatWidth * project.imageScale / 2.0,
                project.imageOffsetY - project.imageFloatHeight * project.imageScale / 2.0
                );
            int imWidth = (int)(project.imageFloatWidth * project.imageScale / scale);
            int imHeight = (int)(project.imageFloatHeight * project.imageScale / scale);
            //MessageBox.Show($"x: {imX}, y:{imY}, w:{imWidth}, h:{imHeight}");
            e.Graphics.DrawImage(
                project.image!,
                new Rectangle(imX, imY, imWidth, imHeight),
                0,
                0,
                project.imageWidth,
                project.imageHeight,
                GraphicsUnit.Pixel,
                project.imageAttributes);
        }

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

        for (int i = 0; i < fig.points.Length; i++)
        {
            if (metadata.points[i].selected)
            {
                var selPoint = WorldToScreen(fig.points[i].x, fig.points[i].y);
                e.Graphics.DrawEllipse(selPen, selPoint.Item1 - 10, selPoint.Item2 - 10, 20, 20);
            }
        }

        if (boxActive)
        {
            e.Graphics.DrawRectangle(boxPen, boxLeft, boxTop, boxWidth, boxHeight);
            //e.Graphics.DrawRectangle(boxPen, 100, 100, 400, 400);
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
        else if (e.Button == MouseButtons.Right && metadata.HasSelection())
        {
            pointMoving = true;
            Cursor.Hide();
            (oldCursorX, oldCursorY) = ScreenToWorld(e.X, e.Y);
            metadata.CopyPos(project.figure);
        }
        else if (e.Button == MouseButtons.Left && !pointMoving)
        {
            (selX1, selY1) = ScreenToWorld(e.X, e.Y);
            boxSelection = true;
        }
    }

    private void RecalculateBox()
    {
        (int x1, int y1) = WorldToScreen(selX1, selY1);
        (int x2, int y2) = WorldToScreen(selX2, selY2);
        boxLeft = Math.Min(x1, x2);
        boxTop = Math.Min(y1, y2);
        boxWidth = Math.Abs(x2 - x1);
        boxHeight = Math.Abs(y2 - y1);
        boxActive = (boxWidth > minBoxSize) && (boxHeight > minBoxSize);
        if (boxActive)
        {
            double boxBorder = 8.0 * scale;
            double left = Math.Min(selX1, selX2) - boxBorder;
            double right = Math.Max(selX1, selX2) + boxBorder;
            double top = Math.Min(selY1, selY2) - boxBorder;
            double bottom = Math.Max(selY1, selY2) + boxBorder;
            for (int i = 0; i < project.figure.points.Length; i++)
            {
                if (!project.figure.points[i].enabled)
                {
                    continue;
                }
                bool selected = project.figure.points[i].x > left &&
                    project.figure.points[i].x < right &&
                    project.figure.points[i].y > top &&
                    project.figure.points[i].y < bottom;
                if (Control.ModifierKeys.HasFlag(Keys.Shift))
                {
                    metadata.points[i].selected = metadata.points[i].selected || selected;
                }
                else
                {
                    metadata.points[i].selected = selected;
                }
            }
            if (SelectionChanged != null)
            {
                SelectionChanged(this, metadata);
            }
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
            for (int i = 0; i < project.figure.points.Length; i++)
            {
                if (metadata.points[i].selected)
                {
                    project.figure.points[i].x = metadata.points[i].oldX + newCursorX - oldCursorX;
                    project.figure.points[i].y = metadata.points[i].oldY + newCursorY - oldCursorY;
                }
            }
            Invalidate();
        }
        if (boxSelection)
        {
            (selX2, selY2) = ScreenToWorld(e.X, e.Y);
            RecalculateBox();
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
        else if (e.Button == MouseButtons.Left && !pointMoving)
        {
            boxSelection = false;
            if (boxActive)
            {
                boxActive = false;
            }
            else
            {
                if (!Control.ModifierKeys.HasFlag(Keys.Shift))
                {
                    metadata.ClearSelection();
                }
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
                        metadata.ToggleOne(i);
                    }
                }
                if (SelectionChanged != null)
                {
                    SelectionChanged(this, metadata);
                }
            }
            Invalidate();
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
        if (boxSelection)
        {
            RecalculateBox();
        }
        Invalidate();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        /*if (e.KeyCode == Keys.H && selection >= 0)
        {
            project.figure.points[selection].enabled = false;
            selection = -1;
            if (VisibilityChanged != null)
            {
                VisibilityChanged(this, project.figure);
            }
            Invalidate();
        }
        else if (e.KeyCode == Keys.J)
        {
            for (int i = 0; i < project.figure.points.Length; i++)
            {
                project.figure.points[i].enabled = true;
            }
            if (VisibilityChanged != null)
            {
                VisibilityChanged(this, project.figure);
            }
            Invalidate();
        }*/
    }
}