using System.Drawing;

namespace ViewerLibrary.gui {
	public interface IDrawer {
		void DrawTurn(Turn turn, Graphics g);
	    void DrawExplodedBullet(Bullet bullet, Graphics g, int lap);
        void DrawExplodedMine(Mine mine, Graphics g, int lap);
        void DrawRepair(Repair repair, Graphics g, int lap);
        void DrawScan(Scan scan, Graphics g, int lap);

	    void RegisterDrawerMore(IDrawerMore drawer);
	}
}
