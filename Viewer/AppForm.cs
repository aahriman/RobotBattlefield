using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Viewer {
    public partial class AppForm : Form {
        private StreamReader reader;
        private Thread drawingThread;

        public AppForm() {
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
            drawingThread = draw();
            drawingThread.Start();

        }

        private void stop_Click(object sender, EventArgs e) {
            alternateStartStopVisibility();
            drawingThread.Join();
            reader.Close();
        }

        private void alternateStartStopVisibility() {
            stopButton.Visible = !stopButton.Visible;
            startButton.Visible = !startButton.Visible;
        }
    }
}
