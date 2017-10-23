using System;
using BaseLibrary;
using BaseLibrary.battlefield;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;

namespace BattlefieldLibrary.battlefield.robot {
    public abstract class BattlefieldRobot : Robot{
        
        public abstract double WantedPower { get; set; }
        public abstract int OldScore {get; set; }
        public string NAME {
            get { return _name; }
            set {
                if (_name == null) {
                    this._name = value;
                } else {
                    throw new NotSupportedException("Name can be set only once.");
                }
            }
        }

        public RobotType ROBOT_TYPE {
            get { return _robotType; }
            set {
                if (_robotType != RobotType.NONE) {
                    throw new NotSupportedException("Robot type can be set only once.");
                }
                _robotType = value;
            }
        }

        public SuperNetworkStream SuperNetworkStream {get; private set;}
        public abstract DateTime LastRequestAt { get; set; }


        private string _name;
        private RobotType _robotType = RobotType.NONE;

        protected BattlefieldRobot(int id, SuperNetworkStream superNetworkStream) {
            if (superNetworkStream == null) {
                throw new ArgumentNullException("SuperNetworkStream can not be null");
            }
            ID = id;
            this.SuperNetworkStream = superNetworkStream;
        }

        protected BattlefieldRobot(int teamId, int id, SuperNetworkStream superNetworkStream) {
            if(superNetworkStream == null){
                throw new ArgumentNullException("SuperNetworkStream can not be null");
            }
            ID = id;
            TEAM_ID = teamId;
            this.SuperNetworkStream = superNetworkStream;
        }

        
        
    }
}
