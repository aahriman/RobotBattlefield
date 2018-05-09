using System;
using System.Threading.Tasks;
using BaseLibrary.communication.command.common;
using BaseLibrary.communication.command.handshake;
using BaseLibrary.utils;
using ClientLibrary.robot;
using FlagCaptureLibrary.battlefield;

namespace RobotForFlagCapture {
    class Program {
        private static FlagPlace[] flagPlaces;

        class MyTank : Tank {
            public bool carryFlag = false;
            public double WantedPower;

            public MyTank(String name, String teamName) : base(name, teamName) {}

            protected override void ProcessState(RobotStateCommand state) {
                base.ProcessState(state);
                Flag[] flags = FlagCapture.GetFlags(state);

                carryFlag = false;
                foreach (var flag in flags) {
                    if (flag.RobotId == this.ID) {
                        carryFlag = true;
                    }
                }   
            }

            protected override void ProcessInit(InitAnswerCommand initAnswerCommand) {
                flagPlaces = FlagCapture.GetFlagPlaces(initAnswerCommand);
            }
        }

        static MyTank tank = new MyTank("Tank_FLAG", Guid.NewGuid().ToString());

        private static void driveTo(FlagPlace place) {
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
                    tank.Shoot(angle, scanAnswer.RANGE);
                }
            }
        }

        static void Main(string[] args) {
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
