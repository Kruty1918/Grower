using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Represents the data associated with a cell in the grid.
    /// This class stores the position, matrix coordinates, and the type of the cell.
    /// It is used to manage and store the necessary data for each individual cell in the grid.
    /// </summary>
    public class CellData
    {
        /// <summary>
        /// The matrix coordinates of the cell in the grid.
        /// This is typically a 2D coordinate representing the cell's position within the grid structure.
        /// </summary>
        public Vector2Int MatrixCoord { get; set; }

        /// <summary>
        /// The world position of the cell.
        /// This is the 3D position in world space where the cell is located.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// The type of the cell, defined by the <see cref="CellType"/> enumeration.
        /// This can be used to distinguish between different types of cells (e.g., grass, water, etc.).
        /// </summary>
        public CellType CellType { get; set; }

        /// <summary>
        /// Default constructor for <see cref="CellData"/>.
        /// Initializes the properties with default values (zero vectors and a default cell type).
        /// </summary>
        public CellData() { }

        /// <summary>
        /// Constructor for <see cref="CellData"/> that initializes the properties with specified values.
        /// </summary>
        /// <param name="matrixCoord">The coordinates of the cell in the grid.</param>
        /// <param name="position">The world position of the cell.</param>
        /// <param name="cellType">The type of the cell.</param>
        public CellData(Vector2Int matrixCoord, Vector3 position, CellType cellType)
        {
            MatrixCoord = matrixCoord;
            Position = position;
            CellType = cellType;
        }
    }
}