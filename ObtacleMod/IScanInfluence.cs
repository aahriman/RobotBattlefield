namespace ObstacleMod {
    /// <summary>
    /// Interface for obstacle witch modify scanning. Obstacle can avoid scan through them.
    /// </summary>
    public interface IScanInfluence : IObstacle {

        /// <summary>
        /// Can scan ray go through this obstacle when it go from [fromX, fromY] at this turn?
        /// </summary>
        /// <param name="turn">in what turn we are</param>
        /// <param name="fromX">x - coordinate from where the scan ray go</param>
        /// <param name="fromY">y - coordinate from where the scan ray go</param>
        /// <param name="toX">x - coordinate to where the scan ray go</param>
        /// <param name="toY">y - coordinate to where the scan ray go</param>
        /// <returns>true - it is possible to scan through this obstacle</returns>
        bool CanScan(int turn, double fromX, double fromY, double toX, double toY);
    }
}
