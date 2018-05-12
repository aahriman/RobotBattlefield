using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary.communication.command.common;
using BaseLibrary.communication.command.handshake;
using ClientLibrary.robot;
using ClientLibrary.config;
using BaseLibrary.utils;


namespace MultiSpot {
    class Program {

        class Spot : Tank {
            public int angle = 90;
            public int direction;
            public ScanAnswerCommand scanAnswer;

            public Spot(string name) : base(name) { }
        }

        enum ToDo {
            DRIVE_100,
            DRIVE_0,
            SHOOT,
            SCAN,
        }

        static ToDo getWhatToDo(Spot spot) {
            if (spot.Power.Equals(0)) {
                return ToDo.DRIVE_100;
            } else if ((spot.direction == 90 && spot.Y > 575)
                       || (spot.direction == 270 && spot.Y < 425)
                       || (spot.direction == 180 && spot.X < 150)
                       || (spot.direction == 0 && spot.X > 850)) {
                spot.direction = (spot.direction + 90) % 360;
                return ToDo.DRIVE_0;
            } else if (spot.scanAnswer.ENEMY_ID != spot.ID) {
                return ToDo.SHOOT;
            } else return ToDo.SCAN;
        }

        static void process(Spot spot, ToDo toDo) {
            switch (toDo) {
                case ToDo.DRIVE_0:
                    spot.Drive(spot.direction, 0);
                    break;
                case ToDo.DRIVE_100:
                    spot.Drive(spot.direction, 100);
                    break;
                case ToDo.SHOOT:
                    spot.Shoot(spot.angle, spot.scanAnswer.RANGE);
                    break;
                case ToDo.SCAN:
                    spot.scanAnswer = spot.Scan(spot.angle, 10);
                    spot.angle = (spot.angle + 30) % 360;
                    break;
            }

        }

        static void Main(string[] args) {
            GameTypeCommand gameType = ClientRobot.Connect(args);
            Spot[] spots = new Spot[gameType.ROBOTS_IN_ONE_TEAM];
            for (int i = 0; i < spots.Length; i++) {
                spots[i] = new Spot("spot_" + i);
            }
            while (true) {
                foreach (Spot spot in spots) {
                    process(spot, getWhatToDo(spot));
                }
            }
        }
    }
}
