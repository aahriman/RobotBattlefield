using System;
using System.Drawing;

namespace ViewerLibrary {
    /// <summary>
    /// Describe what was in arena in that turn.
    /// </summary>
    public class Turn {
        /// <summary>
        /// Turn number.
        /// </summary>
        public int TURN { get; private set; }

        /// <summary>
        /// Bullets witch are presented on map in this turn.
        /// </summary>
        public Bullet[] BULLETS { get; private set; }
        /// <summary>
        /// Mines witch are presented on map in this turn.
        /// </summary>
        public Mine[] MINES { get; private set; }
        /// <summary>
        /// Repairs whit was done in this turn.
        /// </summary>
        public Repair[] REPAIRS { get; private set; }
        /// <summary>
        /// Robot witch are presented on map in this turn.
        /// </summary>
        public Robot[] ROBOTS { get; private set; }

        /// <summary>
        /// Scans witch ws done in  this turn.
        /// </summary>
        public Scan[] SCANS { get; private set; }

        /// <summary>
        /// Some more data like flags, flag places etc.
        /// </summary>
        public object[][] MORE { get; private set; }

        public Turn(int TURN, Bullet[] BULLETS, Mine[] MINES, Repair[] REPAIRS, Robot[] ROBOTS, Scan[] SCANS, object[][] MORE) {
            this.BULLETS = BULLETS ?? new Bullet[0];
            this.MINES = MINES ?? new Mine[0];
            this.REPAIRS = REPAIRS ?? new Repair[0];
            this.ROBOTS = ROBOTS ?? new Robot[0];
            this.SCANS = SCANS ?? new Scan[0];
            this.TURN = TURN;
            this.MORE = MORE;
        }
    }

    public class Bullet {
        /// <summary>
        /// X-coordinate of bullet.
        /// </summary>
        public double X { get; private set; }
        /// <summary>
        /// Y-coordinate of bullet.
        /// </summary>
        public double Y { get; private set; }

        /// <summary>
        /// Explode this bullet.
        /// </summary>
        public bool EXPLODED { get; private set; }

        public Bullet(double X, double Y, bool EXPLODED) {
            this.X = X;
            this.Y = Y;
            this.EXPLODED = EXPLODED;
        }
    }

    public class Mine {
        /// <summary>
        /// X-coordinate of bullet.
        /// </summary>
        public double X { get; private set; }
        /// <summary>
        /// Y-coordinate of bullet.
        /// </summary>
        public double Y { get; private set; }

        /// <summary>
        /// Explode this mine.
        /// </summary>
        public bool EXPLODED { get; private set; }

        public Mine(double X, double Y, bool EXPLODED) {
            this.X = X;
            this.Y = Y;
            this.EXPLODED = EXPLODED;
        }
    }
    
    public class Repair {

        /// <summary>
        /// X-coordinate
        /// </summary>
        public double X { get; private set; }
        /// <summary>
        /// Y-coordinate
        /// </summary>
        public double Y { get; private set; }

        public Repair(double X, double Y) {
            this.X = X;
            this.Y = Y;
        }
    }

    public class Robot {
        /// <summary>
        /// Team id for team in which robot belong to.
        /// </summary>
        public int TEAM_ID { get; private set; }
        /// <summary>
        /// Robots score.
        /// </summary>
        public int SCORE { get; private set; }
        /// <summary>
        /// Robots gold.
        /// </summary>
        public int GOLD { get; private set; }
        /// <summary>
        /// Robots hit points.
        /// </summary>
        public int HIT_POINTS { get; private set; }
        
        /// <summary>
        /// Robots name.
        /// </summary>
        public string NAME { get; private set; }

        /// <summary>
        /// Angle in degree where robot go. 0° - east, 90° - south, 180°- west and so on.
        /// </summary>
        public double ANGLE { get; private set; }
        /// <summary>
        /// X-coordinate
        /// </summary>
        public double X { get; private set; }
        /// <summary>
        /// Y-coordinate
        /// </summary>
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
        /// <summary>
        /// Angle in degree in witch direction was scanned. 0° - east, 90° - south, 180°- west and so on.
        /// </summary>
        public double ANGLE { get; private set; }
        /// <summary>
        /// Tolerance from ANGLE in degree. Scan is from ANGLE-PRECISION to ANGLE+PRECISION.
        /// </summary>
        public double PRECISION { get; private set; }

        /// <summary>
        /// How far was scanned.
        /// </summary>
        public double DISTANCE { get; private set; }

        /// <summary>
        /// X-coordinate where scan start.
        /// </summary>
        public double X { get; private set; }
        /// <summary>
        /// Y-coordinate where scan start.
        /// </summary>
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
}
