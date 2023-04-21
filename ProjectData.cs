public class ProjectData
{
    public Figure figure;
    public int canvasWidth { get; } = 512;
    public int canvasHeight { get; } = 512;
    public double canvasFloatWidth { get; } = 1.0;
    public double canvasFloatHeight { get; } = 1.0;

    public ProjectData()
    {
        figure = new Figure();
        figure.ResetPose();
    }
}