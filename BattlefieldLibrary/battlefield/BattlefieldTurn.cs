using System;
using System.Collections.Generic;
using System.Linq;
using ViewerLibrary;

namespace BattlefieldLibrary.battlefield {
    
    public class BattlefieldTurn {
        private static int registratedObject = 0;

        public static int RegisterMore() {
            return registratedObject++;
        }

        private readonly List<Scan> scans = new List<Scan>();
        private readonly List<ViewerLibrary.Bullet> bullets = new List<ViewerLibrary.Bullet>();
        private readonly List<ViewerLibrary.Mine> mines = new List<ViewerLibrary.Mine>();
        private readonly List<ViewerLibrary.Repair> repairs = new List<ViewerLibrary.Repair>();
        private readonly List<Robot> robots = new List<Robot>();
        private readonly object[] more = new object[registratedObject];

        private readonly int turn;

        internal BattlefieldTurn(int turn) {
            this.turn = turn;
        }

        public void AddBullet(ViewerLibrary.Bullet bullet) {
            bullets.Add(bullet);
        }

        public void AddMine(ViewerLibrary.Mine mine) {
            mines.Add(mine);
        }

        public void AddRepair(ViewerLibrary.Repair repair) {
            repairs.Add(repair);
        }

        public void AddScan(Scan scan) {
            scans.Add(scan);
        }

        public void AddRobot(Robot robot) {
            robots.Add(robot);
        }

        public void AddMore(object @object, int position) {
            more[position] = @object;
        }

        public Turn ConvertToTurn() {
            object[][] moreObject = (from o in more
                                     select  (object[]) o).ToArray();
            return new Turn(turn, bullets.ToArray(), mines.ToArray(), repairs.ToArray(), robots.ToArray(), scans.ToArray(), moreObject);
        }
    }
}
