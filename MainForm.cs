namespace controlpose;

public partial class MainForm : Form
{
    public ProjectData project;

    public MainForm()
    {
        project = new ProjectData();
        InitializeComponent();
    }
}
