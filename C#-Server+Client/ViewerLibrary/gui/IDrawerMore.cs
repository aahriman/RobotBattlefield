using System;
using System.Drawing;

namespace ViewerLibrary.gui {
    public interface IDrawerMore {

        /// <summary>
        /// Draw something more. For ex. Flag, base etc. For every mod it is needed.
        /// </summary>
        /// <param name="more">one field od more object</param>
        /// <param name="g">graphics context</param>
        void DrawMore(object[] more, Graphics g);
    }
}
