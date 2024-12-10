using UnityEngine;
using System.Collections.Generic;

namespace Grower
{
    public class ResultBuilder
    {
        private readonly HeadMover headMover;
        private readonly LevelValidator levelValidator;

        public ResultBuilder(HeadMover headMover, LevelValidator levelValidator)
        {
            this.headMover = headMover;
            this.levelValidator = levelValidator;
        }

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