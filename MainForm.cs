namespace controlpose;

public partial class MainForm : Form
{
    public ProjectData project;

    public MainForm()
    {
        project = new ProjectData();
        InitializeComponent();
    }

    public void RenderImage(object sender, System.EventArgs e)
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "PNG Image|*.png";
        saveFileDialog.Title = "Save an Image";
        saveFileDialog.ShowDialog();

        if (saveFileDialog.FileName != "")
        {
            Renderer renderer = new Renderer(project.canvasWidth, project.canvasHeight);
            renderer.DrawFigure(project.figure);
            renderer.Save(saveFileDialog.FileName);
        }
    }
}
