namespace Grower
{
    /// <summary>
    /// Enumeration of possible cell types used within the grid.
    /// This enum defines the different categories or types a cell can belong to within the grid system.
    /// It is used to distinguish between various cell types, such as walls or other types of objects that can be placed in the grid.
    /// </summary>
    public enum CellType
    {
        /// <summary>
        /// Represents a wall cell.
        /// Typically used to define obstacles or boundaries within the grid that cannot be passed through.
        /// </summary>
        Wall,

        /// <summary>
        /// Represents a body cell.
        /// Used for cells that are part of an entity or object, such as a player or other dynamic objects within the grid.
        /// </summary>
        Body
    }
}