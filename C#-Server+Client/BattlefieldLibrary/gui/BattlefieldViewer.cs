using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewerLibrary.gui;
using ViewerLibrary.model;

namespace BattlefieldLibrary.gui {
    public partial class BattlefieldViewer : Form {
        private readonly Render render;

        public BattlefieldViewer(ITurnDataModel turnDataModel) {
            InitializeComponent();
            render = new Render(turnDataModel, pictureBox1, new DefaultDrawer());
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) {
            if (render != null) {
                render.Delay = (int) numericUpDown1.Value;
            }
        }

        public void StepNext() {
            render.StepNext();
        }

        public void EnableStart() {
            if (startButton.InvokeRequired) {
                startButton.BeginInvoke(new Action( () => startButton.Enabled = true));
            } else {
                startButton.Enabled = true;
            }
        }

        private void startButton_Click(object sender, EventArgs e) {
            render.Play();

            if (startButton.InvokeRequired) {
                startButton.BeginInvoke(new Action(() => startButton.Visible = false));
            } else {
                startButton.Visible = false;
            }
            
        }
    }
}
