public class ProjectData
{
    private int _canwasWidth = 512;
    private int _canwasHeight = 512;
    public Figure figure;
    public double canvasFloatWidth { get; private set; } = 1.0;
    public double canvasFloatHeight { get; private set; } = 1.0;

    public int canvasWidth
    {
        get { return _canwasWidth; }
        set { _canwasWidth = value; updateAR(); }
    }
    public int canvasHeight
    {
        get { return _canwasHeight; }
        set { _canwasHeight = value; updateAR(); }
    }

    public ProjectData()
    {
        figure = new Figure();
        figure.ResetPose();
    }

    private void updateAR()
    {
        if (canvasWidth > canvasHeight)
        {
            canvasFloatHeight = 1.0;
            canvasFloatWidth = (double)canvasWidth / (double)canvasHeight;
        }
        else if (canvasWidth < canvasHeight)
        {
            canvasFloatWidth = 1.0;
            canvasFloatHeight = (double)canvasHeight / (double)canvasWidth;
        }
    }

    public void Save(string filename)
    {
        using (BinaryWriter file = new BinaryWriter(new FileStream(filename, FileMode.Create)))
        {
            file.Write((byte)1); // version
            file.Write(canvasWidth); // canvas size
            file.Write(canvasHeight);
            file.Write((byte)1); // figure count
            file.Write(false); // have image

            foreach (FigurePoint point in figure.points) // figure data
            {
                file.Write(point.enabled);
                file.Write(point.x);
                file.Write(point.y);
            }

            //TODO: image data
        }
    }

    public void Load(string filename)
    {
        using (BinaryReader file = new BinaryReader(new FileStream(filename, FileMode.Open)))
        {
            int version = file.ReadByte();
            if (version != 1)
            {
                MessageBox.Show("Wrong file format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            canvasWidth = file.ReadInt32();
            canvasHeight = file.ReadInt32();

            int figureCount = file.ReadByte();
            bool hasImage = file.ReadBoolean();

            for (int i = 0; i < figure.points.Length; i++)
            {
                figure.points[i].enabled = file.ReadBoolean();
                figure.points[i].x = file.ReadDouble();
                figure.points[i].y = file.ReadDouble();
            }
        }
    }
}