namespace FlagCaptureLibrary.battlefield {
    public class Flag {
        /// <summary>
        /// Robots id witch carry flag.
        /// </summary>
        public int RobotId;

        /// <summary>
        /// From what place is this flag.
        /// </summary>
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
