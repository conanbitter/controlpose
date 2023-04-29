namespace controlpose;

partial class MainForm
{
    private PoseCanvas pcCanvas;
    private Panel pToolBox;
    private Button bCameraReset;
    private Button bRender;
    private ListView lvPoints;
    private Button bSave;
    private Button bLoad;
    private OpenFileDialog ofdLoadProject;
    private SaveFileDialog sfdSaveProject;

    public static Color ColorBlend(Color color, Color backColor, double amount)
    {
        byte r = (byte)(color.R * amount + backColor.R * (1 - amount));
        byte g = (byte)(color.G * amount + backColor.G * (1 - amount));
        byte b = (byte)(color.B * amount + backColor.B * (1 - amount));
        return Color.FromArgb(r, g, b);
    }

    private void InitializeComponent()
    {
        this.SuspendLayout();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1280, 720);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "ControlPose";
        this.Name = "ControlPose";

        pcCanvas = new PoseCanvas(project);
        pcCanvas.Dock = DockStyle.Fill;
        pcCanvas.VisibilityChanged += UpdateListVisibility;
        pcCanvas.SelectionChanged += UpdateListSelection;
        this.Controls.Add(pcCanvas);

        pToolBox = new Panel();
        pToolBox.Size = new Size(200, 100);
        pToolBox.Dock = DockStyle.Right;
        this.Controls.Add(pToolBox);

        bCameraReset = new Button();
        bCameraReset.Size = new Size(100, 40);
        bCameraReset.Location = new Point(30, 30);
        bCameraReset.Text = "Reset Camera";
        bCameraReset.Click += (object sender, System.EventArgs e) => pcCanvas.ResetCamera();
        pToolBox.Controls.Add(bCameraReset);

        bRender = new Button();
        bRender.Size = new Size(100, 40);
        bRender.Location = new Point(30, 80);
        bRender.Text = "Render";
        bRender.Click += new EventHandler(RenderImage);
        pToolBox.Controls.Add(bRender);

        sfdSaveProject = new SaveFileDialog()
        {
            Filter = "Project files (*.cps)|*.cps",
            Title = "Save project",
            CheckPathExists = true,
        };

        bSave = new Button();
        bSave.Size = new Size(100, 40);
        bSave.Location = new Point(30, 130);
        bSave.Text = "Save Project";
        bSave.Click += (object sender, System.EventArgs e) =>
        {
            if (sfdSaveProject.ShowDialog() == DialogResult.OK)
            {
                project.Save(sfdSaveProject.FileName);
            }
        };
        pToolBox.Controls.Add(bSave);

        ofdLoadProject = new OpenFileDialog()
        {
            Filter = "Project files (*.cps)|*.cps",
            Title = "Load project",
            CheckFileExists = true,
            CheckPathExists = true,
        };

        bLoad = new Button();
        bLoad.Size = new Size(100, 40);
        bLoad.Location = new Point(30, 180);
        bLoad.Text = "Load Project";
        bLoad.Click += (object sender, System.EventArgs e) =>
        {
            if (ofdLoadProject.ShowDialog() == DialogResult.OK)
            {
                project.Load(ofdLoadProject.FileName);
                pcCanvas.metadata.ClearSelection();
                lvPoints.SelectedIndices.Clear();
                pcCanvas.ResetCamera();
                pcCanvas.Invalidate();
            }
        };
        pToolBox.Controls.Add(bLoad);

        lvPoints = new ListView();
        lvPoints.View = View.Details;
        lvPoints.GridLines = true;
        lvPoints.Columns.Add("Name", 115);
        lvPoints.HeaderStyle = ColumnHeaderStyle.None;
        lvPoints.CheckBoxes = true;
        lvPoints.FullRowSelect = true;
        lvPoints.MultiSelect = true;
        for (int i = 0; i < Figure.pointNames.Length; i++)
        {
            ListViewItem item = new ListViewItem(Figure.pointNames[i]);
            item.Checked = true;
            item.BackColor = ColorBlend(Figure.pointColors[i], item.BackColor, 0.3);
            lvPoints.Items.Add(item);
        }
        lvPoints.Width = 120;
        lvPoints.ClientSize = new Size(
            lvPoints.ClientSize.Width,
            lvPoints.Items[17].Bounds.Bottom
        );
        lvPoints.Location = new Point(30, 230);
        lvPoints.ItemChecked += new ItemCheckedEventHandler(UpdateCanvasVisibility);
        lvPoints.SelectedIndexChanged += new EventHandler(UpdateCanvasSelection);
        pToolBox.Controls.Add(lvPoints);

        this.ResumeLayout(false);
    }
}
