using SGS29.Utilities;
using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Represents a single cell in the grid.
    /// </summary>
    public class Cell : MonoBehaviour
    {
        /// <summary>
        /// The type of this cell, used to determine its behavior and characteristics.
        /// </summary>
        public CellType CellType;

        /// <summary>
        /// Indicates whether this cell should be added to the grid at the start.
        /// </summary>
        [SerializeField] private bool pushInStart;

        /// <summary>
        /// Called when the script instance is being loaded.
        /// If <see cref="pushInStart"/> is true, the cell is added to the grid automatically.
        /// </summary>
        private void Start()
        {
            if (pushInStart) PushCell();
        }

        /// <summary>
        /// Adds this cell to the grid managed by the <see cref="Grid"/> singleton instance.
        /// </summary>
        public void PushCell() => SM.Instance<Grid>().AddCell(this);
    }
}