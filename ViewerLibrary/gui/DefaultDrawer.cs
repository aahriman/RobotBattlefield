using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using ViewerLibrary.utils;
using BaseLibrary.utils;

namespace ViewerLibrary.gui {
    public class DefaultDrawer : IDrawer {
        private const int BULLET_SIZE = 4;
        private const int REPAIR_SIZE = 4;
        private const int MINE_SIZE = 4;

        private static readonly Color[] teamColor = {
            ColorUtils.ColorWithAlpha(Color.Black, 200),
            ColorUtils.ColorWithAlpha(Color.Blue, 200),
            ColorUtils.ColorWithAlpha(Color.Magenta, 200),
            ColorUtils.ColorWithAlpha(Color.Green, 200),
            ColorUtils.ColorWithAlpha(Color.Red, 200),
        };

        private static readonly Pen[] teamPen;

        protected static readonly Color[] robotBodyColor = {
            Color.Gray, Color.Black, Color.Green, Color.Brown, Color.Blue,
             Color.Chartreuse, Color.Coral, Color.CornflowerBlue, Color.CadetBlue
        };

        protected static readonly PointF[] spike;
        protected static readonly PointF[] bottom;
        protected static readonly PointF name;
        protected static readonly PointF maxArenaSize = new PointF(1000,1000);

        protected static readonly Pen[] scannerPen = {
            new Pen(ColorUtils.HsvToColor(Color.DarkGreen.GetHue(), 1, 0.4, 200)),
            new Pen(ColorUtils.HsvToColor(Color.DarkGreen.GetHue(), 1, 0.65, 200)),
            new Pen(ColorUtils.HsvToColor(Color.DarkGreen.GetHue(), 1, 0.8, 200)),
            new Pen(ColorUtils.HsvToColor(Color.DarkGreen.GetHue(), 1, 1, 200))
        };
        protected static readonly Pen bulletPen = new Pen(Color.Black);
        protected static readonly Pen minePen = new Pen(Color.Black);
        protected static readonly Pen mineInnerPen = new Pen(Color.Red);

        protected static readonly Pen[] ExplodedBulletPen = {
            new Pen(ColorUtils.HsvToColor(Color.Red.GetHue(), 1, 0.5, 200)),
            new Pen(ColorUtils.HsvToColor(Color.Red.GetHue(), 0.8, 0.8, 200)),
            new Pen(ColorUtils.HsvToColor(Color.Red.GetHue(), 0.65, 1, 200)),
            new Pen(ColorUtils.HsvToColor(Color.Red.GetHue(), 0.5, 1, 200)),
            new Pen(ColorUtils.HsvToColor(Color.Red.GetHue(), 0.4, 1, 200))
        };

        protected static readonly Pen[] ExplodedMinePen = {
            new Pen(ColorUtils.HsvToColor(Color.Orange.GetHue(), 1, 0.5, 200)),
            new Pen(ColorUtils.HsvToColor(Color.Orange.GetHue(), 0.8, 0.8, 200)),
            new Pen(ColorUtils.HsvToColor(Color.Orange.GetHue(), 0.65, 1, 200)),
            new Pen(ColorUtils.HsvToColor(Color.Orange.GetHue(), 0.5, 1, 200)),
            new Pen(ColorUtils.HsvToColor(Color.Orange.GetHue(), 0.4, 1, 200))
        };

        protected static readonly Pen[] RepairPen = {
            new Pen(ColorUtils.HsvToColor(Color.Blue.GetHue(), 1, 0.5, 200)),
            new Pen(ColorUtils.HsvToColor(Color.Blue.GetHue(), 0.8, 0.8, 200)),
            new Pen(ColorUtils.HsvToColor(Color.Blue.GetHue(), 0.65, 1, 200)),
            new Pen(ColorUtils.HsvToColor(Color.Blue.GetHue(), 0.5, 1, 200)),
            new Pen(ColorUtils.HsvToColor(Color.Blue.GetHue(), 0.4, 1, 200))
        };

        private readonly Dictionary<int, Pen> robotPen = new Dictionary<int, Pen>();


        static DefaultDrawer() {
            spike = new PointF[] {
                new PointF(0, -15),
                new PointF(15, 0),
                new PointF(0, 15)
            };
            bottom = new PointF[] {
                spike[0],
                new PointF(-5, -15),
                new PointF(-7, -13),
                new PointF(-15, -10),
                new PointF(-15, 10),
                new PointF(-7, 13),
                new PointF(-5, 15),
                spike[spike.Length-1]
            };
            name = new PointF(-15, 15);

            teamPen = new Pen[teamColor.Length];
            for (int i = 0; i < teamColor.Length; i++) {
                teamPen[i] = new Pen(teamColor[i]);
            }

        }

        List<IDrawerMore> drawersMore = new List<IDrawerMore>();

        /// <summary>
        /// Return pen for team, or crete new one for that team and return.
        /// </summary>
        /// <param name="teamId">For what team id wants to get pen.</param>
        /// <returns></returns>
        public static Pen GetTeamPen(int teamId) {
            if (teamPen.Length > teamId) {
                return teamPen[teamId];
            } else {
                Color c = teamColor[teamId % teamPen.Length];
                return new Pen(ColorUtils.HsvToColor(c.GetHue(), c.GetSaturation(), c.GetBrightness() / (1 + teamId / teamColor.Length)));
            }
        }

        public DefaultDrawer() {
        }

        /// <inheritdoc />
        public void DrawTurn(Turn turn, Graphics g) {
            drawRobotsStats(turn.ROBOTS, g);

            foreach (var robot in turn.ROBOTS) {
                DrawRobot(robot, g);
            }

            foreach (var scan in turn.SCANS) {
                DrawScan(scan, g, 0);
            }

            foreach (var bullet in turn.BULLETS) {
                DrawBullet(bullet, g);
            }

            foreach (var mine in turn.MINES) {
                DrawMine(mine, g);
            }

            foreach (var drawer in drawersMore) {
                foreach (var singleMore in turn.MORE) {
                    drawer.DrawMore(singleMore, g);
                }
            }
        }

        /// <summary>
        /// Return pen for robot.
        /// </summary>
        /// <param name="robot"></param>
        /// <returns></returns>
        public Pen GetRobotPen(Robot robot) {
            if (robotPen.TryGetValue(robot.ID, out Pen pen)) return pen;

            int id = robot.ID;

            Color robotColor;
            if (robotBodyColor.Length < id) {
                robotColor = robotBodyColor[id];
            } else {
                Color c = robotBodyColor[id % robotBodyColor.Length];
                robotColor = ColorUtils.HsvToColor(c.GetHue(), c.GetSaturation(), c.GetBrightness() / (1 + id / robotBodyColor.Length));
            }

            robotPen.Add(id, new Pen(robotColor));
            return robotPen[id];
        }

        /// <summary>
        /// Draw robot on graphics context. When robot HP is 0, then it will not be drawn.
        /// </summary>
        /// <param name="robot">robot with should be drawn</param>
        /// <param name="g">graphics context</param>
        public void DrawRobot(Robot robot, Graphics g) {
            if (robot.HIT_POINTS <= 0) return;
            Pen pen = GetRobotPen(robot);


            drawRobotBody(robot, pen, g);

            Font drawFont = new Font("Arial", 10);
            string drawString = $"{robot.NAME}:{robot.ID} ({robot.HIT_POINTS})";
            lock (g) {
                lock (pen) {
                    g.DrawString(drawString, drawFont, pen.Brush, DrawerUtils.Translate(robot.GetPosition(), name));
                }
            }
        }

        /// <summary>
        /// Draw bullet.
        /// </summary>
        /// <param name="info">bullet with should be drawn</param>
        /// <param name="g">graphics context</param>
        public void DrawBullet(Bullet info, Graphics g) {
            lock (g) {
                lock (bulletPen) {
                    g.FillEllipse(bulletPen.Brush, (float) (info.X - BULLET_SIZE / 2.0), (float) (info.Y - BULLET_SIZE / 2.0), BULLET_SIZE, BULLET_SIZE);
                }
            }
        }

        /// <inheritdoc />
        public void DrawExplodedBullet(Bullet info, Graphics g, int turn) {
            int index = turn;
            int size = BULLET_SIZE*(1 + index);
            lock (g) {
                lock (ExplodedBulletPen[index]) {
                    g.FillEllipse(ExplodedBulletPen[index].Brush, (float)(info.X - size / 2.0), (float)(info.Y - size / 2.0), size, size);
                }
            }
        }

        /// <summary>
        /// Draw placed mine.
        /// </summary>
        /// <param name="info">mine with should be drawn</param>
        /// <param name="g">graphics context</param>
        public void DrawMine(Mine info, Graphics g) {
            lock (g) {
                lock (minePen) {
                    g.FillEllipse(minePen.Brush, (float) (info.X - MINE_SIZE / 2.0), (float) (info.Y - MINE_SIZE / 2.0), MINE_SIZE, MINE_SIZE);
                }
                lock (mineInnerPen) {
                    g.FillEllipse(mineInnerPen.Brush, (float)(info.X - MINE_SIZE / 2.0), (float)(info.Y - MINE_SIZE / 2.0), 1, 1);
                }
            }         
        }

        /// <inheritdoc />
        public void DrawExplodedMine(Mine info, Graphics g, int turn) {
            int index = turn;
            int size = MINE_SIZE*(1 + index);
            lock (g) {
                lock (ExplodedMinePen[index]) {
                    g.FillEllipse(ExplodedMinePen[index].Brush, (float)(info.X - size / 2.0), (float)(info.Y - size / 2.0), size, size);
                }
            }            
        }

        /// <inheritdoc />
        public void DrawRepair(Repair info, Graphics g, int turn) {
            int index = turn;
            int size = REPAIR_SIZE*(1 + index);
            lock (g) {
                lock (RepairPen[index]) {
                    g.FillEllipse(RepairPen[index].Brush, (float)(info.X - size / 2.0), (float)(info.Y - size / 2.0), size, size);
                }
            }
            
        }

        private void drawRobotBody(Robot robot, Pen pen, Graphics g) {
            PointF[] spike = DrawerUtils.Rotate(AngleUtils.ToRads(robot.ANGLE), PointF.Empty, DefaultDrawer.spike);
            PointF[] bottom = DrawerUtils.Rotate(AngleUtils.ToRads(robot.ANGLE), PointF.Empty, DefaultDrawer.bottom);
            PointF robotPosition = robot.GetPosition();
            Pen teamPen = DefaultDrawer.GetTeamPen(robot.TEAM_ID);
            
            
            lock (g) {
                lock (teamPen) {
                    float oldTeamPenWidth = teamPen.Width;
                    teamPen.Width = 5;
                    g.DrawLines(teamPen, DrawerUtils.Translate(robotPosition, spike));
                    teamPen.Width = oldTeamPenWidth;
                }

                lock (pen) {
                    pen.Width = 5;
                    g.DrawLines(pen, DrawerUtils.Translate(robotPosition, bottom));
                }
            }
            
        }

        private class RobotComparer : IComparer, IComparer<Robot> {
            /// <inheritdoc />
            public int Compare(Robot x, Robot y) {
                if (x == null || y == null) {
                    throw new InvalidOperationException("RobotComparer cannot compare null references.");
                } 
                return y.SCORE - x.SCORE;
            }

            /// <inheritdoc />
            public int Compare(object x, object y) {
                if (x is Robot && y is Robot) {
                    return Compare((Robot) x, (Robot) y);
                } else {
                    throw new InvalidOperationException("RobotComparer cannot compare other object then robot.");
                }
            }
        }

        private void drawRobotsStats(Robot[] robots, Graphics g) {
            Array.Sort(robots, null, new RobotComparer());
            Font drawFont = new Font("Arial", 10);
            float spaceToScore = 0;
            float indentText = 10;
            lock (g) {
                lock (Brushes.Black) {
                    g.DrawString("Score:", drawFont, Brushes.Black, new PointF(indentText, 0));
                }
            }
            for (int i = 0; i < robots.Length; i++) {
                Robot robot = robots[i];
                Pen robotPen = GetRobotPen(robot);
                string name = $"{robot.NAME}:{robot.ID} - T{robot.TEAM_ID}";
                PointF drawFrom = new PointF(indentText, drawFont.Height * (i + 1));
                lock (g) {
                    lock (robotPen) {
                        g.DrawString(name, drawFont, robotPen.Brush, drawFrom);
                    }
                }
                spaceToScore = Math.Max(spaceToScore, g.MeasureString(name, drawFont).Width);
            }

            for (int i = 0; i < robots.Length; i++) {
                Robot robot = robots[i];
                Pen robotPen = GetRobotPen(robot);
                string score = $"{robot.SCORE}";
                PointF drawFrom = new PointF(indentText + spaceToScore + indentText, drawFont.Height * (i + 1));
                lock (g) {
                    lock (robotPen) {
                        g.DrawString(score, drawFont, robotPen.Brush, drawFrom);
                    }
                }
            }
        }

        /// <inheritdoc />
        public void DrawScan(Scan scan, Graphics g, int turn) {
            int index = turn;
            Pen pen = scannerPen[index];
            double startAngle = scan.ANGLE - scan.PRECISION + 360;
            double stopAngle = scan.ANGLE + scan.PRECISION + 360;
            if (turn == 0) {
                PointF scanPosition = scan.GetPosition();
                PointF endScan1 = DrawerUtils.Translate(scanPosition,
                    DrawerUtils.Rotate(AngleUtils.ToRads(startAngle), PointF.Empty,
                        new PointF((float)scan.DISTANCE, 0F)));
                PointF endScan2 = DrawerUtils.Translate(scanPosition,
                    DrawerUtils.Rotate(AngleUtils.ToRads(stopAngle), PointF.Empty,
                        new PointF((float)scan.DISTANCE, 0F)));
                lock (g) {
                    lock (pen) {
                        pen.Width = 2;
                        g.DrawLine(pen, scanPosition, endScan1);
                        g.DrawLine(pen, scanPosition, endScan2);
                    }
                }

            }
            RectangleF circle = new RectangleF(
                (float) (scan.X - scan.DISTANCE),
                (float) (scan.Y - scan.DISTANCE),
                (float) scan.DISTANCE*2,
                (float) scan.DISTANCE*2
                );
            if (scan.PRECISION > 0 && scan.DISTANCE > 0) {
                lock (g) {
                    lock (pen) {
                        g.DrawArc(pen, circle, (float) startAngle, (float) scan.PRECISION * 2);
                    }
                }
            }
        }

        /// <inheritdoc />
        public void RegisterDrawerMore(IDrawerMore drawer) {
            drawersMore.Add(drawer);
        }
    }
}
