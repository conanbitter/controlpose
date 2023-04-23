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

    public void ApplyFigureVisibilityToList(object sender, Figure figure)
    {
        for (int i = 0; i < figure.points.Length; i++)
        {
            lvPoints.Items[i].Checked = figure.points[i].enabled;
        }
    }

    public void ApplyFigureSelectionToList(object sender, int index)
    {
        lvPoints.SelectedIndices.Clear();
        if (index >= 0)
        {
            lvPoints.Items[index].Selected = true;
        }
        lvPoints.Select();
    }

    public void ApplyVisibilityToFigure(object sender, System.Windows.Forms.ItemCheckedEventArgs e)
    {
        for (int i = 0; i < project.figure.points.Length; i++)
        {
            project.figure.points[i].enabled = lvPoints.Items[i].Checked;
        }
        if (pcCanvas.selection >= 0 && !project.figure.points[pcCanvas.selection].enabled)
        {
            pcCanvas.selection = -1;
        }
        pcCanvas.Invalidate();
    }

    public void ApplySelectionToFigure(object sender, System.EventArgs e)
    {
        if (lvPoints.SelectedIndices.Count > 0 && project.figure.points[lvPoints.SelectedIndices[0]].enabled)
        {
            pcCanvas.selection = lvPoints.SelectedIndices[0];
        }
        else
        {
            pcCanvas.selection = -1;
        }
        pcCanvas.Invalidate();
    }
}
