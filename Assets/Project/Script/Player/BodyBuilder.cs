using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Grower
{
    public class BodyBuilder : MonoBehaviour
    {
#if ODIN_INSPECTOR
        [TitleGroup("References", "Об'єкти, які необхідні для роботи BodyBuilder", TitleAlignments.Centered)]
        [LabelText("Snake Head"), Required, Tooltip("Посилання на голову змії")]
#else
        [Header("References")]
        [Tooltip("Посилання на голову змії")]
#endif
        [SerializeField] private Transform snakeHead;

#if ODIN_INSPECTOR
        [LabelText("Body Prefab"), Required, Tooltip("Префаб сегменту тіла змії")]
#endif
        [SerializeField] private Cell bodyPrefab;

#if ODIN_INSPECTOR
        [TitleGroup("Settings", "Налаштування спавну та поведінки тіла", TitleAlignments.Centered)]
        [BoxGroup("Settings/Grid Settings")]
        [LabelText("Grid Size"), MinValue(0.1f), Tooltip("Розмір сітки для вирівнювання")]
#else
        [Header("Settings")]
        [Tooltip("Розмір сітки для вирівнювання")]
        [Min(0.1f)]
#endif
        [SerializeField] private float gridSize = 1f;

#if ODIN_INSPECTOR
        [BoxGroup("Settings/Grid Settings")]
        [LabelText("Spawn Cell Offset"), Tooltip("Зміщення під час спавну сегмента")]
#endif
        [SerializeField] private Vector3 spawnCellOffset;

#if ODIN_INSPECTOR
        [BoxGroup("Settings/Body Settings")]
        [LabelText("Max Body Length"), MinValue(1), Tooltip("Максимальна довжина тіла")]
#else
        [Min(1), Tooltip("Максимальна довжина тіла")]
#endif
        [SerializeField] private int maxBodyLength = 10;

#if ODIN_INSPECTOR
        [BoxGroup("Settings/Body Settings")]
        [LabelText("Min Spawn Threshold"), MinValue(0.01f), Tooltip("Мінімальний рух для створення нового сегмента")]
#endif
        [SerializeField] private float minSpawnThreshold = 0.1f;

        [ShowInInspector, ReadOnly, BoxGroup("Runtime Data"), Tooltip("Остання позиція, де був створений сегмент")]
        private Vector3 lastSpawnPosition;

        [ShowInInspector, ReadOnly, BoxGroup("Runtime Data"), Tooltip("Список поточних позицій тіла")]
        private List<Vector3> bodyPositions = new List<Vector3>();

        private IBodyAnimation bodyAnimation;

        private void Awake()
        {
            bodyAnimation = new BodyAnimation();

            // Ініціалізація першої позиції для спавну
            lastSpawnPosition = snakeHead.position;
            Vector3 alignedStartPos = GridUtility.AlignToGrid(lastSpawnPosition, gridSize, spawnCellOffset);
            bodyPositions.Add(alignedStartPos);
        }

        private void FixedUpdate()
        {
            Vector3 currentHeadPosition = snakeHead.position;

            // Перевірка, чи голова змістилася хоча б на мінімальну відстань
            if (Vector3.Distance(currentHeadPosition, lastSpawnPosition) >= minSpawnThreshold)
            {
                // Вирівнюємо останню позицію перед спавном за сіткою
                Vector3 spawnPosition = GridUtility.AlignToGrid(lastSpawnPosition, gridSize, spawnCellOffset);

                bodyPositions.Insert(0, spawnPosition);
                bodyAnimation.AnimateBodySegmentSpawn(spawnPosition, bodyPrefab, gridSize);

                // Оновлюємо останню зафіксовану позицію
                lastSpawnPosition = currentHeadPosition;

                // Видаляємо зайві сегменти, якщо їх більше, ніж дозволено
                if (bodyPositions.Count > maxBodyLength)
                {
                    bodyPositions.RemoveAt(bodyPositions.Count - 1);
                }
            }
        }
    }
}
