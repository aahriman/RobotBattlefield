namespace ObtacleMod {
    public interface IScanInfluence : IObtacle{

        /// <summary>
        /// </summary>
        /// <param name="turn">in what turn we are</param>
        /// <param name="fromX"></param>
        /// <param name="fromY"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <returns>it is possible to scan throught this obtacle or not</returns>
        bool CanScan(int turn, double fromX, double fromY, double toX, double toY);
    }
}
