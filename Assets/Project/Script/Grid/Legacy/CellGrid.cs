using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Manages the grid of cells and their corresponding data.
    /// The grid stores cells and their data, allowing for management of cell operations like adding, removing, and accessing cell data by their coordinates.
    /// </summary>
    [Serializable]
    public class CellGrid
    {
        /// <summary>
        /// A dictionary that stores cell data, indexed by the coordinates (x, z) of the cells.
        /// The key is a <see cref="Vector2Int"/> representing the coordinates, and the value is a <see cref="CellData"/> object representing the data associated with the cell.
        /// </summary>
        [ShowInInspector, ReadOnly, DictionaryDrawerSettings(KeyLabel = "Coordinates", ValueLabel = "Cell Data")]
        private readonly Dictionary<Vector2Int, CellData> data = new();

        /// <summary>
        /// A list that holds the cells in the grid.
        /// Cells are added or removed dynamically as needed.
        /// </summary>
        [ShowInInspector, ReadOnly, ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        private readonly List<Cell> cells = new();

        #region Cell Management

        /// <summary>
        /// Adds a new cell to the grid.
        /// The cell is added if its coordinate does not already exist in the grid.
        /// A new <see cref="CellData"/> is created and associated with the cell's coordinate.
        /// </summary>
        /// <param name="cell">The cell to add to the grid.</param>
        public void AddCell(Cell cell)
        {
            // Extract coordinates from the cell's position in the world
            var cellCoord = new Vector2Int((int)cell.transform.position.x, (int)cell.transform.position.z);

            // Check if the cell already exists at these coordinates
            if (data.ContainsKey(cellCoord)) return;

            // Create and store the new cell's data
            var cellData = new CellData(cellCoord, cell.transform.position, cell.CellType);
            data[cellCoord] = cellData;
            cells.Add(cell); // Add the cell to the cells list
        }

        /// <summary>
        /// Removes a cell from the grid based on its position.
        /// If the cell is present, it is removed both from the dictionary and the list of cells.
        /// </summary>
        /// <param name="cell">The cell to remove from the grid.</param>
        public void RemoveCell(Cell cell)
        {
            var cellCoord = new Vector2Int((int)cell.transform.position.x, (int)cell.transform.position.z);
            if (data.Remove(cellCoord)) cells.Remove(cell); // Remove both from data and the list
        }

        /// <summary>
        /// Retrieves the cell at the specified coordinates.
        /// </summary>
        /// <param name="cellCoord">The coordinates of the cell to retrieve.</param>
        /// <returns>The <see cref="Cell"/> at the specified coordinates, or null if no cell is found.</returns>
        public Cell GetCell(Vector2Int cellCoord) =>
            data.ContainsKey(cellCoord) ? cells.Find(c => data[cellCoord].Position == c.transform.position) : null;

        /// <summary>
        /// Gets the first cell in the grid.
        /// </summary>
        /// <returns>The first <see cref="Cell"/> in the grid, or null if the grid is empty.</returns>
        public Cell GetFirstCell() => cells.Count > 0 ? cells[0] : null;

        /// <summary>
        /// Gets the last cell in the grid.
        /// </summary>
        /// <returns>The last <see cref="Cell"/> in the grid, or null if the grid is empty.</returns>
        public Cell GetLastCell() => cells.Count > 0 ? cells[^1] : null;

        /// <summary>
        /// Gets a random cell from the grid.
        /// </summary>
        /// <returns>A random <see cref="Cell"/> from the grid, or null if the grid is empty.</returns>
        public Cell GetRandomCell() => cells.Count > 0 ? cells[UnityEngine.Random.Range(0, cells.Count)] : null;

        #endregion

        #region CellData Management

        /// <summary>
        /// Retrieves a list of coordinates for all cells in the grid.
        /// </summary>
        /// <returns>A list of <see cref="Vector2Int"/> representing the coordinates of all cells in the grid.</returns>
        public List<Vector2Int> GetAllCellCoordinates()
        {
            return new List<Vector2Int>(data.Keys);
        }

        /// <summary>
        /// Retrieves the data associated with the cell at the specified coordinates.
        /// </summary>
        /// <param name="cellCoord">The coordinates of the cell whose data is to be retrieved.</param>
        /// <returns>The <see cref="CellData"/> at the specified coordinates, or null if no data exists at that location.</returns>
        public CellData GetCellData(Vector2Int cellCoord) =>
            data.TryGetValue(cellCoord, out var cellData) ? cellData : null;

        /// <summary>
        /// Gets the data of the first cell in the grid.
        /// </summary>
        /// <returns>The data of the first <see cref="CellData"/> in the grid, or null if the grid is empty.</returns>
        public CellData GetFirstCellData() =>
            cells.Count > 0
            ? data[new Vector2Int((int)cells[0].transform.position.x, (int)cells[0].transform.position.z)]
            : null;

        /// <summary>
        /// Gets the data of the last cell in the grid.
        /// </summary>
        /// <returns>The data of the last <see cref="CellData"/> in the grid, or null if the grid is empty.</returns>
        public CellData GetLastCellData() =>
            cells.Count > 0
            ? data[new Vector2Int((int)cells[^1].transform.position.x, (int)cells[^1].transform.position.z)]
            : null;

        /// <summary>
        /// Checks if the grid contains the specified cell.
        /// </summary>
        /// <param name="cell">The <see cref="Cell"/> to check for.</param>
        /// <returns>True if the grid contains the cell, otherwise false.</returns>
        public bool Contains(Cell cell) => cells.Contains(cell);

        /// <summary>
        /// Checks if the grid contains the specified coordinates.
        /// </summary>
        /// <param name="coord">The coordinates to check for.</param>
        /// <returns>True if the grid contains the specified coordinates, otherwise false.</returns>
        public bool Contains(Vector2Int coord) => data.ContainsKey(coord);

        /// <summary>
        /// Compares two cells based on their position.
        /// </summary>
        /// <param name="A">The first cell.</param>
        /// <param name="B">The second cell.</param>
        /// <returns>True if the two cells are at the same position, otherwise false.</returns>
        public bool Compare(Cell A, Cell B) => A.transform.position == B.transform.position;

        /// <summary>
        /// Checks if the grid contains the specified cell data.
        /// </summary>
        /// <param name="cellData">The <see cref="CellData"/> to check for.</param>
        /// <returns>True if the grid contains the specified cell data, otherwise false.</returns>
        public bool Contains(CellData cellData) => data.ContainsValue(cellData);

        /// <summary>
        /// Retrieves a random cell's data from the grid.
        /// The corresponding cell's method <see cref="Cell.PushCell"/> is also called as part of this operation.
        /// </summary>
        /// <returns>The <see cref="CellData"/> of a random cell, or null if the grid is empty.</returns>
        public CellData GetRandomCellData()
        {
            var cell = GetRandomCell();
            if (cell != null)
            {
                // Call a method on the cell before returning the data
                cell.GetComponent<Cell>().PushCell();
                return cell.GetComponent<CellData>(); // Return the cell's data
            }
            return null;
        }

        public List<Cell> GetCellsByType(CellType cellType)
        {
            List<Cell> cellsByType = new List<Cell>();

            foreach (var cell in cells)
            {
                if (cell.CellType == cellType) cellsByType.Add(cell);
            }

            return cellsByType;
        }

        #endregion
    }
}