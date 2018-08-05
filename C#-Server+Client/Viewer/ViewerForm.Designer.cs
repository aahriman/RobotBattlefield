namespace Viewer {
    partial class ViewerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.fileChooser = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.choosedFile = new System.Windows.Forms.Label();
            this.topControlPanel = new System.Windows.Forms.Panel();
            this.resetButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.playButton = new System.Windows.Forms.Button();
            this.nextTurnButton = new System.Windows.Forms.Button();
            this.previousButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.animationPanel = new System.Windows.Forms.Panel();
            this.bottomContolPanel = new System.Windows.Forms.Panel();
            this.fpsPanel = new System.Windows.Forms.Panel();
            this.delayLabel = new System.Windows.Forms.Label();
            this.delayNumber = new System.Windows.Forms.NumericUpDown();
            this.topControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.animationPanel.SuspendLayout();
            this.bottomContolPanel.SuspendLayout();
            this.fpsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.delayNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // fileChooser
            // 
            this.fileChooser.Dock = System.Windows.Forms.DockStyle.Left;
            this.fileChooser.Location = new System.Drawing.Point(0, 0);
            this.fileChooser.Name = "fileChooser";
            this.fileChooser.Size = new System.Drawing.Size(75, 28);
            this.fileChooser.TabIndex = 0;
            this.fileChooser.Text = "Choose file";
            this.fileChooser.UseVisualStyleBackColor = true;
            this.fileChooser.Click += new System.EventHandler(this.fileChooser_Click);
            // 
            // choosedFile
            // 
            this.choosedFile.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.choosedFile.AutoSize = true;
            this.choosedFile.Location = new System.Drawing.Point(81, 9);
            this.choosedFile.Name = "choosedFile";
            this.choosedFile.Size = new System.Drawing.Size(71, 13);
            this.choosedFile.TabIndex = 1;
            this.choosedFile.Text = "(Choosed file)";
            // 
            // topControlPanel
            // 
            this.topControlPanel.Controls.Add(this.resetButton);
            this.topControlPanel.Controls.Add(this.fileChooser);
            this.topControlPanel.Controls.Add(this.choosedFile);
            this.topControlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topControlPanel.Location = new System.Drawing.Point(0, 0);
            this.topControlPanel.Name = "topControlPanel";
            this.topControlPanel.Size = new System.Drawing.Size(1177, 28);
            this.topControlPanel.TabIndex = 3;
            // 
            // resetButton
            // 
            this.resetButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.resetButton.Location = new System.Drawing.Point(1102, 0);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(75, 28);
            this.resetButton.TabIndex = 2;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1177, 430);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // playButton
            // 
            this.playButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playButton.Location = new System.Drawing.Point(100, 0);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(882, 40);
            this.playButton.TabIndex = 4;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // nextTurnButton
            // 
            this.nextTurnButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.nextTurnButton.Location = new System.Drawing.Point(982, 0);
            this.nextTurnButton.Name = "nextTurnButton";
            this.nextTurnButton.Size = new System.Drawing.Size(75, 40);
            this.nextTurnButton.TabIndex = 6;
            this.nextTurnButton.Text = "Next turn";
            this.nextTurnButton.UseVisualStyleBackColor = true;
            this.nextTurnButton.Click += new System.EventHandler(this.nextTurnButton_Click);
            // 
            // previousButton
            // 
            this.previousButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.previousButton.Location = new System.Drawing.Point(0, 0);
            this.previousButton.Name = "previousButton";
            this.previousButton.Size = new System.Drawing.Size(100, 40);
            this.previousButton.TabIndex = 3;
            this.previousButton.Text = "Previous Turn";
            this.previousButton.UseVisualStyleBackColor = true;
            this.previousButton.Click += new System.EventHandler(this.previousButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pauseButton.Location = new System.Drawing.Point(100, 0);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(882, 40);
            this.pauseButton.TabIndex = 5;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Visible = false;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // animationPanel
            // 
            this.animationPanel.Controls.Add(this.playButton);
            this.animationPanel.Controls.Add(this.pauseButton);
            this.animationPanel.Controls.Add(this.previousButton);
            this.animationPanel.Controls.Add(this.nextTurnButton);
            this.animationPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.animationPanel.Location = new System.Drawing.Point(0, 0);
            this.animationPanel.Name = "animationPanel";
            this.animationPanel.Size = new System.Drawing.Size(1057, 40);
            this.animationPanel.TabIndex = 9;
            // 
            // bottomContolPanel
            // 
            this.bottomContolPanel.Controls.Add(this.animationPanel);
            this.bottomContolPanel.Controls.Add(this.fpsPanel);
            this.bottomContolPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomContolPanel.Location = new System.Drawing.Point(0, 418);
            this.bottomContolPanel.Name = "bottomContolPanel";
            this.bottomContolPanel.Size = new System.Drawing.Size(1177, 40);
            this.bottomContolPanel.TabIndex = 10;
            // 
            // fpsPanel
            // 
            this.fpsPanel.Controls.Add(this.delayLabel);
            this.fpsPanel.Controls.Add(this.delayNumber);
            this.fpsPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.fpsPanel.Location = new System.Drawing.Point(1057, 0);
            this.fpsPanel.Name = "fpsPanel";
            this.fpsPanel.Size = new System.Drawing.Size(120, 40);
            this.fpsPanel.TabIndex = 10;
            // 
            // delayLabel
            // 
            this.delayLabel.AutoSize = true;
            this.delayLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.delayLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.delayLabel.Location = new System.Drawing.Point(0, 0);
            this.delayLabel.Name = "delayLabel";
            this.delayLabel.Size = new System.Drawing.Size(67, 25);
            this.delayLabel.TabIndex = 3;
            this.delayLabel.Text = "Delay";
            // 
            // delayNumber
            // 
            this.delayNumber.Dock = System.Windows.Forms.DockStyle.Right;
            this.delayNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.delayNumber.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.delayNumber.Location = new System.Drawing.Point(67, 0);
            this.delayNumber.Name = "delayNumber";
            this.delayNumber.Size = new System.Drawing.Size(53, 35);
            this.delayNumber.TabIndex = 2;
            this.delayNumber.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.delayNumber.ValueChanged += new System.EventHandler(this.delayNumber_ValueChanged);
            // 
            // ViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1177, 458);
            this.Controls.Add(this.bottomContolPanel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.topControlPanel);
            this.Name = "ViewerForm";
            this.Text = "Viewer battlefield";
            this.topControlPanel.ResumeLayout(false);
            this.topControlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.animationPanel.ResumeLayout(false);
            this.bottomContolPanel.ResumeLayout(false);
            this.fpsPanel.ResumeLayout(false);
            this.fpsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.delayNumber)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button fileChooser;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label choosedFile;
        private System.Windows.Forms.Panel topControlPanel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button nextTurnButton;
        private System.Windows.Forms.Button previousButton;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Panel animationPanel;
        private System.Windows.Forms.Panel bottomContolPanel;
        private System.Windows.Forms.NumericUpDown delayNumber;
        private System.Windows.Forms.Panel fpsPanel;
        private System.Windows.Forms.Label delayLabel;
    }
}

