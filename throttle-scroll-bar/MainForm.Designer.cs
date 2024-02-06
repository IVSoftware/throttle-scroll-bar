namespace throttle_scroll_bar
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox = new PictureBox();
            trackBar = new TrackBar();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)trackBar).BeginInit();
            SuspendLayout();
            // 
            // pictureBox
            // 
            pictureBox.Dock = DockStyle.Top;
            pictureBox.Location = new Point(10, 10);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(596, 525);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabIndex = 2;
            pictureBox.TabStop = false;
            // 
            // trackBar
            // 
            trackBar.Dock = DockStyle.Bottom;
            trackBar.Location = new Point(10, 564);
            trackBar.Maximum = 3000;
            trackBar.Name = "trackBar";
            trackBar.Size = new Size(596, 69);
            trackBar.TabIndex = 3;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Azure;
            ClientSize = new Size(616, 643);
            Controls.Add(trackBar);
            Controls.Add(pictureBox);
            Name = "MainForm";
            Padding = new Padding(10);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Main Form";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)trackBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private PictureBox pictureBox;
        private TrackBar trackBar;
    }
}
