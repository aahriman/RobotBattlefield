using BaseLibrary.battlefield;

namespace ObstacleMod {
    /// <summary>
    /// Interface for obstacle witch modify moving or react somehow to moving.
    /// </summary>
    public interface IMoveInfluence : IObstacle {

        /// <summary>
        /// True if robot can stand (be generated) on this obstacle otherwise false.
        /// </summary>
        bool Standable { get; }

        /// <summary>
        /// </summary>
        /// <param name="robot">its X and Y value is set to intersect with obstacle</param>
        /// <param name="turn">in what turn we are</param>
        /// <param name="fromX">x - coordinate from where the robot go. This coordinate can be change by obstacle.</param>
        /// <param name="fromY">y - coordinate from where the robot go. This coordinate can be change by obstacle.</param>
        /// <param name="toX">x - coordinate to where the bullet go. This coordinate can be change by obstacle.</param>
        /// <param name="toY">y - coordinate to where the bullet go. This coordinate can be change by obstacle.</param>
        void Change(Robot robot, int turn, ref double fromX, ref double fromY, ref double toX, ref double toY);
    }
}
