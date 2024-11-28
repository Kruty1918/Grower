using SGS29.Utilities;
using UnityEngine;

namespace Grower
{
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
}