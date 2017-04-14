using BaseLibrary.battlefield;

namespace ObtacleMod {
    public interface IMoveInfluence : IObtacle{
        /// <summary>
        /// </summary>
        /// <param name="robot">its X and Y value is set to intersect with obtacle</param>
        /// <param name="turn">in what turn we are</param>
        /// <param name="fromX"></param>
        /// <param name="fromY"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        void Change(Robot robot, int turn, ref double fromX, ref double fromY, ref double toX, ref double toY);
    }
}
