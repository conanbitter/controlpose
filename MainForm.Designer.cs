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
    private Button bAddImage;
    private Button bRemImage;
    private OpenFileDialog ofdLoadProject;
    private SaveFileDialog sfdSaveProject;
    private OpenFileDialog ofdLoadImage;

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
                project.SaveProject(sfdSaveProject.FileName);
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
                project.LoadProject(ofdLoadProject.FileName);
                pcCanvas.metadata.ClearSelection();
                lvPoints.SelectedIndices.Clear();
                pcCanvas.ResetCamera();
                pcCanvas.Invalidate();
            }
        };
        pToolBox.Controls.Add(bLoad);

        ofdLoadImage = new OpenFileDialog()
        {

            Filter = "PNG image (*.png)|*.png|JPEG image (*.jpg;*.jpeg)|*.jpg;*.jpeg",
            Title = "Load image",
            CheckFileExists = true,
            CheckPathExists = true,
        };

        bAddImage = new Button();
        bAddImage.Size = new Size(100, 40);
        bAddImage.Location = new Point(30, 230);
        bAddImage.Text = "Set Image";
        bAddImage.Click += (object sender, System.EventArgs e) =>
        {
            if (ofdLoadImage.ShowDialog() == DialogResult.OK)
            {
                project.LoadImage(ofdLoadImage.FileName);
                pcCanvas.Invalidate();
            }
        };
        pToolBox.Controls.Add(bAddImage);

        bRemImage = new Button();
        bRemImage.Size = new Size(100, 40);
        bRemImage.Location = new Point(30, 280);
        bRemImage.Text = "Remove Image";
        bRemImage.Click += (object sender, System.EventArgs e) =>
        {
            project.haveImage = false;
            pcCanvas.Invalidate();
        };
        pToolBox.Controls.Add(bRemImage);

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
        lvPoints.Location = new Point(30, 330);
        lvPoints.ItemChecked += new ItemCheckedEventHandler(UpdateCanvasVisibility);
        lvPoints.SelectedIndexChanged += new EventHandler(UpdateCanvasSelection);
        pToolBox.Controls.Add(lvPoints);

        this.ResumeLayout(false);
    }
}
