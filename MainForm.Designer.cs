namespace controlpose;

partial class MainForm
{
    private PoseCanvas pcCanvas;
    private Panel pToolBox;
    private Button bCameraReset;
    private Button bRender;
    private ListView lvPoints;

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
        pcCanvas.VisibilityChanged += ApplyFigureVisibilityToList;
        pcCanvas.SelectionChanged += ApplyFigureSelectionToList;
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

        lvPoints = new ListView();
        lvPoints.View = View.List;
        lvPoints.CheckBoxes = true;
        lvPoints.FullRowSelect = true;
        lvPoints.MultiSelect = false;
        for (int i = 0; i < Figure.pointNames.Length; i++)
        //foreach (string name in Figure.pointNames)
        {
            ListViewItem item = new ListViewItem(Figure.pointNames[i]);
            item.Checked = true;
            item.BackColor = ColorBlend(Figure.pointColors[i], item.BackColor, 0.3);
            lvPoints.Items.Add(item);
        }
        lvPoints.Size = new Size(120, 400);
        lvPoints.Location = new Point(30, 130);
        lvPoints.ItemChecked += new ItemCheckedEventHandler(ApplyVisibilityToFigure);
        lvPoints.SelectedIndexChanged += new EventHandler(ApplySelectionToFigure);
        pToolBox.Controls.Add(lvPoints);

        this.ResumeLayout(false);
    }
}
