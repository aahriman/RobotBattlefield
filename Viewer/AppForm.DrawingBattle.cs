using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using BaseLibrary;
using BaseLibrary.utils;
using Viewer.gui;
using ViewerLibrary;
using ViewerLibrary.serializers;

namespace Viewer {
    partial class AppForm {

        private const int FPS = 40;
        private const int MAX_HISTORICS_LAP = 4;
        private IDrawer drawer;

        private Bullet[][] historictExplodedBullets = new Bullet[MAX_HISTORICS_LAP][];
        private Mine[][] historictExplodedMines = new Mine[MAX_HISTORICS_LAP][];
        private Repair[][] historictRepairs = new Repair[MAX_HISTORICS_LAP][];
        private Scan[][] historicScans = new Scan[MAX_HISTORICS_LAP][];

        private Bitmap drawingBitmap;
        private delegate void InvokeDelegate();
        private Thread draw() {
            if (drawer == null) {
                SetDrawFactory(new DefaultDrawer());
            }
            Thread t = new Thread(() => {
                int index = 0;
                JSONSerializer serializer = new JSONSerializer();

                String turnLine = reader.ReadLine();
                while (this.stopButton.Visible && (turnLine = reader.ReadLine()) != null) {

                    Turn turn = serializer.Deserialize(turnLine);
                    if (turn == null) {
                        continue;
                    }
                    drawingBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    Graphics g = Graphics.FromImage(drawingBitmap);
                    g.ScaleTransform(drawingBitmap.Width/1000f, drawingBitmap.Height/1000f);

                    historictExplodedBullets[0] = turn.BULLETS.Where(bullet => bullet.EXPLODED).ToArray();
                    historictExplodedMines[0] = turn.MINES.Where(mine => mine.EXPLODED).ToArray();
                    historictRepairs[0] = turn.REPAIRS;
                    historicScans[0] = turn.SCANS;

                    drawer.DrawTurn(turn, g);
                    for (int lap = Math.Min(MAX_HISTORICS_LAP, historictExplodedBullets.Length) - 1; lap >= 0; lap--) {
                        if (historictExplodedBullets[lap] != null) {
                            foreach (var bullet in historictExplodedBullets[lap]) {
                                drawer.DrawExplodedBullet(bullet, g, lap);
                            }
                        }
                    }

                    for (int lap = Math.Min(MAX_HISTORICS_LAP, historictExplodedMines.Length) - 1; lap >= 0; lap--) {
                        if (historictExplodedMines[lap] != null) {
                            foreach (var bullet in historictExplodedMines[lap]) {
                                drawer.DrawExplodedMine(bullet, g, lap);
                            }
                        }
                    }

                    for (int lap = Math.Min(MAX_HISTORICS_LAP, historictRepairs.Length) - 1; lap >= 0; lap--) {
                        if (historictRepairs[lap] != null) {
                            foreach (var bullet in historictRepairs[lap]) {
                                drawer.DrawRepair(bullet, g, lap);
                            }
                        }
                    }

                    for (int lap = Math.Min(MAX_HISTORICS_LAP, historicScans.Length) - 1; lap >= 0; lap--) {
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
                    for (int i = historictExplodedBullets.Length - 2; i >= 0; i--) {
                        historictExplodedBullets[i + 1] = historictExplodedBullets[i];
                    }

                    for (int i = historictExplodedMines.Length - 2; i >= 0; i--) {
                        historictExplodedMines[i + 1] = historictExplodedMines[i];
                    }

                    for (int i = historictRepairs.Length - 2; i >= 0; i--) {
                        historictRepairs[i + 1] = historictRepairs[i];
                    }

                    for (int i = historicScans.Length - 2; i >= 0; i--) {
                        historicScans[i + 1] = historicScans[i];
                    }
                    Task.WaitAll(Task.Delay(1000 / FPS));
                }
            });
            return t;
        }

        public void SetDrawFactory(IDrawer drawer) {
            if (drawer == null) {
                throw new ArgumentNullException(nameof(drawer));
            }
            this.drawer = drawer;
            registerDrawerMore();
        }

        private void registerDrawerMore() {

            List<Assembly> assemblies = new List<Assembly>();
            assemblies.Add(Assembly.GetCallingAssembly());
            assemblies.AddRange(ModUtils.LoadAllAssemblyInDir("."));
            foreach (var assembly in assemblies) {
                Type[] modTypes = (Type[]) assembly
                .GetTypes()
                .Where(t =>
                           t.IsClass &&
                           (typeof(IDrawerMore).IsAssignableFrom(t)))
                .ToArray();
                foreach (Type drawerModeType in modTypes) {
                    IDrawerMore drawerMore = (IDrawerMore) Activator.CreateInstance(drawerModeType);
                    drawer.RegisterDrawerMore(drawerMore);
                }
            }
        }
    }
}
