namespace controlpose;

public partial class MainForm : Form
{
    public ProjectData project;

    bool selectionChanging = false;

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

    public void UpdateListVisibility(object sender, Figure figure)
    {
        for (int i = 0; i < figure.points.Length; i++)
        {
            lvPoints.Items[i].Checked = figure.points[i].enabled;
        }
    }

    public void UpdateListSelection(object sender, FigureMetadata metadata)
    {
        selectionChanging = true;
        for (int i = 0; i < metadata.points.Length; i++)
        {
            lvPoints.Items[i].Selected = metadata.points[i].selected;
        }
        lvPoints.Select();
        selectionChanging = false;
    }

    public void UpdateCanvasVisibility(object sender, System.Windows.Forms.ItemCheckedEventArgs e)
    {
        bool selectionChanged = false;
        for (int i = 0; i < project.figure.points.Length; i++)
        {
            if (!lvPoints.Items[i].Checked && pcCanvas.metadata.points[i].selected)
            {
                selectionChanged = true;
                pcCanvas.metadata.points[i].selected = false;
            }
            project.figure.points[i].enabled = lvPoints.Items[i].Checked;

        }
        if (selectionChanged)
        {
            UpdateListSelection(this, pcCanvas.metadata);
            pcCanvas.Invalidate();
        }

    }

    public void UpdateCanvasSelection(object sender, System.EventArgs e)
    {
        if (selectionChanging)
        {
            return;
        }
        for (int i = 0; i < pcCanvas.metadata.points.Length; i++)
        {
            if (!project.figure.points[i].enabled)
            {
                lvPoints.Items[i].Selected = false;
            }
            pcCanvas.metadata.points[i].selected = lvPoints.Items[i].Selected;
        }
        pcCanvas.Invalidate();
    }
}
