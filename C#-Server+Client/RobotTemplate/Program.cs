using System;
using System.Collections.Generic;

using BaseLibrary.battlefield;
using BaseLibrary.communication.command.common;
using BaseLibrary.communication.command.miner;
using BaseLibrary.communication.command.tank;
using BaseLibrary.communication.command.repairman;
using BaseLibrary.communication.command.handshake;
using BaseLibrary.equipment;
using BaseLibrary.utils.euclidianSpaceStruct;
using BaseLibrary.utils;

using ClientLibrary.robot;
using ClientLibrary.config;

namespace RobotTemplate {
    class Program {
        static void Main(string[] args) {
            ClientRobot.Connect(args);
            Tank mujRobot = new Tank("My first tank", ClientRobot.TEAM_NAME);

            /* Odkud níže piště kód*/
        }
    }
}
