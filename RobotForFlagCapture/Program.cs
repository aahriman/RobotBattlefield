using System;
using System.Threading.Tasks;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using BaseLibrary.utils;
using ClientLibrary.robot;
using FlagCaptureLibrary.battlefield;

namespace RobotForFlagCapture {
    class Program {

        class MyTank : Tank {
            public bool carryFlag = false;
            public double WantedPower;

            public override void ProcessState(RobotStateCommand state) {
                base.ProcessState(state);
                Flag[] flags = FlagCapture.GetFlags(state);

                carryFlag = false;
                foreach (var flag in flags) {
                    if (flag.RobotId == this.ID) {
                        carryFlag = true;
                    }
                }   
            }
        }

        static MyTank tank = new MyTank();

        static void driveTo(FlagPlace place) {
            double driveAngle = AngleUtils.AngleDegree(tank.X, tank.Y, place.X, place.Y);


            if (tank.HitPoints == 0) {
                tank.Wait(); // wait to respawn
            }

            if (Math.Abs(driveAngle - tank.AngleDrive) > 5) {
                if (tank.Power > tank.Motor.ROTATE_IN && tank.WantedPower > tank.Motor.ROTATE_IN) {
                    tank.Drive(driveAngle, tank.Motor.ROTATE_IN);
                    tank.WantedPower = tank.Motor.ROTATE_IN;
                } else if(tank.Power <= tank.Motor.ROTATE_IN) {
                    tank.Drive(driveAngle, 100);
                    tank.WantedPower = 100;
                }
            } 
            for (int angle = 0; angle < 360; angle += 30) {
                ScanAnswerCommand scanAnswer = tank.Scan(angle, 10);
                if (scanAnswer.ENEMY_ID != tank.ID) {
                    tank.Shot(angle, scanAnswer.RANGE);
                }
            }
        }

        static void Main(string[] args) {
            tank.Connect(args);
            InitAnswerCommand initAnswerCommand = tank.Init("Tank_FLAG", Guid.NewGuid().ToString());
            FlagPlace[] flagPlaces = FlagCapture.GetFlagPlaces(initAnswerCommand);
            FlagPlace toFlagPlace = null;
            FlagPlace ownFlagPlace = null;

            foreach (var flagPlace in flagPlaces) {
                if (flagPlace.TEAM_ID == tank.TEAM_ID) {
                    ownFlagPlace = flagPlace;
                } else {
                    toFlagPlace = flagPlace;
                }
            }


            while (true) {
                if (tank.carryFlag) {
                    driveTo(ownFlagPlace);
                } else {
                    driveTo(toFlagPlace);
                }
            }
        }
    }
}
