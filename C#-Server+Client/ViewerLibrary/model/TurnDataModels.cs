using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerLibrary.model
{
    /// <summary>
    /// Allow to return future Turn. First turn is returned by first call Next().
    /// </summary>
    public interface ITurnDataModel
    {
        /// <summary>
        /// Returns next turn. Null when no next turn exist.
        /// </summary>
        Turn Next();

        /// <summary>
        /// It is possible to get next turn.
        /// </summary>
        /// <returns>true - if is possible to get next turn, false - if there is not more turns</returns>
        bool HasNext();
    }

    /// <summary>
    /// This allow to step backward.
    /// </summary>
    public interface IReversibleTurnDataModel : ITurnDataModel
    {
        /// <summary>
        /// Returns previous turn. Null when no previous turn exists.
        /// </summary>
        Turn Previous();

        /// <summary>
        /// It is possible to get previous turn.
        /// </summary>
        /// <returns>true - if is possible to get previous turn, false - if there is not more turns</returns>
        bool HasPrevious();
    }
}
