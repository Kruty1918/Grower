using UnityEngine;

namespace Grower
{
    public static class Utility
    {
        /// <summary>
        /// Aligns a given position to the grid with an offset.
        /// </summary>
        /// <param name="position">The position to align.</param>
        /// <returns>The aligned position.</returns>
        public static Vector3 AlignToGrid(Vector3 position, MoverSettings settings)
        {
            position += settings.gridOffset;
            position.x = GridUtility.AlignAxis(position.x, settings.gridSize);
            position.y = settings.gridOffset.y; // Maintain vertical alignment
            position.z = GridUtility.AlignAxis(position.z, settings.gridSize);
            return position;
        }

        /// <summary>
        /// Converts a world position to grid coordinates.
        /// </summary>
        /// <param name="position">The position to convert.</param>
        /// <returns>The grid coordinates.</returns>
        public static Vector2Int ConvertToGridCoords(Vector3 position, MoverSettings settings)
        {
            position += settings.gridOffset;
            return new Vector2Int(GridUtility.AlignAxisAsInt(position.x, settings.gridSize),
            GridUtility.AlignAxisAsInt(position.z, settings.gridSize));
        }
    }
}