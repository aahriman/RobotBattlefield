﻿namespace BaseLibrary.battlefield {
    /// <summary>
    /// Indicate lap states.
    /// </summary>
	public enum LapState {

        /// <summary>
        /// Nothing happens.
        /// </summary>
        NONE,

        /// <summary>
        /// Someone win the battle.
        /// </summary>
		SOMEONE_WIN,

        /// <summary>
        /// Out of turns in lap.
        /// </summary>
        TURNS_OUT
	};
}
