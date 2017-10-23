using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary.utils;
using ClientLibrary.robot;

namespace Rabbit {
    public class Program {

        private static readonly Random RANDOM = new Random();

        public static void Main(string[] args) {
            Tank tank = new Tank("Rabbit", Guid.NewGuid().ToString());

            while (true) {
                double toX = RANDOM.Next(0, 1000);
                double toY = RANDOM.Next(0, 1000);

                while (Math.Abs(toX - tank.X) > 50 && Math.Abs(toY - tank.Y) > 50) {
                    double angle = AngleUtils.ToDegree(AngleUtils.Angle(tank.X, tank.Y, toX, toY));
                    if (Math.Abs(angle - tank.AngleDrive) > 5) {
                        tank.Drive(angle, tank.Motor.ROTATE_IN);
                    } else {
                        tank.Wait();
                    }
                }
            }
        }
    }
}
