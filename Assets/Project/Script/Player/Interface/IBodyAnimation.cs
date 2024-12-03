using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Interface for body animation behavior.
    /// This interface defines a method for animating the spawn of body segments within the grid.
    /// Any class implementing this interface should provide the logic to animate the appearance of new body segments (e.g., for a growing entity).
    /// </summary>
    public interface IBodyAnimation
    {
        /// <summary>
        /// Animates the spawning of a body segment at a target position.
        /// This method is meant to provide visual feedback when a new segment of the body (such as a growing snake or entity) is spawned.
        /// The animation should consider the grid size for proper placement and scaling.
        /// </summary>
        /// <param name="targetPosition">The target position where the body segment will appear.</param>
        /// <param name="bodyPrefab">The prefab for the body segment that will be spawned.</param>
        /// <param name="gridSize">The size of each cell in the grid, used to adjust positioning and scaling of the body segment.</param>
        void AnimateBodySegmentSpawn(Vector3 targetPosition, Cell bodyPrefab, float gridSize);
    }
}