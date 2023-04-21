public struct FigurePoint
{
    public double x;
    public double y;
    public bool enabled = true;

    public FigurePoint(double x, double y)
    {
        this.x = x;
        this.y = y;
    }
}

public class Figure
{
    static FigurePoint[] defaultPose = new FigurePoint[18]{
        new FigurePoint( 0.000000, -0.393345),
        new FigurePoint( 0.000000, -0.296005),
        new FigurePoint(-0.083981, -0.292099),
        new FigurePoint(-0.114137, -0.147260),
        new FigurePoint(-0.154135, -0.027264),
        new FigurePoint( 0.083981, -0.292099),
        new FigurePoint( 0.114137, -0.147260),
        new FigurePoint( 0.154135, -0.027264),
        new FigurePoint(-0.054139, -0.032889),
        new FigurePoint(-0.076013,  0.188353),
        new FigurePoint(-0.086013,  0.411469),
        new FigurePoint( 0.054139, -0.032889),
        new FigurePoint( 0.076013,  0.188353),
        new FigurePoint( 0.086013,  0.411469),
        new FigurePoint(-0.017108, -0.411469),
        new FigurePoint( 0.017108, -0.411469),
        new FigurePoint(-0.041482, -0.399751),
        new FigurePoint( 0.041482, -0.399751)
    };

    /*public static Color[] pointColors = new Color[18]{
        Color.FromArgb(255,   0,  85),
        Color.FromArgb(255,   0,   0),
        Color.FromArgb(255,  85,   0),
        Color.FromArgb(255, 170,   0),
        Color.FromArgb(255, 255,   0),
        Color.FromArgb(170, 255,   0),
        Color.FromArgb( 85, 255,   0),
        Color.FromArgb(  0, 255,   0),
        Color.FromArgb(  0, 255,  85),
        Color.FromArgb(  0, 255, 170),
        Color.FromArgb(  0, 255, 255),
        Color.FromArgb(  0, 170, 255),
        Color.FromArgb(  0,  85, 255),
        Color.FromArgb(  0,   0, 255),
        Color.FromArgb(255,   0, 170),
        Color.FromArgb(170,   0, 255),
        Color.FromArgb(255,   0, 255),
        Color.FromArgb(85,    0, 255)
    };*/
    public static Color[] pointColors = new Color[18]{
        Color.FromArgb(255,   0,   0),
        Color.FromArgb(255,  85,   0),
        Color.FromArgb(255, 170,   0),
        Color.FromArgb(255, 255,   0),
        Color.FromArgb(170, 255,   0),
        Color.FromArgb( 85, 255,   0),
        Color.FromArgb(  0, 255,   0),
        Color.FromArgb(  0, 255,  85),
        Color.FromArgb(  0, 255, 170),
        Color.FromArgb(  0, 255, 255),
        Color.FromArgb(  0, 170, 255),
        Color.FromArgb(  0,  85, 255),
        Color.FromArgb(  0,   0, 255),
        Color.FromArgb(85,    0, 255),
        Color.FromArgb(170,   0, 255),
        Color.FromArgb(255,   0, 170),
        Color.FromArgb(255,   0, 255),
        Color.FromArgb(255,   0,  85),
    };

    public static (int, int)[] pairs = new (int, int)[]{
        (1,2),
        (1,5),
        (2,3),
        (3,4),
        (5,6),
        (6,7),
        (1,8),
        (8,9),
        (9,10),
        (1,11),
        (11,12),
        (12,13),
        (1,0),
        (0,14),
        (14,16),
        (0,15),
        (15,17),
    };

    public FigurePoint[] points = new FigurePoint[18];

    public void ResetPose()
    {
        Array.Copy(defaultPose, points, defaultPose.Length);
    }
}