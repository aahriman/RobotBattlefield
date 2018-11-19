using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using BaseLibrary.utils;
using ViewerLibrary.model;

namespace ViewerLibrary.gui
{
    public class Render {

        protected const int MAX_HISTORICS_LAP = 4;

        public int Delay;

        protected bool animate;



        private readonly ITurnDataModel dataModel;
        private readonly IDrawer DRAWER;
        private readonly System.Windows.Forms.PictureBox PICTURE_BOX;

        private delegate void InvokeDelegate();

        private delegate void HistoryDelegate<in T>(T history, Graphics g, int lap);

        private readonly Bullet[][] HISTORIC_EXPLODED_BULLETS = new Bullet[MAX_HISTORICS_LAP][];
        private readonly Mine[][] HISTORIC_EXPLODED_MINES = new Mine[MAX_HISTORICS_LAP][];
        private readonly Repair[][] HISTORIC_REPAIRS = new Repair[MAX_HISTORICS_LAP][];
        private readonly Scan[][] HISTORIC_SCANS = new Scan[MAX_HISTORICS_LAP][];

        private readonly HistoryDelegate<Bullet> BULLET_DELEGATE;
        private readonly HistoryDelegate<Mine> MINE_DELEGATE;
        private readonly HistoryDelegate<Repair> REPAIR_DELEGATE;
        private readonly HistoryDelegate<Scan> SCAN_DELEGATE;

        public Render(ITurnDataModel dataModel, System.Windows.Forms.PictureBox pictureBox, IDrawer drawer) {
            this.PICTURE_BOX = pictureBox;
            this.DRAWER = drawer;

            registerMoreDrawer(drawer);

            this.dataModel = dataModel;
            pictureBox.Resize += (sender, args) => drawTurn();

            BULLET_DELEGATE = (bullet, g, turn) => DRAWER.DrawExplodedBullet(bullet, g, turn);
            MINE_DELEGATE = (mine, g, turn) => DRAWER.DrawExplodedMine(mine, g, turn);
            REPAIR_DELEGATE = (repair, g, turn) => DRAWER.DrawRepair(repair, g, turn);
            SCAN_DELEGATE = (scan, g, turn) => DRAWER.DrawScan(scan, g, turn);
        }

        public void Pause() {
            animate = false;
        }

        public void Play() {
            animate = true;

            Thread thread = new Thread(() => {
                while (animate && dataModel.HasNext()) {
                    actualTurn = dataModel.Next();
                    drawTurn();
                    Task.Delay(Delay).Wait();
                }
            });
            thread.Start();
        }

        public void StepNext() {
            animate = false;
            dataModel.HasNext();
            actualTurn = dataModel.Next();
            drawTurn();
        }

        protected Turn actualTurn;

        protected void drawTurn() {
            if (actualTurn == null) {
                return;
            }
            int width = PICTURE_BOX.Width;
            int height = PICTURE_BOX.Height;
            if (width == 0 || height == 0) return; 

            Bitmap drawingBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(drawingBitmap)) {
                g.ScaleTransform(drawingBitmap.Width / 1000f, drawingBitmap.Height / 1000f);

                HISTORIC_EXPLODED_BULLETS[0] = actualTurn.BULLETS.Where(bullet => bullet.EXPLODED).ToArray();
                HISTORIC_EXPLODED_MINES[0] = actualTurn.MINES.Where(mine => mine.EXPLODED).ToArray();
                HISTORIC_REPAIRS[0] = actualTurn.REPAIRS;
                HISTORIC_SCANS[0] = actualTurn.SCANS;

                DRAWER.DrawTurn(actualTurn, g);

                drawHistory(HISTORIC_EXPLODED_BULLETS, BULLET_DELEGATE, g);
                drawHistory(HISTORIC_EXPLODED_MINES, MINE_DELEGATE, g);
                drawHistory(HISTORIC_REPAIRS, REPAIR_DELEGATE, g);
                drawHistory(HISTORIC_SCANS, SCAN_DELEGATE, g);
            }
            if (PICTURE_BOX.Visible) {
                try {
                    PICTURE_BOX.BeginInvoke(new InvokeDelegate(() => {
                        PICTURE_BOX.Image = drawingBitmap;
                    }));
                } catch (InvalidOperationException) {
                    // someone close window form
                }
            }

            moveHistory(HISTORIC_EXPLODED_BULLETS);
            moveHistory(HISTORIC_EXPLODED_MINES);
            moveHistory(HISTORIC_REPAIRS);
            moveHistory(HISTORIC_SCANS);
        }



        private void drawHistory<T>(IReadOnlyList<T[]> history, HistoryDelegate<T> historyDelegate, Graphics g) {
            for (int turn = history.Count - 1; turn >= 0; turn--) {
                if (history[turn] != null) {
                    for (int i = 0; i < history[turn].Length; i++) {
                        historyDelegate.Invoke(history[turn][i], g, turn);
                    }
                }
            }
        }

        private void moveHistory<T>(IList<T[]> history) {
            for (int i = history.Count - 2; i >= 0; i--) {
                history[i + 1] = history[i];
            }
        }

        private void registerMoreDrawer(IDrawer drawer) {
            var type = typeof(IDrawerMore);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && p.IsClass);
            foreach (var t in types) {
                drawer.RegisterDrawerMore((IDrawerMore) Activator.CreateInstance(t));
            }
            
        }
    }

    public class ReversibleRender : Render {
        private readonly IReversibleTurnDataModel dataModel;

        public ReversibleRender(IReversibleTurnDataModel dataModel, System.Windows.Forms.PictureBox pictureBox, IDrawer drawer) : base(dataModel, pictureBox, drawer) {
            this.dataModel = dataModel;
        }

        public void StepPrevious()
        {
            animate = false;
            int i = -1;
            for (; i < MAX_HISTORICS_LAP && dataModel.HasPrevious(); i++) {
                dataModel.Previous();
            }
            while (i-- > 0) {
                actualTurn = dataModel.Next();
                drawTurn();
            }
        }
    }
}
