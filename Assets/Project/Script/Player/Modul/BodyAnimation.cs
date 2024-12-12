using UnityEngine;
using SGS29.Utilities;

namespace Grower
{
    /// <summary>
    /// Handles the animations for body segments.
    /// </summary>
    public class BodyAnimation : IBodyAnimation
    {
        /// <summary>
        /// Animates the spawning of a body segment at the target position.
        /// </summary>
        /// <param name="targetPosition">The target position where the segment should spawn.</param>
        /// <param name="bodyPrefab">The prefab for the body segment to instantiate.</param>
        /// <param name="gridSize">The size of the grid cells for alignment.</param>
        public void AnimateBodySegmentSpawn(Vector3 targetPosition, Cell bodyPrefab, float gridSize)
        {
            // Align the target position to the nearest grid coordinates
            Vector2Int cellCoords = new Vector2Int(
                GridUtility.AlignAxisAsInt(targetPosition.x, gridSize),
                GridUtility.AlignAxisAsInt(targetPosition.z, gridSize)
            );

            // If a cell already exists at the calculated position, return early to prevent duplication
            if (SM.Instance<Grid>().ContainsCell(cellCoords))
            {
                return;
            }

            // Instantiate the body segment at the aligned position
            Cell segment = Object.Instantiate(bodyPrefab, targetPosition, Quaternion.identity);

            // Push the new cell into the grid
            segment.PushCell();
        }
    }
}