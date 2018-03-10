using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClientLibrary.robot;
using ClientLibrary.config;
using BaseLibrary.command.common;
using BaseLibrary.utils;
using BaseLibrary.equip;



namespace MultiSpot {
    class Program {

        class Spot : Tank {
            public int angle = 90;
            public int direction;
            public ScanAnswerCommand scanAnswer;

            public Spot(string name, string teamName) : base(name, teamName) { }
        }

        enum Udalost {
            DRIVE_100,
            DRIVE_0,
            SHOOT,
            SCAN,
        }

        static Udalost vratUdalost(Spot spot) {
            if (spot.Power.Equals(0)) {
                return Udalost.DRIVE_100;
            } else if ((spot.direction == 90 && spot.Y > 575)
                       || (spot.direction == 270 && spot.Y < 425)
                       || (spot.direction == 180 && spot.X < 150)
                       || (spot.direction == 0 && spot.X > 850)) {
                spot.direction = (spot.direction + 90) % 360;
                return Udalost.DRIVE_0;
            } else if (spot.scanAnswer.ENEMY_ID != spot.ID) {
                return Udalost.SHOOT;
            } else return Udalost.SCAN;
        }

        static void process(Spot spot, Udalost udalost) {
            switch (udalost) {
                case Udalost.DRIVE_0:
                    spot.Drive(spot.direction, 0);
                    break;
                case Udalost.DRIVE_100:
                    spot.Drive(spot.direction, 100);
                    break;
                case Udalost.SHOOT:
                    spot.Shoot(spot.angle, spot.scanAnswer.RANGE);
                    break;
                case Udalost.SCAN:
                    spot.scanAnswer = spot.Scan(spot.angle, 10);
                    spot.angle = (spot.angle + 30) % 360;
                    break;
            }

        }

        static void Main(string[] args) {
            ClientRobot.Connect(args);
            Spot[] spots = new Spot[3];
            for (int i = 0; i < spots.Length; i++) {
                spots[i] = new Spot("spot_" + i, ClientRobot.TEAM_NAME);
            }
            while (true) {
                foreach (Spot spot in spots) {
                    process(spot, vratUdalost(spot));
                }
            }
        }
    }
}
