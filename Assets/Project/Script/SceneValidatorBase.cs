using SGS29.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Grower
{
    public abstract class SceneValidatorBase : ScriptableObject
    {
        /// <summary>
        /// Визначає наступну сцену на основі результату рівня.
        /// </summary>
        /// <param name="levelResult">Результат рівня.</param>
        /// <returns>Наступна сцена для завантаження.</returns>
        public abstract int GetNextSceneBuildIndex(LevelResult levelResult);
    }
}