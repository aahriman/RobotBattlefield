using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewerLibrary.model
{
    public interface ITurnDataModel
    {
        /// <summary>
        /// Returns next turn
        /// </summary>
        Turn Next();

        /// <summary>
        /// It is possible to get next turn.
        /// </summary>
        /// <returns>true - if is possible to get next turn, false - if there is not more turns</returns>
        bool HasNext();
    }

    public interface IReversibleTurnDataModel : ITurnDataModel
    {
        /// <summary>
        /// Returns previous turn
        /// </summary>
        Turn Previous();

        /// <summary>
        /// It is possible to get previous turn.
        /// </summary>
        /// <returns>true - if is possible to get previous turn, false - if there is not more turns</returns>
        bool HasPrevious();
    }
}
