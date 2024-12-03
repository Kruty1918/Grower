using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Interface for listening to swipe events with direction and type.
    /// </summary>
    public interface ISwipeListener
    {
        #region Methods

        /// <summary>
        /// Called when a swipe is detected with its direction and type.
        /// </summary>
        /// <param name="type">The type of the swipe direction (e.g., Up, Down, Left, Right).</param>
        /// <param name="direction">The direction of the swipe as a <see cref="Vector2"/>.</param>
        void OnSwipe(CardinalDirection type, Vector2 direction);

        #endregion
    }
}