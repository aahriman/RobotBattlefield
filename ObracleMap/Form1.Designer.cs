using System.Drawing;

namespace ObracleMap {
    partial class Form1 {
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
                battlefieldImage.Dispose();
                backgroundBrush.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.battlefield = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.loadedFileName = new System.Windows.Forms.Label();
            this.loadButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.squareStyleRadioButton = new System.Windows.Forms.RadioButton();
            this.crossStyleRadioButton = new System.Windows.Forms.RadioButton();
            this.horizontalLineStyleRadioButton = new System.Windows.Forms.RadioButton();
            this.verticalLineStyleRadioButton = new System.Windows.Forms.RadioButton();
            this.previewStyle = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.styleSize = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.battlefield)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewStyle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.styleSize)).BeginInit();
            this.SuspendLayout();
            // 
            // battlefield
            // 
            this.battlefield.Dock = System.Windows.Forms.DockStyle.Fill;
            this.battlefield.Location = new System.Drawing.Point(0, 0);
            this.battlefield.Name = "battlefield";
            this.battlefield.Size = new System.Drawing.Size(1283, 566);
            this.battlefield.TabIndex = 1;
            this.battlefield.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.loadedFileName);
            this.panel1.Controls.Add(this.loadButton);
            this.panel1.Controls.Add(this.saveButton);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.previewStyle);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.styleSize);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1083, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 566);
            this.panel1.TabIndex = 2;
            // 
            // loadedFileName
            // 
            this.loadedFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loadedFileName.AutoSize = true;
            this.loadedFileName.Location = new System.Drawing.Point(4, 30);
            this.loadedFileName.Name = "loadedFileName";
            this.loadedFileName.Size = new System.Drawing.Size(61, 13);
            this.loadedFileName.TabIndex = 9;
            this.loadedFileName.Text = "(loaded file)";
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(103, 4);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(85, 23);
            this.loadButton.TabIndex = 8;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(7, 4);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(87, 23);
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(7, 371);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(181, 186);
            this.listBox1.TabIndex = 6;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.squareStyleRadioButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.crossStyleRadioButton, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.horizontalLineStyleRadioButton, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.verticalLineStyleRadioButton, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 82);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 54);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // squareStyleRadioButton
            // 
            this.squareStyleRadioButton.AutoSize = true;
            this.squareStyleRadioButton.Location = new System.Drawing.Point(3, 3);
            this.squareStyleRadioButton.Name = "squareStyleRadioButton";
            this.squareStyleRadioButton.Size = new System.Drawing.Size(59, 17);
            this.squareStyleRadioButton.TabIndex = 0;
            this.squareStyleRadioButton.Text = "Square";
            this.squareStyleRadioButton.UseVisualStyleBackColor = true;
            // 
            // crossStyleRadioButton
            // 
            this.crossStyleRadioButton.AutoSize = true;
            this.crossStyleRadioButton.Location = new System.Drawing.Point(103, 3);
            this.crossStyleRadioButton.Name = "crossStyleRadioButton";
            this.crossStyleRadioButton.Size = new System.Drawing.Size(51, 17);
            this.crossStyleRadioButton.TabIndex = 1;
            this.crossStyleRadioButton.Text = "Cross";
            this.crossStyleRadioButton.UseVisualStyleBackColor = true;
            // 
            // horizontalLineStyleRadioButton
            // 
            this.horizontalLineStyleRadioButton.AutoSize = true;
            this.horizontalLineStyleRadioButton.Location = new System.Drawing.Point(3, 30);
            this.horizontalLineStyleRadioButton.Name = "horizontalLineStyleRadioButton";
            this.horizontalLineStyleRadioButton.Size = new System.Drawing.Size(91, 17);
            this.horizontalLineStyleRadioButton.TabIndex = 2;
            this.horizontalLineStyleRadioButton.TabStop = true;
            this.horizontalLineStyleRadioButton.Text = "Horizontal line";
            this.horizontalLineStyleRadioButton.UseVisualStyleBackColor = true;
            // 
            // verticalLineStyleRadioButton
            // 
            this.verticalLineStyleRadioButton.AutoSize = true;
            this.verticalLineStyleRadioButton.Location = new System.Drawing.Point(103, 30);
            this.verticalLineStyleRadioButton.Name = "verticalLineStyleRadioButton";
            this.verticalLineStyleRadioButton.Size = new System.Drawing.Size(79, 17);
            this.verticalLineStyleRadioButton.TabIndex = 3;
            this.verticalLineStyleRadioButton.TabStop = true;
            this.verticalLineStyleRadioButton.Text = "Vertical line";
            this.verticalLineStyleRadioButton.UseVisualStyleBackColor = true;
            // 
            // previewStyle
            // 
            this.previewStyle.Location = new System.Drawing.Point(7, 165);
            this.previewStyle.Name = "previewStyle";
            this.previewStyle.Size = new System.Drawing.Size(184, 200);
            this.previewStyle.TabIndex = 4;
            this.previewStyle.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Size";
            // 
            // styleSize
            // 
            this.styleSize.Location = new System.Drawing.Point(37, 142);
            this.styleSize.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.styleSize.Name = "styleSize";
            this.styleSize.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.styleSize.Size = new System.Drawing.Size(120, 20);
            this.styleSize.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1283, 566);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.battlefield);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.battlefield)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewStyle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.styleSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox battlefield;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton crossStyleRadioButton;
        private System.Windows.Forms.RadioButton squareStyleRadioButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown styleSize;
        private System.Windows.Forms.PictureBox previewStyle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton horizontalLineStyleRadioButton;
        private System.Windows.Forms.RadioButton verticalLineStyleRadioButton;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label loadedFileName;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button saveButton;
    }
}

