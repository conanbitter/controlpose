public struct FigurePoint
{
    public float x;
    public float y;
    public bool enabled = true;

    public FigurePoint(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}

public class Figure
{
    static FigurePoint[] defaultPose = new FigurePoint[18]{
        new FigurePoint(0.500000f, 0.106655f),
        new FigurePoint(0.500000f, 0.203995f),
        new FigurePoint(0.416019f, 0.207901f),
        new FigurePoint(0.385863f, 0.352740f),
        new FigurePoint(0.345865f, 0.472736f),
        new FigurePoint(0.583981f, 0.207901f),
        new FigurePoint(0.614137f, 0.352740f),
        new FigurePoint(0.654135f, 0.472736f),
        new FigurePoint(0.445861f, 0.467111f),
        new FigurePoint(0.423987f, 0.688353f),
        new FigurePoint(0.413987f, 0.911469f),
        new FigurePoint(0.554139f, 0.467111f),
        new FigurePoint(0.576013f, 0.688353f),
        new FigurePoint(0.586013f, 0.911469f),
        new FigurePoint(0.482892f, 0.088531f),
        new FigurePoint(0.517108f, 0.088531f),
        new FigurePoint(0.458518f, 0.100249f),
        new FigurePoint(0.541482f, 0.100249f)
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