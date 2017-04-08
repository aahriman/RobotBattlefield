﻿using System;
using System.Collections.Generic;
using System.Drawing;
using BaseLibrary.utils;
using Viewer.utils;
using ViewerLibrary;

namespace Viewer.gui {
	public interface Drawer {
		void DrawTurn(Turn turn, Graphics g);
	    void DrawExplodedBullet(Bullet bullet, Graphics g, int lap);
	    void DrawScan(Scan scan, Graphics g, int lap);
	}


	internal class DefaultDrawer : Drawer {
		private const int BULLET_SIZE = 4;

		protected static readonly Color[] robotBodyColor = {
			Color.Black, Color.Green, Color.Brown, Color.BlueViolet, Color.Blue,
			Color.Gray, Color.Chartreuse, Color.Coral, Color.CornflowerBlue, Color.CadetBlue
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

		protected static readonly Pen[] ExplodedBulletPen = {
			new Pen(ColorUtils.HsvToColor(Color.Red.GetHue(), 1, 0.5, 200)),
			new Pen(ColorUtils.HsvToColor(Color.Red.GetHue(), 0.8, 0.8, 200)),
			new Pen(ColorUtils.HsvToColor(Color.Red.GetHue(), 0.65, 1, 200)),
			new Pen(ColorUtils.HsvToColor(Color.Red.GetHue(), 0.5, 1, 200)),
			new Pen(ColorUtils.HsvToColor(Color.Red.GetHue(), 0.4, 1, 200))
		};

		private readonly List<Pen> robotPen;
        private readonly Dictionary<string, int> robotId;


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

		}

		public DefaultDrawer() {
			robotPen = new List<Pen>();
            robotId = new Dictionary<string, int>();
        }

	    public void DrawTurn(Turn turn, Graphics g) {
	        foreach (var robot in turn.ROBOTS) {
	            drawRobot(robot, g);
	        }

	        foreach (var scan in turn.SCANS) {
                DrawScan(scan, g, 0);
	        }

            foreach (var bullet in turn.BULLETS) {
                drawBullet(bullet, g);
            }
        }

		private void drawRobot(Robot robot, Graphics g) {
			
		    int ID;
			if(! robotId.TryGetValue(robot.NAME, out ID)) {
			    ID = robotId.Count;
                robotId.Add(robot.NAME, ID);

                Color robotColor;
				if (robotBodyColor.Length < ID) {
					robotColor = robotBodyColor[ID];
				} else {
					Color c = robotBodyColor[ID % robotBodyColor.Length];
					robotColor = ColorUtils.HsvToColor(c.GetHue(), c.GetSaturation(), c.GetBrightness() / (1 + ID / robotBodyColor.Length));
				}
				
				robotPen.Add(new Pen(robotColor));
			}
            Pen pen = robotPen[ID];
            drawRobotBody(robot, pen, g);

			Font drawFont = new Font("Arial", 16);
			String drawString = String.Format("{0} ({1})", robot.NAME, robot.HIT_POINTS);
			g.DrawString(drawString, drawFont, pen.Brush, DrawerUtils.Translate(robot.GetPosition(), name));
		}

		public void drawBullet(Bullet info, Graphics g) {
			g.FillEllipse(bulletPen.Brush, (float)(info.X - BULLET_SIZE/2.0), (float)(info.Y - BULLET_SIZE / 2.0), BULLET_SIZE, BULLET_SIZE);
		}

		public void DrawExplodedBullet(Bullet info, Graphics g, int lap) {
			int index = lap;
			int size = BULLET_SIZE*(1 + index);
			g.FillEllipse(ExplodedBulletPen[index].Brush, (float)(info.X - size / 2.0), (float)(info.Y - size / 2.0), size, size);
		}

		private void drawRobotBody(Robot robot, Pen pen, Graphics g) {
			pen.Width = 5;
			PointF[] spike = DrawerUtils.Transform(AngleUtils.ToRads(robot.ANGLE), PointF.Empty, DefaultDrawer.spike);
			PointF[] botton = DrawerUtils.Transform(AngleUtils.ToRads(robot.ANGLE), PointF.Empty, DefaultDrawer.bottom);
		    PointF robotPosition = robot.GetPosition();
			g.DrawLines(pen, DrawerUtils.Translate(robotPosition, spike));
			g.DrawLines(pen, DrawerUtils.Translate(robotPosition, botton));
		}

		public void DrawScan(Scan scan, Graphics g, int lap) {
            int index = lap;
            Pen pen = scannerPen[index];
			pen.Width = 2;
		    double startAngle = scan.ANGLE - scan.PRECISION;
            double stopAngle = scan.ANGLE + scan.PRECISION;
		    if (lap == 0) {
		        PointF scanPosition = scan.GetPosition();

                g.DrawLine(pen, scanPosition,
		            DrawerUtils.Translate(scanPosition,
		                DrawerUtils.Transform(AngleUtils.ToRads(startAngle), PointF.Empty,
		                    new PointF((float) scan.DISTANCE, 0F))));
		        g.DrawLine(pen, scanPosition,
		            DrawerUtils.Translate(scanPosition,
		                DrawerUtils.Transform(AngleUtils.ToRads(stopAngle), PointF.Empty,
		                    new PointF((float) scan.DISTANCE, 0F))));
		    }
		    RectangleF circle = new RectangleF(
                (float) (scan.X - scan.DISTANCE),
                (float) (scan.Y - scan.DISTANCE),
                (float) scan.DISTANCE*2,
                (float) scan.DISTANCE*2
                );
		    if (scan.PRECISION > 0 && scan.DISTANCE > 0) {
		        g.DrawArc(pen, circle, (float) startAngle, (float) scan.PRECISION * 2);
		    }
		}
	}
}
