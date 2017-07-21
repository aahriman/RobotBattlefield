using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ObstacleMod;

namespace ObstacleMap {
    public partial class ObstacleMapForm : Form {
        // TODO zoom na kurzor
        // TODO Preview
        // TODO Opravit posun u mezí (vždy má být vidět co nejvíce)

        private const String CLEAR_OBTACLE = "<Clear>";

        public static readonly Size DEFAULT_IMAGE_SIZE = new Size(3000, 3000);
        private Image battlefieldImage;
        private Size battlefieldImageSize = DEFAULT_IMAGE_SIZE;
        private PointF battlefielLeftUpperCorner = new PointF();

        private IDictionary<Point, IObstacle> obtacles = new Dictionary<Point, IObstacle>();
        private Brush backgroundBrush = new SolidBrush(Color.White);

        public ObstacleMapForm() {
            InitializeComponent();
            battlefieldImage = new Bitmap(battlefield.Width, battlefield.Height);
            this.battlefield.MouseWheel += Battlefield_MouseWheel;
            this.battlefield.MouseMove += Battlefield_MouseMove;
            this.battlefield.MouseDown += Battlefield_MouseDown;
            this.battlefield.MouseUp += Battlefield_MouseUp;
            this.battlefield.Paint += (sender, args) => {
                                          drawBattlefieldImage();
                                          battlefield.Image = battlefieldImage;
                                      };
            this.battlefield.SizeChanged += (sender, args) => {
                if (battlefield.Visible && ! this.MinimizeBox) {
                    this.battlefieldImage = new Bitmap(this.battlefield.Size.Width, this.battlefield.Size.Height);
                    drawBattlefieldImage();
                }
            };
            fillListBox();
        }

        private void fillListBox() {
            listBox1.Items.Add(CLEAR_OBTACLE);

            IEnumerable<Type> obtacleTypes = Assembly.GetAssembly(typeof(IObstacle)).GetTypes()
                .Where(t =>
                    t.IsClass &&
                    typeof(IObstacle).IsAssignableFrom(t));

            foreach (var obtacleType in  obtacleTypes){
                this.listBox1.Items.Add(obtacleType.ToString());
            }
        }
        
        private void drawBattlefieldImage() {
            clearBattlefieldImage();
            drawBattlefieldImageGrid();
            drawBattlefieldObtacles();
        }

        private void clearBattlefieldImage() {
            Graphics.FromImage(battlefieldImage)
                .FillRectangle(backgroundBrush, new Rectangle(0, 0, battlefieldImage.Width, battlefieldImage.Height));
            getBattlefieldGraphics().DrawRectangle(new Pen(Color.Black), new Rectangle(0, 0, battlefieldImageSize.Width, battlefieldImageSize.Height));
        }

        private void drawBattlefieldObtacles() {
            Graphics graphics = getBattlefieldGraphics();
            
            foreach (var obtacle in obtacles.Values) {
                if (battlefielLeftUpperCorner.X <= (obtacle.X + 1) * xScale() &&
                    (obtacle.X - 1) * xScale() <= battlefielLeftUpperCorner.X + battlefield.Width &&
                    battlefielLeftUpperCorner.Y <= (obtacle.Y + 1) * yScale() &&
                    (obtacle.Y - 1) * yScale() <= battlefielLeftUpperCorner.Y + battlefield.Height) {
                    obtacle.Draw(graphics, xScale(), yScale());
                }
            }
        }

        private void drawBattlefieldImageGrid() {
            
            Pen blackPen = new Pen(Color.LightGray, 0.25F);
            Graphics graphics = getBattlefieldGraphics();

            if (battlefieldImageSize.Width >= 2000) {
                float xStep = xScale();
                for (float x = 0; x < battlefieldImageSize.Width; x += xStep) {
                    graphics.DrawLine(blackPen, x, 0, x, battlefieldImageSize.Height);
                }
            }

            if (battlefieldImageSize.Height >= 2000) {
                float yStep = yScale();
                for (float y = 0; y < battlefieldImageSize.Height; y += yStep) {
                    graphics.DrawLine(blackPen, 0, y, battlefieldImageSize.Width, y);
                }
            }
        }

        private Graphics getBattlefieldGraphics() {
            Graphics graphics = Graphics.FromImage(battlefieldImage);
            Matrix transformation = new Matrix();
            transformation.Translate(-battlefielLeftUpperCorner.X, -battlefielLeftUpperCorner.Y);
            graphics.Transform = transformation;
            return graphics;
        }

        private void Battlefield_MouseWheel(object sender, MouseEventArgs e) {
            if (e.Delta > 0) {
                increaseImageSize();
            } else if (e.Delta < 0) {
                decreaseImageSize();
            }
        }

        private PointF mousetAtDown = new PointF();
        private PointF prevBattleFieldLeftUpperConrner = new PointF();
        private void Battlefield_MouseMove(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle) {
                battlefielLeftUpperCorner = new PointF(Math.Max(0, e.X - mousetAtDown.X + prevBattleFieldLeftUpperConrner.X), Math.Max(0, e.Y - mousetAtDown.Y + prevBattleFieldLeftUpperConrner.Y));
            } else if (e.Button == MouseButtons.Left) {
                addObtacles(new Point((int)((e.X + battlefielLeftUpperCorner.X) / xScale()), (int)((e.Y + battlefielLeftUpperCorner.Y) / yScale())));
            }
        }

        private void Battlefield_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle) {
                Cursor.Current = Cursors.SizeAll;
                mousetAtDown = e.Location;
            } else if (e.Button == MouseButtons.Left) {
                addObtacles(new Point((int) ((e.X + battlefielLeftUpperCorner.X)/ xScale()), (int) ((e.Y + battlefielLeftUpperCorner.Y) / yScale())));
            }
        }

        private void Battlefield_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle) {
                prevBattleFieldLeftUpperConrner = battlefielLeftUpperCorner;
            }
        }

        private void addObtacles(Point point) {
            String obtacleName = listBox1?.SelectedItem?.ToString();
            if (obtacleName != null) {
                Point[] points;
                if (squareStyleRadioButton.Checked) {
                    points = genereteSquerPoints(point);
                } else if (crossStyleRadioButton.Checked) {
                    points = genereteCrossPoints(point);
                } else if (verticalLineStyleRadioButton.Checked) {
                    points = genereteVerticalLinePoints(point);
                } else if (horizontalLineStyleRadioButton.Checked) {
                    points = genereteHorizontalLinePoints(point);
                } else {
                    points = new Point[0];
                }

                foreach (var p in points) {
                    IObstacle obtacle = null;
                    if (obtacles.TryGetValue(p, out obtacle)) {
                        if (!obtacle.TypeName.Equals(obtacleName)) {
                            addObtacle(p, obtacleName);
                        }
                    } else {
                        addObtacle(p, obtacleName);
                    }
                }
            }
        }

        private void addObtacle(Point p, String obtacleName) {
            obtacles.Remove(p);
            if (!CLEAR_OBTACLE.Equals(listBox1.SelectedItem)) {
                IObstacle obtacle = ObstacleManager.GetObtacle(obtacleName, p.X, p.Y);

                obtacles.Add(p, obtacle);
            }
        }

        private void increaseImageSize() {
            Size prevImageSize = battlefieldImageSize;
            battlefieldImageSize = new Size(battlefieldImageSize.Width + 1000, battlefieldImageSize.Height + 1000);
            Cursor.Current = Cursors.Default;
            battlefielLeftUpperCorner = new PointF((battlefieldImageSize.Width * battlefielLeftUpperCorner.X) / prevImageSize.Width, (battlefieldImageSize.Height * battlefielLeftUpperCorner.Y) / prevImageSize.Height);
        }

        private void decreaseImageSize() {
            Size prevImageSize = battlefieldImageSize;
            battlefieldImageSize = new Size(battlefieldImageSize.Width - 1000, battlefieldImageSize.Height - 1000);
            if (battlefieldImageSize.Height < 1000 || battlefieldImageSize.Width < 1000) {
                Cursor.Current = Cursors.No;
                battlefieldImageSize = new Size(1000, 1000);

            }
            battlefielLeftUpperCorner = new PointF((battlefieldImageSize.Width * battlefielLeftUpperCorner.X) / prevImageSize.Width, (battlefieldImageSize.Height * battlefielLeftUpperCorner.Y) / prevImageSize.Height);
        }

        private float xScale() {
            return battlefieldImageSize.Width / 1000.0F;
        }

        private float yScale() {
            return battlefieldImageSize.Height / 1000.0F;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private Point[] genereteSquerPoints(Point point) {
            List<Point> ret = new List<Point>();

            for (int i = 0; i <= styleSize.Value; i++) {
                ret.AddRange(genereteVerticalLinePoints(new Point(point.X - i, point.Y)));
                if (i != 0) {
                    ret.AddRange(genereteVerticalLinePoints(new Point(point.X + i, point.Y)));
                }
            }
            return ret.ToArray();
        }

        private Point[] genereteCrossPoints(Point point) {
            List<Point> ret = new List<Point>();

            ret.AddRange(genereteHorizontalLinePoints(point));
            ret.Remove(point);
            ret.AddRange(genereteVerticalLinePoints(point));
            
            return ret.ToArray();
        }

        private Point[] genereteVerticalLinePoints(Point point) {
            List<Point> ret = new List<Point>();

            ret.Add(point);
            for (int i = 1; i <= styleSize.Value; i++) {
                ret.Add(new Point(point.X, point.Y - i));
                ret.Add(new Point(point.X, point.Y + i));
            }
            return ret.ToArray();
        }

        private Point[] genereteHorizontalLinePoints(Point point) {
            List<Point> ret = new List<Point>();

            ret.Add(point);
            for (int i = 1; i <= styleSize.Value; i++) {
                ret.Add(new Point(point.X - i, point.Y));
                ret.Add(new Point(point.X + i, point.Y));
            }
            return ret.ToArray();
        }

        private void saveButton_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (!"(loaded file)".Equals(loadedFileName.Text)) {
                saveFileDialog.FileName = loadedFileName.Text;
            }
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.Filter = "All files (*)|*";
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK) {
                ObstacleManager.SaveObtaclesToFile(obtacles.Values, saveFileDialog.FileName);
            }
        }

        private void loadButton_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (!"(loaded file)".Equals(loadedFileName.Text)) {
                openFileDialog.FileName = loadedFileName.Text;
            }
            
            DialogResult dialogResult = openFileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK) {
                obtacles.Clear();
                loadedFileName.Text = openFileDialog.FileName;
                if (dialogResult == DialogResult.OK) {
                    loadedFileName.Text = openFileDialog.FileName;
                    foreach (IObstacle obtacle in ObstacleManager.LoadObtaclesFromFile(openFileDialog.FileName)) {
                        Point key = new Point(obtacle.X, obtacle.Y);
                        obtacles.Add(key, obtacle);
                    }
                }
            }
        }
    }
}
