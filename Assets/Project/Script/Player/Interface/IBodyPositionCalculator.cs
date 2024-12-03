using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Interface for calculating body segment positions within the grid.
    /// This interface defines methods for calculating aligned positions of body segments and checking if the head has moved relative to the previous position.
    /// It is used by entities with dynamic bodies (e.g., a snake or any other segmented object) to manage segment placement and movement.
    /// </summary>
    public interface IBodyPositionCalculator
    {
        /// <summary>
        /// Calculates the aligned position of a body segment on the grid.
        /// This method ensures the segment's position is correctly aligned with the grid based on the grid size and an optional offset.
        /// The alignment is typically used to place body segments at correct intervals or grid locations.
        /// </summary>
        /// <param name="position">The initial position of the segment before alignment.</param>
        /// <param name="gridSize">The size of each cell in the grid. It is used to align the segment to grid coordinates.</param>
        /// <param name="offset">An additional offset to apply to the calculated position, useful for slight adjustments or offsets between segments.</param>
        /// <returns>The aligned position for the body segment on the grid.</returns>
        Vector3 CalculateAlignedPosition(Vector3 position, float gridSize, Vector3 offset);

        /// <summary>
        /// Checks whether the head of the entity has moved to a different grid position.
        /// This method is useful for determining if the entity's head has changed position, which could be used to trigger movement logic or other events.
        /// </summary>
        /// <param name="currentGridPosition">The current grid position of the head.</param>
        /// <param name="lastGridPosition">The last known grid position of the head.</param>
        /// <returns>True if the head has moved to a new position, otherwise false.</returns>
        bool HasHeadMoved(Vector3 currentGridPosition, Vector3 lastGridPosition);
    }
}