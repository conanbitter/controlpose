class PoseCanvas : Control
{
    private static Color BackgroundColor = Color.FromArgb(40, 40, 40);
    private SolidBrush brush = new SolidBrush(Color.White);
    private Pen pen = new Pen(Color.White);
    private Figure fig;

    private double offsetX = 0;
    private double offsetY = 0;
    private double scale = 1.0 / 512.0;

    private double offsetXold = 0;
    private double offsetYold = 0;
    private bool dragging = false;
    private int oldx = 0;
    private int oldy = 0;

    private bool initialResize = true;
    private Size oldSize = Size.Empty;

    private int canvasWidth = 512;
    private int canvasHeight = 512;
    private double canvasWidthF = 1.0;
    private double canvasHeightF = 1.0;

    const double ZoomStep = 0.9;

    public PoseCanvas()
    {
        pen.Width = 8;
        this.DoubleBuffered = true;
        fig = new Figure();
        fig.ResetPose();
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
        if (canvasWidthF / canvasHeightF < (double)Width / (double)(Height))
        {
            scale = canvasHeightF / (double)Height;
            offsetX = (Width - canvasWidthF / scale) / 2;
            offsetY = 0;
        }
        else
        {
            scale = canvasWidthF / (double)Width;
            offsetX = 0;
            offsetY = (Height - canvasHeightF / scale) / 2;
        }
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
        e.Graphics.FillRectangle(brush, (int)offsetX, (int)offsetY, (int)(canvasWidthF / scale), (int)(canvasHeightF / scale));
        for (int i = 0; i < Figure.pairs.Length; i++)
        {
            var pair = Figure.pairs[i];
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
            brush.Color = Figure.pointColors[i];
            brush.Color = Color.FromArgb(153, brush.Color);
            var point = WorldToScreen(fig.points[i].x, fig.points[i].y);
            e.Graphics.FillEllipse(brush, point.Item1 - 8, point.Item2 - 8, 16, 16);
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        if (e.Button == MouseButtons.Middle)
        {
            dragging = true;
            oldx = e.X;
            oldy = e.Y;
            offsetXold = offsetX;
            offsetYold = offsetY;
        }
        else
        {

        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (dragging)
        {
            int dx = e.X - oldx;
            int dy = e.Y - oldy;
            offsetX = offsetXold + e.X - oldx;
            offsetY = offsetYold + e.Y - oldy;
            Invalidate();
        }
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        dragging = false;
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
}