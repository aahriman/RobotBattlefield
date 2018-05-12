using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using BaseLibrary.utils;
using ViewerLibrary.gui;
using ViewerLibrary.model;

namespace Viewer {
    public partial class ViewerForm : Form {

        static ViewerForm() {
            ModUtils.LoadMods();
        }
        private ReversibleRender render;

        private FileTurnDataModel dataModel;

        public ViewerForm() {
            InitializeComponent();
        }

        private void fileChooser_Click(object sender, EventArgs e) {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != null && !string.Empty.Equals(openFileDialog1.FileName)) {
                choosedFile.Text = openFileDialog1.FileName;
                dataModel = new FileTurnDataModel(choosedFile.Text, 20);
                render = new ReversibleRender(dataModel, pictureBox1, new DefaultDrawer());
            }
        }

        private void playButton_Click(object sender, EventArgs e) {
            if (render == null) return;
            render.Play();
            playButton.Visible = false;
            pauseButton.Visible = true;
        }

        private void resetButton_Click(object sender, EventArgs e) {
            if (render == null) return;
            pictureBox1.Image = null;
            dataModel.Reset();
            pauseButton.Visible = false;
            playButton.Visible = true;
        }

        private void previousButton_Click(object sender, EventArgs e) {
            if (render == null) return;
            render.StepPrevious();
            pauseButton.Visible = false;
            playButton.Visible = true;
        }

        private void nextTurnButton_Click(object sender, EventArgs e) {
            if (render == null) return;
            render.StepNext();
            pauseButton.Visible = false;
            playButton.Visible = true;
        }

        private void pauseButton_Click(object sender, EventArgs e) {
            if (render == null) return;
            render.Pause();
            playButton.Visible = true;
            pauseButton.Visible = false;
        }

        private void delayNumber_ValueChanged(object sender, EventArgs e)
        {
            if (render == null) return;
            render.Delay = (int) delayNumber.Value;
        }
    }
}
