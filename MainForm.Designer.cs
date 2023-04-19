namespace controlpose;

partial class MainForm
{
    private PoseCanvas pcCanvas;
    private Panel pToolBox;
    private void InitializeComponent()
    {
        this.SuspendLayout();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1280, 720);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "ControlPose";
        this.Name = "ControlPose";

        pcCanvas = new PoseCanvas();
        pcCanvas.Dock = DockStyle.Fill;
        this.Controls.Add(pcCanvas);

        pToolBox = new Panel();
        pToolBox.Size = new Size(200, 100);
        pToolBox.Dock = DockStyle.Right;
        this.Controls.Add(pToolBox);
        this.ResumeLayout(false);
    }
}
