using System;
using System.Drawing;

namespace ViewerLibrary {
    public class Turn {
        public int TURN { get; private set; }
        public Bullet[] BULLETS { get; private set; }
        public Robot[] ROBOTS { get; private set; }
        public Scan[] SCANS { get; private set; }

        public Object[][] MORE { get; private set; }

        public Turn(int TURN, Bullet[] BULLETS, Robot[] ROBOTS, Scan[] SCANS, Object[][] MORE) {
            this.BULLETS = BULLETS ?? new Bullet[0];
            this.ROBOTS = ROBOTS ?? new Robot[0];
            this.SCANS = SCANS ?? new Scan[0];
            this.TURN = TURN;
            this.MORE = MORE;
        }
    }

    public class Bullet {
        public double X { get; private set; }
        public double Y { get; private set; }

        public bool EXPLODED { get; private set; }

        public Bullet(double X, double Y, bool EXPLODED) {
            this.X = X;
            this.Y = Y;
            this.EXPLODED = EXPLODED;
        }
    }

    public class Robot {
        public int TEAM_ID { get; private set; }
        public int SCORE { get; private set; }
        public int GOLD { get; private set; }
        public int HIT_POINTS { get; private set; }
        
        public string NAME { get; private set; }

        public double ANGLE { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }

        public Robot(int TEAM_ID, int SCORE, int GOLD, int HIT_POINTS, double X, double Y, double ANGLE, string NAME) {
            this.TEAM_ID = TEAM_ID;
            this.SCORE = SCORE;
            this.GOLD = GOLD;
            this.HIT_POINTS = HIT_POINTS;
            this.X = X;
            this.Y = Y;
            this.ANGLE = ANGLE;
            this.NAME = NAME;
        }

        public PointF GetPosition() {
            return new PointF((float) X, (float) Y);
        }
    }

    public class Scan {
        public double ANGLE { get; private set; }
        public double PRECISION { get; private set; }
        public double DISTANCE { get; private set; }

        public double X { get; private set; }
        public double Y { get; private set; }

        public Scan(double ANGLE, double PRECISION, double DISTANCE, double X, double Y) {
            this.ANGLE = ANGLE;
            this.PRECISION = PRECISION;
            this.DISTANCE = DISTANCE;
            this.X = X;
            this.Y = Y;
        }

        public PointF GetPosition() {
            return new PointF((float)X, (float)Y);
        }
    }

    public class Base {
        public double X { get; private set; }
        public double Y { get; private set; }
        public int PROGRESS { get; private set; }
        public int TEAM_ID { get; private set; }

        public Base(double X, double Y, int PROGRESS, int TEAM_ID) {
            this.X = X;
            this.Y = Y;
            this.PROGRESS = PROGRESS;
            this.TEAM_ID = TEAM_ID;
        }

        public PointF GetPosition() {
            return new PointF((float)X, (float)Y);
        }
    }

    public class Flag {
        public double X { get; private set; }
        public double Y { get; private set; }

        public Flag(double X, double Y) {
            this.X = X;
            this.Y = Y;
        }

        public PointF GetPosition() {
            return new PointF((float)X, (float)Y);
        }
    }
}
