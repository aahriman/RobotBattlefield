using System;
using BaseLibrary;
using BaseLibrary.battlefield;
using BaseLibrary.command;
using BaseLibrary.command.common;
using BaseLibrary.command.handshake;
using JetBrains.Annotations;

namespace BattlefieldLibrary.battlefield.robot {
    public abstract class BattlefieldRobot : Robot{

        public double WantedPower { get; set; }
        public int OldScore {get; set; }
        public string NAME {
            get => _name;
            set {
                if (_name == null) {
                    this._name = value;
                } else {
                    throw new NotSupportedException("Name can be set only once.");
                }
            }
        }

        public RobotType ROBOT_TYPE {
            get => _robotType;
            set {
                if (_robotType != RobotType.NONE) {
                    throw new NotSupportedException("Robot type can be set only once.");
                }
                _robotType = value;
            }
        }

        public NetworkStream NETWORK_STREAM {get;}
        public DateTime LastRequestAt { get; set; }


        private string _name;
        private RobotType _robotType = RobotType.NONE;
        
        protected BattlefieldRobot(int teamId, int id, [NotNull] NetworkStream networkStream) {
            ID = id;
            TEAM_ID = teamId;
            NETWORK_STREAM = networkStream ?? throw new ArgumentNullException("NETWORK_STREAM can not be null");
        }

        
        
    }
}
