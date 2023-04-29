using System.Drawing.Imaging;

public class ProjectData
{
    private int _canwasWidth = 512;
    private int _canwasHeight = 512;
    public Figure figure;
    public double canvasFloatWidth { get; private set; } = 1.0;
    public double canvasFloatHeight { get; private set; } = 1.0;

    public Image? image;
    public bool haveImage = false;
    public int imageWidth;
    public int imageHeight;
    public double imageFloatWidth;
    public double imageFloatHeight;
    public double imageScale;
    public double imageOffsetX;
    public double imageOffsetY;
    //public double 
    private ColorMatrix colorMatrix;
    public ImageAttributes imageAttributes;

    public int canvasWidth
    {
        get { return _canwasWidth; }
        set { _canwasWidth = value; (canvasFloatWidth, canvasFloatHeight) = updateAR(_canwasWidth, _canwasHeight); }
    }
    public int canvasHeight
    {
        get { return _canwasHeight; }
        set { _canwasHeight = value; (canvasFloatWidth, canvasFloatHeight) = updateAR(_canwasWidth, _canwasHeight); }
    }

    public ProjectData()
    {
        figure = new Figure();
        figure.ResetPose();
        colorMatrix = new ColorMatrix();
        colorMatrix.Matrix33 = 0.5f;
        imageAttributes = new ImageAttributes();
        imageAttributes.SetColorMatrix(
            colorMatrix,
            ColorMatrixFlag.Default,
            ColorAdjustType.Bitmap
        );
    }

    private (double, double) updateAR(int width, int height)
    {
        if (width > height)
        {
            return ((double)width / (double)height, 1.0);
        }
        if (width < height)
        {
            return (1.0, (double)height / (double)width);
        }
        return (1.0, 1.0);
    }

    public void LoadImage(string filename)
    {
        image = Image.FromFile(filename);
        imageWidth = image.Width;
        imageHeight = image.Height;
        (imageFloatWidth, imageFloatHeight) = updateAR(imageWidth, imageHeight);
        imageOffsetX = 0.0;
        imageOffsetY = 0.0;
        imageScale = 1.0;
        //MessageBox.Show($"w: {imageWidth}, h:{imageHeight}, fw:{imageFloatWidth}, fh:{imageFloatHeight}");
        haveImage = true;
    }

    public void SaveProject(string filename)
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

    public void LoadProject(string filename)
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