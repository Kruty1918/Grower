using UnityEngine;
using System.Collections.Generic;

namespace Grower
{
    /// <summary>
    /// A class that builds the result data for a level.
    /// It collects relevant information such as player movement, level index, and time spent.
    /// </summary>
    public class ResultBuilder
    {
        private readonly HeadMover headMover;  // The component responsible for moving the player
        private readonly LevelValidator levelValidator;  // The validator used to check the level's completion status

        /// <summary>
        /// Initializes a new instance of the ResultBuilder class.
        /// </summary>
        /// <param name="headMover">The HeadMover component for player movement tracking.</param>
        /// <param name="levelValidator">The LevelValidator used for validating level progress.</param>
        public ResultBuilder(HeadMover headMover, LevelValidator levelValidator)
        {
            this.headMover = headMover;
            this.levelValidator = levelValidator;
        }

        /// <summary>
        /// Builds the LevelResult containing information about the player's performance in the level.
        /// </summary>
        /// <returns>A LevelResult object containing the collected data.</returns>
        public LevelResult LevelResultBuild()
        {
            // Collecting data for LevelResult
            int sceneBuildIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;  // Current scene index
            int levelIndex = 1; // Replace this with the actual level index or retrieve it from the appropriate source
            Vector2Int lastCellCoord = Utility.ConvertToGridCoords(headMover.transform.position, headMover.Settings);  // The last position of the player on the grid
            float passageTime = headMover.totalMovementTime;  // The total movement time spent in the level
            List<Vector2Int> movementPath = new List<Vector2Int>(headMover.movementPathTracker);  // Copy the movement path

            // Create the level result object
            LevelResult result = new LevelResult(
                sceneBuildIndex,
                levelIndex,
                lastCellCoord,
                passageTime,
                movementPath.Count,  // Number of movements
                levelValidator // This needs to be defined or replaced with the actual validation logic
            );

            return result;
        }
    }
}