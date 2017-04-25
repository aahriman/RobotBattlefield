using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary.command;
using BaseLibrary.command.common;
using ClientLibrary.robot;

namespace Spot {
	class Program {
	    public static String TEAM_NAME = Guid.NewGuid().ToString();
        public static void Main(string[] args) {
            Console.WriteLine("Spot is ready to action.");
            Console.WriteLine(TEAM_NAME + " " + TEAM_NAME.Length);
            Tank spot = new Tank();
		    spot.Connect();
		    String name = "Spot___1";
		    spot.ProcessInit(spot.Init(name, TEAM_NAME));

            int direction = 90;
            while (true) {
                if (spot.Power.Equals(0)) {
                    spot.Drive(100, direction);
                }
                if ((direction == 90 && spot.Y > 575)
                    || (direction == 270 && spot.Y < 425)
                    || (direction == 180 && spot.X < 150)
                    || (direction == 0 && spot.X > 850)) {
                        spot.Drive(0, direction);
                        direction = (direction + 90) % 360;
                } else {
                    ScanAnswerCommand scanAnswer;
                    if ((scanAnswer = spot.Scan(10, 0)).ENEMY_ID != spot.ID) spot.Shot(scanAnswer.RANGE, 0);
                    else if ((scanAnswer = spot.Scan(10, 30)).ENEMY_ID != spot.ID) spot.Shot(scanAnswer.RANGE, 30);
                    else if ((scanAnswer = spot.Scan(10, 60)).ENEMY_ID != spot.ID) spot.Shot(scanAnswer.RANGE, 60);
                    else if ((scanAnswer = spot.Scan(10 ,90)).ENEMY_ID != spot.ID) spot.Shot(scanAnswer.RANGE, 90);
                    else if ((scanAnswer = spot.Scan(10, 120)).ENEMY_ID != spot.ID) spot.Shot(scanAnswer.RANGE, 120);
                    else if ((scanAnswer = spot.Scan(10, 150)).ENEMY_ID != spot.ID) spot.Shot(scanAnswer.RANGE, 150);
                    else if ((scanAnswer = spot.Scan(10, 180)).ENEMY_ID != spot.ID) spot.Shot(scanAnswer.RANGE, 180);
                    else if ((scanAnswer = spot.Scan(10, 210)).ENEMY_ID != spot.ID) spot.Shot(scanAnswer.RANGE, 210);
                    else if ((scanAnswer = spot.Scan(10, 240)).ENEMY_ID != spot.ID) spot.Shot(scanAnswer.RANGE, 240);
                    else if ((scanAnswer = spot.Scan(10, 270)).ENEMY_ID != spot.ID) spot.Shot(scanAnswer.RANGE, 270);
                    else if ((scanAnswer = spot.Scan(10, 300)).ENEMY_ID != spot.ID) spot.Shot(scanAnswer.RANGE, 300);
                    else if ((scanAnswer = spot.Scan(10, 330)).ENEMY_ID != spot.ID) spot.Shot(scanAnswer.RANGE, 330);
                }
            }
        }
	}
}
