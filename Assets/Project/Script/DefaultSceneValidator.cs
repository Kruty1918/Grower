using UnityEngine;
using UnityEngine.SceneManagement;

namespace Grower
{
    [CreateAssetMenu(menuName = "Grower/Validator/Scene/Default")]
    public class DefaultSceneValidator : SceneValidatorBase
    {
        [SerializeField] private int defaultNextSceneIndex = 1; // Індекс сцени за замовчуванням

        /// <summary>
        /// Визначає наступну сцену для завантаження або перезавантаження.
        /// </summary>
        /// <param name="levelResult">Результат рівня.</param>
        /// <returns>Індекс наступної сцени.</returns>
        public override int GetNextSceneBuildIndex(LevelResult levelResult)
        {
            if (levelResult.LevelComplete)
            {
                return (levelResult.SceneBuildIndex + 1) % SceneManager.sceneCountInBuildSettings;
            }
            else
            {
                return levelResult.SceneBuildIndex; // Перезавантажуємо поточну сцену
            }
        }
    }
}