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
}