class Renderer
{
    public bool ellipticLimbs = true;
    public int jointDiameter = 14;
    public int limbWidth = 8;
    private int canvasWidth;
    private int canvasHeight;
    private SolidBrush brush = new SolidBrush(Color.White);
    private Pen pen = new Pen(Color.White);
    private Bitmap bitmap;
    private Graphics gfx;

    public Renderer(int width, int height)
    {
        canvasWidth = width;
        canvasHeight = height;
        bitmap = new Bitmap(canvasWidth, canvasHeight);
        gfx = Graphics.FromImage(bitmap);
        gfx.Clear(Color.Black);
        gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
    }

    public void DrawFigure(in Figure figure)
    {
        pen.Width = limbWidth;
        for (int i = 0; i < Figure.pairs.Length; i++)
        {
            var pair = Figure.pairs[i];
            if (!figure.points[pair.Item1].enabled || !figure.points[pair.Item2].enabled)
            {
                continue;
            }
            double x1 = (figure.points[pair.Item1].x + 0.5) * canvasWidth;
            double y1 = (figure.points[pair.Item1].y + 0.5) * canvasHeight;
            double x2 = (figure.points[pair.Item2].x + 0.5) * canvasWidth;
            double y2 = (figure.points[pair.Item2].y + 0.5) * canvasHeight;

            if (ellipticLimbs)
            {
                brush.Color = Color.FromArgb(153, Figure.pointColors[i]);
                double dx = x2 - x1;
                double dy = y2 - y1;
                double angle = Math.Atan2(dy, dx) * (180.0 / Math.PI);
                double length = Math.Sqrt(dx * dx + dy * dy);
                gfx.TranslateTransform((float)x1, (float)y1);
                gfx.RotateTransform((float)angle);
                gfx.FillEllipse(brush, 0.0f, -(float)limbWidth / 2, (float)length, limbWidth);
                gfx.ResetTransform();
            }
            else
            {
                pen.Color = Color.FromArgb(153, Figure.pointColors[i]);
                gfx.DrawLine(pen, (float)x1, (float)y1, (float)x2, (float)y2);
            }
        }

        for (int i = 0; i < figure.points.Length; i++)
        {
            if (!figure.points[i].enabled)
            {
                continue;
            }
            brush.Color = Color.FromArgb(255, Figure.pointColors[i]);
            double x = (figure.points[i].x + 0.5) * canvasWidth;
            double y = (figure.points[i].y + 0.5) * canvasHeight;
            gfx.FillEllipse(brush,
                (float)x - (float)jointDiameter / 2,
                (float)y - (float)jointDiameter / 2,
                jointDiameter,
                jointDiameter);
        }
    }

    public void Save(string filename)
    {
        bitmap.Save(filename);
    }
}