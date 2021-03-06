﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BaseLibrary.communication.command.common;
using ClientLibrary.robot;

namespace Spot {
	class Program {
	
        public static void Main(string[] args) {
            Console.WriteLine("Spot start.");
            string name = "Spot";
            Tank spot = new Tank(name, ClientRobot.TEAM_NAME);

            ClientRobot.Connect(args);
            Console.WriteLine("Spot is ready for action.");

            int direction = 90;
            while (true) {
                if (spot.Power.Equals(0)) {
                    spot.Drive(direction, 100);
                } else if ((direction == 90 && spot.Y > 575)
                    || (direction == 270 && spot.Y < 425)
                    || (direction == 180 && spot.X < 150)
                    || (direction == 0 && spot.X > 850)) {
                        spot.Drive(direction, 0);
                        direction = (direction + 90) % 360;
                } else {
                    for (int angle = 0; angle < 360; angle += 30) {
                        ScanAnswerCommand scanAnswer;
                        if ((scanAnswer = spot.Scan(angle, 10)).ENEMY_ID != spot.ID) {
                            spot.Shoot(angle, scanAnswer.RANGE);
                        }
                    }
                }
            }}
	}
}
