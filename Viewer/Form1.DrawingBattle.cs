using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Viewer.gui;
using ViewerLibrary;
using ViewerLibrary.serializers;

namespace Viewer {
    partial class Form1 {

        private const int FPS = 10;
        private const int MAX_HISTORICS_LAP = 4;
        private Drawer drawer = new DefaultDrawer();

        private Bullet[][] historictExplodedBullets = new Bullet[MAX_HISTORICS_LAP][];
        private Scan[][] historicScans = new Scan[MAX_HISTORICS_LAP][];

        private Bitmap drawingBitmap;
        private delegate void InvokeDelegate();
        private Thread draw() {
            Thread t = new Thread(() => {
                int index = 0;
                ASerializer serializer = ASerializer.getSerializer(reader.ReadLine());

                String turnLine = reader.ReadLine();
                while (this.stopButton.Visible && (turnLine = reader.ReadLine()) != null) {
                   
                    Turn turn = serializer.Deserialize(turnLine);

                    drawingBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    Graphics g = Graphics.FromImage(drawingBitmap);
                    g.ScaleTransform(drawingBitmap.Width/1000f, drawingBitmap.Height/1000f);
                    
                    historictExplodedBullets[0] = turn.BULLETS.Where(bullet => bullet.EXPLODED).ToArray();
                    historicScans[0] = turn.SCANS;

                    drawer.DrawTurn(turn, g);
                    for (int lap = Math.Min(MAX_HISTORICS_LAP - 1, historictExplodedBullets.Length); lap >= 0; lap--) {
                        if (historictExplodedBullets[lap] != null) {
                            foreach (var bullet in historictExplodedBullets[lap]) {
                                drawer.DrawExplodedBullet(bullet, g, lap);
                            }
                        }
                    }
                    for (int lap = Math.Min(MAX_HISTORICS_LAP - 1, historicScans.Length); lap >= 0; lap--) {
                        if (historicScans[lap] != null) {
                            foreach (var scans in historicScans[lap]) {
                                drawer.DrawScan(scans, g, lap);
                            }
                        }
                    }
                    pictureBox1.BeginInvoke(new InvokeDelegate(() => {
                        pictureBox1.Image = drawingBitmap;
                    }));
                    index = (index + 1);
                    Console.WriteLine(index);
                    for (int i = historictExplodedBullets.Length - 2; i >= 0; i--) {
                        historictExplodedBullets[i + 1] = historictExplodedBullets[i];
                    }

                    for (int i = historicScans.Length - 2; i >= 0; i--) {
                        historicScans[i + 1] = historicScans[i];
                    }
                    Task.WaitAll(Task.Delay(1000 / FPS));
                }
            });
            return t;
        }

        public void SetDrawFactory(Drawer drawer) {
            if (this.drawer == null) {
                throw new ArgumentNullException(nameof(drawer));
            }
            this.drawer = drawer;
        }
    }
}
