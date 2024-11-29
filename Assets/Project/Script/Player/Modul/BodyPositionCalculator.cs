using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Handles the calculation of body segment positions and movement checks.
    /// </summary>
    public class BodyPositionCalculator : IBodyPositionCalculator
    {
        /// <summary>
        /// Calculates the position aligned to the grid, considering the grid size and offset.
        /// </summary>
        /// <param name="position">The original position to be aligned.</param>
        /// <param name="gridSize">The size of the grid for alignment.</param>
        /// <param name="offset">The offset applied during alignment.</param>
        /// <returns>The aligned position.</returns>
        public Vector3 CalculateAlignedPosition(Vector3 position, float gridSize, Vector3 offset)
        {
            return GridUtility.AlignToGrid(position, gridSize, offset);
        }

        /// <summary>
        /// Checks if the head has moved from the last recorded grid position.
        /// </summary>
        /// <param name="currentGridPosition">The current position of the snake's head on the grid.</param>
        /// <param name="lastGridPosition">The last recorded position of the snake's head on the grid.</param>
        /// <returns>True if the head has moved, false if not.</returns>
        public bool HasHeadMoved(Vector3 currentGridPosition, Vector3 lastGridPosition)
        {
            return currentGridPosition != lastGridPosition;
        }
    }
}