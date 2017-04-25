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
        private readonly List<Robot> robots = new List<Robot>();
        private readonly Object[] more = new Object[registratedObject];
        private readonly List<Flag> flags = new List<Flag>();

        private readonly int turn;

        internal BattlefieldTurn(int turn) {
            this.turn = turn;
        }

        public void AddBullet(ViewerLibrary.Bullet bullet) {
            bullets.Add(bullet);
        }

        public void AddScan(Scan scan) {
            scans.Add(scan);
        }

        public void AddRobot(Robot robot) {
            robots.Add(robot);
        }

        public void AddMore(Object @object, int position) {
            more[position] = @object;
        }

        public void AddFlag(Flag flag) {
            flags.Add(flag);
        }

        public Turn ConvertToTurn() {
            Object[][] moreObject = (from o in more
                                     select ((List<Object>) o).ToArray()).ToArray();
            return new Turn(turn, bullets.ToArray(), robots.ToArray(), scans.ToArray(), moreObject);
        }
    }
}
