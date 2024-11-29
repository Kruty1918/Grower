using SGS29.Utilities;
using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Represents the grid system and manages the cells within it.
    /// </summary>
    public class Grid : MonoSingleton<Grid>
    {
        [SerializeField] private CellGrid cellGrid;

        protected override void Awake()
        {
            base.Awake();
            cellGrid = new CellGrid();
        }

        public void AddCell(Cell cell) => cellGrid.AddCell(cell);

        public void RemoveCell(Cell cell) => cellGrid.RemoveCell(cell);

        public Cell GetCell(Vector2Int coord) => cellGrid.GetCell(coord);

        public CellData GetCellData(Vector2Int coord) => cellGrid.GetCellData(coord);
        public bool ContainsCell(Vector2Int coord) => cellGrid.Contains(coord);
    }

    /// <summary>
    /// Utility class for grid-related position calculations.
    /// </summary>
    public static class GridUtility
    {
        /// <summary>
        /// Aligns a single axis value to the grid.
        /// </summary>
        /// <param name="value">The value to align.</param>
        /// <param name="gridSize">The size of the grid cells.</param>
        /// <returns>The aligned value.</returns>
        public static float AlignAxis(float value, float gridSize)
        {
            return Mathf.RoundToInt(value / gridSize) * gridSize;
        }

        /// <summary>
        /// Converts a value to grid coordinates as an integer.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The grid-aligned integer.</returns>
        public static int AlignAxisAsInt(float value, float gridSize)
        {
            return Mathf.RoundToInt(value / gridSize);
        }

        /// <summary>
        /// Aligns a position to the grid with an offset.
        /// </summary>
        /// <param name="position">The position to align.</param>
        /// <param name="gridSize">The size of the grid cells.</param>
        /// <param name="offset">The offset for alignment.</param>
        /// <returns>The aligned position.</returns>
        public static Vector3 AlignToGrid(Vector3 position, float gridSize, Vector3 offset)
        {
            position += offset;
            position.x = AlignAxis(position.x, gridSize);
            position.y = offset.y; // Maintain vertical alignment
            position.z = AlignAxis(position.z, gridSize);
            return position;
        }

        /// <summary>
        /// Converts a world position to grid coordinates.
        /// </summary>
        /// <param name="position">The position to convert.</param>
        /// <param name="gridSize">The size of the grid cells.</param>
        /// <param name="offset">The offset for alignment.</param>
        /// <returns>The grid coordinates as integers.</returns>
        public static Vector2Int ConvertToGridCoords(Vector3 position, float gridSize, Vector3 offset)
        {
            position += offset;
            return new Vector2Int(
                Mathf.RoundToInt(position.x / gridSize),
                Mathf.RoundToInt(position.z / gridSize)
            );
        }
    }
}