using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Abstract base class for handling input events.
    /// </summary>
    public abstract class InputHandler
    {
        #region Events

        /// <summary>
        /// Event triggered when a swipe is detected.
        /// </summary>
        public event System.Action<CardinalDirection, Vector2> OnSwipe;

        #endregion

        #region Methods

        /// <summary>
        /// Handles the start of an input interaction.
        /// </summary>
        /// <param name="position">The starting position of the input.</param>
        /// <param name="time">The starting time of the input.</param>
        public abstract void HandleInputStart(Vector2 position, float time);

        /// <summary>
        /// Handles the end of an input interaction.
        /// </summary>
        /// <param name="position">The ending position of the input.</param>
        /// <param name="time">The ending time of the input.</param>
        public abstract void HandleInputEnd(Vector2 position, float time);

        #endregion
    }
}