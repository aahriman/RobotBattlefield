using System;
using System.IO;
using System.Windows.Forms;

namespace Viewer {
    public partial class Form1 : Form {
        private StreamReader reader;

        public Form1() {
            InitializeComponent();
        }

        private void fileChooser_Click(object sender, EventArgs e) {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != null && !string.Empty.Equals(openFileDialog1.FileName)) {
                choosedFile.Text = openFileDialog1.FileName;
                startButton.Visible = true;
                stopButton.Visible = false;
            }
        }

        private void start_Click(object sender, EventArgs e) {
            alternateStartStopVisibility();
            reader = new StreamReader(File.OpenRead(openFileDialog1.FileName));
            draw().Start();

        }

        private void stop_Click(object sender, EventArgs e) {
            reader.Close();
            alternateStartStopVisibility();
        }

        private void alternateStartStopVisibility() {
            stopButton.Visible = !stopButton.Visible;
            startButton.Visible = !startButton.Visible;
        }
    }
}
