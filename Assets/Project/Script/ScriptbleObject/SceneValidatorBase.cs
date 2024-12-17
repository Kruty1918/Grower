using SGS29.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Grower
{
    /// <summary>
    /// Abstract base class for scene validators that determine the next scene based on the level result.
    /// </summary>
    public abstract class SceneValidatorBase : ScriptableObject
    {
        /// <summary>
        /// Determines the next scene based on the level result.
        /// </summary>
        /// <param name="levelResult">The result of the level.</param>
        /// <returns>The build index of the next scene to load.</returns>
        public abstract int GetNextSceneBuildIndex(LevelResult levelResult);
    }
}