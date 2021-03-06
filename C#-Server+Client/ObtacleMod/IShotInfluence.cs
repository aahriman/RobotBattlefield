﻿namespace ObstacleMod {
    /// <summary>
    /// Interface for obstacle witch modify shooting. Obstacle can modify trajectory of bullet.
    /// </summary>
    public interface IShotInfluence : IObstacle {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="turn">in what turn we are</param>
        /// <param name="fromX">x - coordinate from where the bullet go.</param>
        /// <param name="fromY">y - coordinate from where the bullet go.</param>
        /// <param name="toX">x - coordinate to where the bullet go. This coordinate can be change by obstacle.</param>
        /// <param name="toY">y - coordinate to where the bullet go. This coordinate can be change by obstacle.</param>
        /// <returns>true - if change toX or toY, false - otherwise</returns>
        bool Change(int turn, double fromX, double fromY, ref double toX, ref double toY);
    }
}
