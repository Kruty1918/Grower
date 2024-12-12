using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Interface for listening to input events such as swipes.
    /// </summary>
    public interface IInputListener
    {
        #region Methods

        /// <summary>
        /// Called when a swipe input is detected.
        /// </summary>
        /// <param name="direction">The direction of the swipe as a <see cref="Vector2"/>.</param>
        void OnSwipe(Vector2 direction);

        #endregion
    }
}