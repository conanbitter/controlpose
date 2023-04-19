class PoseCanvas : Control
{
    private static Color BackgroundColor = Color.Black;
    private SolidBrush brush = new SolidBrush(Color.White);
    private Pen pen = new Pen(Color.White);
    private Figure fig;

    public PoseCanvas()
    {
        pen.Width = 4;
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
            pen.Color = Figure.pointColors[i + 1];
            pen.Color = Color.FromArgb(128, Figure.pointColors[i + 1]);
            e.Graphics.DrawLine(
                pen,
                fig.points[pair.Item1].x * 512,
                fig.points[pair.Item1].y * 512,
                fig.points[pair.Item2].x * 512,
                fig.points[pair.Item2].y * 512);
        }
        for (int i = 0; i < fig.points.Length; i++)
        {
            brush.Color = Figure.pointColors[i];
            brush.Color = Color.FromArgb(128, Figure.pointColors[i]);
            FigurePoint point = fig.points[i];
            e.Graphics.FillEllipse(brush, point.x * 512 - 3, point.y * 512 - 3, 6, 6);
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