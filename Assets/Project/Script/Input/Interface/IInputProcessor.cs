using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Interface for processing input and returning direction.
    /// </summary>
    public interface IInputProcessor
    {
        #region Methods

        /// <summary>
        /// Gets the direction of the input as a <see cref="Vector2"/>.
        /// </summary>
        /// <returns>The input direction as a normalized <see cref="Vector2"/>.</returns>
        Vector2 GetInputDirection();

        #endregion
    }
}
