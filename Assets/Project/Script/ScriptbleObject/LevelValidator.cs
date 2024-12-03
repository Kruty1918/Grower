using UnityEngine;

namespace Grower
{
    /// <summary>
    /// A class responsible for validating whether a level is complete based on filled cells.
    /// </summary>
    [CreateAssetMenu(fileName = "NewLevelValidator", menuName = "Grower/Level Validator")]
    public class LevelValidator : ScriptableObject
    {
        #region Fields

        /// <summary>
        /// The number of cells that need to be filled to complete the level.
        /// </summary>
        [SerializeField] protected int needCellToFill = 9;

        #endregion

        #region Methods

        /// <summary>
        /// Checks if the level is complete based on the number of filled cells.
        /// </summary>
        /// <param name="fillCell">The number of cells that have been filled.</param>
        /// <returns>True if the level is complete, otherwise false.</returns>
        public bool LevelComplete(int fillCell)
        {
            return fillCell >= needCellToFill;
        }

        #endregion
    }
}