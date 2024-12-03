using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Class that stores the results of completing a level in the game.
    /// This class contains data about the scene, level, coordinates of the last cell,
    /// the passage time, and level completion status.
    /// </summary>
    [System.Serializable]
    public class LevelResult
    {
        /// <summary>
        /// The scene's build index in the game.
        /// </summary>
        [SerializeField] public int SceneBuildIndex;

        /// <summary>
        /// The index of the level.
        /// </summary>
        [SerializeField] public int LevelIndex;

        /// <summary>
        /// The coordinates of the last cell in the game.
        /// </summary>
        [SerializeField] public Vector2Int LastCellCoord;

        /// <summary>
        /// The time taken to complete the level.
        /// </summary>
        [SerializeField] public float PassageTime;

        /// <summary>
        /// Whether the level was successfully completed.
        /// </summary>
        [SerializeField] public bool LevelComplete;

        /// <summary>
        /// The level validator object used to check level completion.
        /// </summary>
        public LevelValidator levelValidator;

        /// <summary>
        /// Constructor to initialize the level result.
        /// </summary>
        /// <param name="sceneBuildIndex">The scene's build index in the game.</param>
        /// <param name="levelIndex">The index of the level.</param>
        /// <param name="lastCellCoord">The coordinates of the last cell in the game.</param>
        /// <param name="passageTime">The time taken to complete the level.</param>
        /// <param name="fillCell">The number of filled cells.</param>
        /// <param name="levelValidator">The level validator object used to check level completion.</param>
        public LevelResult(int sceneBuildIndex, int levelIndex, Vector2Int lastCellCoord, float passageTime, int fillCell, LevelValidator levelValidator)
        {
            SceneBuildIndex = sceneBuildIndex;
            LevelIndex = levelIndex;
            LastCellCoord = lastCellCoord;
            PassageTime = passageTime;

            // Calls the method to check if the level is complete
            LevelComplete = levelValidator.LevelComplete(fillCell);

            Debug.Log($"Level Complete: {levelValidator.LevelComplete(fillCell)}");
        }
    }
}