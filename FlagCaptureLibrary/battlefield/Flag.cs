namespace FlagCaptureLibrary.battlefield {
    public class Flag {
        public int RobotId;
        public readonly int FROM_FLAGPLACE_ID;

        public Flag(int FROM_FLAGPLACE_ID) {
            this.FROM_FLAGPLACE_ID = FROM_FLAGPLACE_ID;
        }

        public Flag(int FROM_FLAGPLACE_ID, int RobotId) {
            this.FROM_FLAGPLACE_ID = FROM_FLAGPLACE_ID;
            this.RobotId = RobotId;
        }
    }
}
