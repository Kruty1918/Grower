using System.Collections.Generic;
using SGS29.Utilities;
using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Represents the grid system and manages the cells within it.
    /// </summary>
    public class Grid : MonoSingleton<Grid>
    {
        /// <summary>
        /// Internal representation of the cell grid.
        /// </summary>
        [SerializeField] private CellGrid cellGrid;

        /// <summary>
        /// Initializes the singleton instance and the internal cell grid.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            cellGrid = new CellGrid();
        }

        /// <summary>
        /// Adds a new cell to the grid.
        /// </summary>
        /// <param name="cell">The cell to add.</param>
        public void AddCell(Cell cell) => cellGrid.AddCell(cell);

        /// <summary>
        /// Removes a cell from the grid.
        /// </summary>
        /// <param name="cell">The cell to remove.</param>
        public void RemoveCell(Cell cell) => cellGrid.RemoveCell(cell);

        /// <summary>
        /// Retrieves a cell from the grid based on its coordinates.
        /// </summary>
        /// <param name="coord">The coordinates of the cell.</param>
        /// <returns>The cell at the specified coordinates, or null if not found.</returns>
        public Cell GetCell(Vector2Int coord) => cellGrid.GetCell(coord);

        /// <summary>
        /// Retrieves the data of a cell at the specified coordinates.
        /// </summary>
        /// <param name="coord">The coordinates of the cell.</param>
        /// <returns>The data of the cell at the specified coordinates.</returns>
        public CellData GetCellData(Vector2Int coord) => cellGrid.GetCellData(coord);

        /// <summary>
        /// Gets all coordinates of cells currently stored in the grid.
        /// </summary>
        /// <returns>A list of all cell coordinates in the grid.</returns>
        public List<Vector2Int> GetAllCellCoordinates() => cellGrid.GetAllCellCoordinates();

        /// <summary>
        /// Retrieves all cells of a specific type.
        /// </summary>
        /// <param name="cellType">The type of cells to retrieve.</param>
        /// <returns>A list of cells that match the specified type.</returns>
        public List<Cell> GetCellsByType(CellType cellType) => cellGrid.GetCellsByType(cellType);

        /// <summary>
        /// Checks if a cell exists at the specified coordinates.
        /// </summary>
        /// <param name="coord">The coordinates to check.</param>
        /// <returns>True if a cell exists at the specified coordinates; otherwise, false.</returns>
        public bool ContainsCell(Vector2Int coord) => cellGrid.Contains(coord);
    }
}