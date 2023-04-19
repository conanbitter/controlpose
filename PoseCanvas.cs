class PoseCanvas : Control
{
    private static Color BackgroundColor = Color.Black;
    private SolidBrush brush = new SolidBrush(Color.White);
    private Pen pen = new Pen(Color.White);
    private Figure fig;

    public PoseCanvas()
    {
        pen.Width = 8;
        this.DoubleBuffered = true;
        fig = new Figure();
        fig.ResetPose();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        e.Graphics.Clear(BackgroundColor);
        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        for (int i = 0; i < Figure.pairs.Length; i++)
        {
            var pair = Figure.pairs[i];
            pen.Color = Figure.pointColors[i];
            pen.Color = Color.FromArgb(153, pen.Color);
            e.Graphics.DrawLine(
                pen,
                fig.points[pair.Item1].x * 1024,
                fig.points[pair.Item1].y * 1024,
                fig.points[pair.Item2].x * 1024,
                fig.points[pair.Item2].y * 1024);
        }
        for (int i = 0; i < fig.points.Length; i++)
        {
            brush.Color = Figure.pointColors[i];
            brush.Color = Color.FromArgb(153, brush.Color);
            FigurePoint point = fig.points[i];
            e.Graphics.FillEllipse(brush, point.x * 1024 - 8, point.y * 1024 - 8, 16, 16);
        }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
    }

    protected override void OnMouseWheel(MouseEventArgs e)
    {
        base.OnMouseWheel(e);
    }
}