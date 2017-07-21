namespace ObstacleMod {
    public interface IScanInfluence : IObstacle{

        /// <summary>
        /// </summary>
        /// <param name="turn">in what turn we are</param>
        /// <param name="fromX">x - coordinate from where the scan ray go</param>
        /// <param name="fromY">y - coordinate from where the scan ray go</param>
        /// <param name="toX">x - coordinate to where the scan ray go</param>
        /// <param name="toY">y - coordinate to where the scan ray go</param>
        /// <returns>true - it is possible to scan throught this obtacle</returns>
        bool CanScan(int turn, double fromX, double fromY, double toX, double toY);
    }
}
