using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ViewerLibrary.model;

namespace ViewerLibrary.gui
{
    public class Render {

        protected const int MAX_HISTORICS_LAP = 4;

        public int Delay;

        protected bool animate;



        private readonly ITurnDataModel DATA_MODEL;
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
            this.DATA_MODEL = dataModel;
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
                while (animate && DATA_MODEL.HasNext()) {
                    actualTurn = DATA_MODEL.Next();
                    drawTurn();
                    Task.Delay(Delay).Wait();
                }
            });
            thread.Start();
        }

        public void StepNext() {
            animate = false;
            actualTurn = DATA_MODEL.Next();
            drawTurn();
        }

        protected Turn actualTurn;

        protected void drawTurn() {
            if (actualTurn == null) {
                return;
            }
            Bitmap drawingBitmap = new Bitmap(PICTURE_BOX.Width, PICTURE_BOX.Height);
            Graphics g = Graphics.FromImage(drawingBitmap);
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
    }

    public class ReversibleRender : Render {
        private readonly IReversibleTurnDataModel DATA_MODEL;

        public ReversibleRender(IReversibleTurnDataModel dataModel, System.Windows.Forms.PictureBox pictureBox, IDrawer drawer) : base(dataModel, pictureBox, drawer) {
            this.DATA_MODEL = dataModel;
        }

        public void StepPrevious()
        {
            animate = false;
            int i = -1;
            for (; i < MAX_HISTORICS_LAP && DATA_MODEL.HasPrevious(); i++) {
                DATA_MODEL.Previous();
            }
            while (i-- > 0) {
                actualTurn = DATA_MODEL.Next();
                drawTurn();
            }
        }
    }
}
