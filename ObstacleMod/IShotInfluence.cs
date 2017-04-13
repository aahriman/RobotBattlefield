﻿namespace ObtacleMod {
    public interface IShotInfluence : IObtacle {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="turn">in what turn we are</param>
        /// <param name="fromX"></param>
        /// <param name="fromY"></param>
        /// <param name="toX"></param>
        /// <param name="toY"></param>
        /// <returns>true - if change toX or toY</returns>
        bool Change(int turn, double fromX, double fromY, ref double toX, ref double toY);
    }
}
