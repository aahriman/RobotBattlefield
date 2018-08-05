using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViewerLibrary.gui;
using ViewerLibrary.model;

namespace BattlefieldLibrary.gui
{
    public partial class BattlefieldViewer : Form {
        private Render render;
        public BattlefieldViewer(ITurnDataModel turnDataModel)
        {
            InitializeComponent();
            render = new Render(turnDataModel, pictureBox1, new DefaultDrawer());
            render.Play();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) {
            if (render != null) {
                render.Delay = (int) numericUpDown1.Value;
            }
        }
    }
}
