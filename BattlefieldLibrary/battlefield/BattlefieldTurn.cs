using System;
using System.Collections.Generic;
using System.Linq;
using ViewerLibrary;

namespace BattlefieldLibrary.battlefield {
    
    public class BattlefieldTurn {
        /// <summary>
        /// How many object are registered.
        /// </summary>
        private static int registeredObject = 0;

        /// <summary>
        /// Register next serialize unit.
        /// </summary>
        /// <returns></returns>
        public static int RegisterMore() {
            return registeredObject++;
        }

        private readonly List<Scan> scans = new List<Scan>();
        private readonly List<ViewerLibrary.Bullet> bullets = new List<ViewerLibrary.Bullet>();
        private readonly List<ViewerLibrary.Mine> mines = new List<ViewerLibrary.Mine>();
        private readonly List<ViewerLibrary.Repair> repairs = new List<ViewerLibrary.Repair>();
        private readonly List<Robot> robots = new List<Robot>();
        private readonly object[][] more = new object[registeredObject][];

        private readonly int turn;

        internal BattlefieldTurn(int turn) {
            this.turn = turn;
        }

        /// <summary>
        /// Add information about bullet to visualize it.
        /// </summary>
        /// <param name="bullet"></param>
        public void AddBullet(ViewerLibrary.Bullet bullet) {
            bullets.Add(bullet);
        }

        /// <summary>
        /// Add information about mine to visualize it.
        /// </summary>
        /// <param name="mine"></param>
        public void AddMine(ViewerLibrary.Mine mine) {
            mines.Add(mine);
        }

        /// <summary>
        /// Add information about repair to visualize it.
        /// </summary>
        /// <param name="repair"></param>
        public void AddRepair(ViewerLibrary.Repair repair) {
            repairs.Add(repair);
        }

        /// <summary>
        /// Add information about scan to visualize it.
        /// </summary>
        /// <param name="scan"></param>
        public void AddScan(Scan scan) {
            scans.Add(scan);
        }

        /// <summary>
        /// Add information about robot to visualize it.
        /// </summary>
        /// <param name="robot"></param>
        public void AddRobot(Robot robot) {
            robots.Add(robot);
        }

        /// <summary>
        /// Add information about more object (for. ex. flag) to visualize it. 
        /// </summary>
        /// <param name="object">added object</param>
        /// <param name="position">position where this kind of object was registered.</param>
        public void AddMore(object[] @object, int position) {
            more[position] = @object;
        }

        /// <summary>
        /// Convert this object to object Turn
        /// </summary>
        /// <returns></returns>
        public Turn ConvertToTurn() {
            return new Turn(turn, bullets.ToArray(), mines.ToArray(), repairs.ToArray(), robots.ToArray(), scans.ToArray(), more);
        }
    }
}
