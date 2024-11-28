using System;
using System.Collections.Generic;
using SGS29.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Grower
{
    #region Cell and CellData Classes

    /// <summary>
    /// Represents a single cell in the grid.
    /// </summary>
    public class Cell : MonoBehaviour
    {
        public CellType CellType;
        [SerializeField] private bool pushInStart;

        private void Start() { if (pushInStart) PushCell(); }

        /// <summary>
        /// Adds this cell to the grid.
        /// </summary>
        public void PushCell() => SM.Instance<Grid>().AddCell(this);
    }

    /// <summary>
    /// Represents the data associated with a cell.
    /// </summary>
    public class CellData
    {
        public Vector2Int MatrixCoord { get; set; }
        public Vector3 Position { get; set; }
        public CellType CellType { get; set; }

        public CellData() { }

        public CellData(Vector2Int matrixCoord, Vector3 position, CellType cellType)
        {
            MatrixCoord = matrixCoord;
            Position = position;
            CellType = cellType;
        }
    }

    /// <summary>
    /// Enumeration of possible cell types.
    /// </summary>
    public enum CellType
    {
        Wall,
        Body
    }

    #endregion

    #region Grid and CellGrid

    /// <summary>
    /// Manages the grid of cells and their data.
    /// </summary>
    [Serializable]
    public class CellGrid
    {
        [ShowInInspector, ReadOnly, DictionaryDrawerSettings(KeyLabel = "Coordinates", ValueLabel = "Cell Data")]
        private readonly Dictionary<Vector2Int, CellData> data = new();

        [ShowInInspector, ReadOnly, ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        private readonly List<Cell> cells = new();

        #region Cell Management

        /// <summary>
        /// Adds a cell to the grid.
        /// </summary>
        public void AddCell(Cell cell)
        {
            var cellCoord = new Vector2Int((int)cell.transform.position.x, (int)cell.transform.position.z);
            if (data.ContainsKey(cellCoord)) return;

            var cellData = new CellData(cellCoord, cell.transform.position, cell.CellType);
            data[cellCoord] = cellData;
            cells.Add(cell);
        }

        /// <summary>
        /// Removes a cell from the grid.
        /// </summary>
        public void RemoveCell(Cell cell)
        {
            var cellCoord = new Vector2Int((int)cell.transform.position.x, (int)cell.transform.position.z);
            if (data.Remove(cellCoord)) cells.Remove(cell);
        }

        public Cell GetCell(Vector2Int cellCoord) =>
            data.ContainsKey(cellCoord) ? cells.Find(c => data[cellCoord].Position == c.transform.position) : null;

        public Cell GetFirstCell() => cells.Count > 0 ? cells[0] : null;

        public Cell GetLastCell() => cells.Count > 0 ? cells[^1] : null;

        public Cell GetRandomCell() => cells.Count > 0 ? cells[UnityEngine.Random.Range(0, cells.Count)] : null;

        #endregion

        #region CellData Management

        public CellData GetCellData(Vector2Int cellCoord) =>
            data.TryGetValue(cellCoord, out var cellData) ? cellData : null;

        public CellData GetFirstCellData() =>
            cells.Count > 0
            ? data[new Vector2Int((int)cells[0].transform.position.x, (int)cells[0].transform.position.z)]
            : null;

        public CellData GetLastCellData() =>
            cells.Count > 0
            ? data[new Vector2Int((int)cells[^1].transform.position.x, (int)cells[^1].transform.position.z)]
            : null;

        public bool Contains(Cell cell) => cells.Contains(cell);
        public bool Contains(Vector2Int coord) => data.ContainsKey(coord);

        public bool Compare(Cell A, Cell B) => A.transform.position == B.transform.position;

        public bool Contains(CellData cellData) => data.ContainsValue(cellData);

        public CellData GetRandomCellData()
        {
            var cell = GetRandomCell();
            if (cell != null)
            {
                cell.GetComponent<Cell>().PushCell(); // Викликаємо метод окремо
                return cell.GetComponent<CellData>(); // Повертаємо CellData
            }
            return null;
        }

        #endregion
    }

    #endregion
}