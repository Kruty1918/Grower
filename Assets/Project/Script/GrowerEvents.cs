using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Grower
{
    public static class GrowerEvents
    {
        public static UnityEvent<LevelResult> OnLevelEnd = new UnityEvent<LevelResult>();
    }

    /// <summary>
    /// Клас, що зберігає результати завершення рівня гри.
    /// Включає дані про рівень, час проходження, координати останньої клітинки і шлях гравця.
    /// </summary>
    public class LevelResult
    {
        /// <summary>
        /// Індекс сцени в білд списку.
        /// Використовується для визначення, яка сцена була активною під час проходження рівня.
        /// </summary>
        public readonly int SceneBuildIndex;

        /// <summary>
        /// Індекс рівня в грі (наприклад, номер рівня).
        /// Визначає, який саме рівень було пройдено.
        /// </summary>
        public readonly int LevelIndex;

        /// <summary>
        /// Координати останньої клітинки, на якій гравець завершив рівень.
        /// Це допомагає визначити, де саме гравець завершив свою подорож на карті.
        /// </summary>
        public readonly Vector2Int LastCellCoord;

        /// <summary>
        /// Час, за який гравець пройшов рівень.
        /// Це може бути використано для аналізу продуктивності або виведення статистики.
        /// </summary>
        public readonly float PassageTime;

        /// <summary>
        /// Шлях, яким пройшов гравець від початку рівня до його завершення.
        /// Список координат клітинок, що вказує на рух гравця по рівню.
        /// </summary>
        public readonly List<Vector2Int> MovementPath;

        /// <summary>
        /// Конструктор для ініціалізації результатів завершення рівня.
        /// </summary>
        /// <param name="sceneBuildIndex">Індекс сцени в білд списку.</param>
        /// <param name="levelIndex">Індекс рівня.</param>
        /// <param name="lastCellCoord">Координати останньої клітинки.</param>
        /// <param name="passageTime">Час проходження рівня.</param>
        /// <param name="movementPath">Шлях, пройдений гравцем.</param>
        public LevelResult(int sceneBuildIndex, int levelIndex, Vector2Int lastCellCoord, float passageTime, List<Vector2Int> movementPath)
        {
            SceneBuildIndex = sceneBuildIndex;
            LevelIndex = levelIndex;
            LastCellCoord = lastCellCoord;
            PassageTime = passageTime;
            MovementPath = movementPath ?? new List<Vector2Int>();
        }
    }
}