using System.Drawing;

namespace ViewerLibrary.gui {
	public interface IDrawer {
        /// <summary>
        /// Draw one turn in graphics.
        /// </summary>
        /// <param name="turn"></param>
        /// <param name="g"></param>
		void DrawTurn(Turn turn, Graphics g);

        /// <summary>
        /// Draw bullets witch explodes.
        /// </summary>
        /// <param name="bullet">exploded bullet</param>
        /// <param name="g">graphics context</param>
        /// <param name="turn">before how many turn bullet was exploded</param>
	    void DrawExplodedBullet(Bullet bullet, Graphics g, int turn);

        /// <summary>
        /// Draw mine witch explodes.
        /// </summary>
        /// <param name="mine">exploded mine</param>
        /// <param name="g">graphics context</param>
        /// <param name="turn">before how many turn mine was exploded</param>
        void DrawExplodedMine(Mine mine, Graphics g, int turn);

        /// <summary>
        /// Draw repair.
        /// </summary>
        /// <param name="repair">repair object</param>
        /// <param name="g">graphics context</param>
        /// <param name="turn">before how many turn repair was done</param>
        void DrawRepair(Repair repair, Graphics g, int turn);
        void DrawScan(Scan scan, Graphics g, int turn);

        /// <summary>
        /// Register drawer more. This is used when turn have some object in more property.
        /// </summary>
        /// <param name="drawer"></param>
	    void RegisterDrawerMore(IDrawerMore drawer);
	}
}
