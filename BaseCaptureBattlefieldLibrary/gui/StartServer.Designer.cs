namespace RobotViewer.gui
{
    partial class StartServer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rabbitsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.countersNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.snipersNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.spotsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.ownsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.rabbitsNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.countersNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.snipersNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spotsNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ownsNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // rabbitsNumericUpDown
            // 
            this.rabbitsNumericUpDown.Location = new System.Drawing.Point(61, 5);
            this.rabbitsNumericUpDown.Name = "rabbitsNumericUpDown";
            this.rabbitsNumericUpDown.Size = new System.Drawing.Size(129, 20);
            this.rabbitsNumericUpDown.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Rabbits";
            // 
            // countersNumericUpDown
            // 
            this.countersNumericUpDown.Location = new System.Drawing.Point(61, 31);
            this.countersNumericUpDown.Name = "countersNumericUpDown";
            this.countersNumericUpDown.Size = new System.Drawing.Size(129, 20);
            this.countersNumericUpDown.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Counters";
            // 
            // snipersNumericUpDown
            // 
            this.snipersNumericUpDown.Location = new System.Drawing.Point(61, 55);
            this.snipersNumericUpDown.Name = "snipersNumericUpDown";
            this.snipersNumericUpDown.Size = new System.Drawing.Size(129, 20);
            this.snipersNumericUpDown.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Snipers";
            // 
            // spotsNumericUpDown
            // 
            this.spotsNumericUpDown.Location = new System.Drawing.Point(61, 82);
            this.spotsNumericUpDown.Name = "spotsNumericUpDown";
            this.spotsNumericUpDown.Size = new System.Drawing.Size(129, 20);
            this.spotsNumericUpDown.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Spots";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 134);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(175, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Start server";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ownsNumericUpDown
            // 
            this.ownsNumericUpDown.Location = new System.Drawing.Point(61, 108);
            this.ownsNumericUpDown.Name = "ownsNumericUpDown";
            this.ownsNumericUpDown.Size = new System.Drawing.Size(129, 20);
            this.ownsNumericUpDown.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Owns";
            // 
            // StartServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(202, 164);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ownsNumericUpDown);
            this.Controls.Add(this.spotsNumericUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.snipersNumericUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.countersNumericUpDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rabbitsNumericUpDown);
            this.Name = "StartServer";
            this.Text = "StartServer";
            ((System.ComponentModel.ISupportInitialize)(this.rabbitsNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.countersNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.snipersNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spotsNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ownsNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown rabbitsNumericUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown countersNumericUpDown;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown snipersNumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown spotsNumericUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown ownsNumericUpDown;
        private System.Windows.Forms.Label label5;

    }
}