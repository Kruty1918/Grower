using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Grower
{
        /// <summary>
        /// Manages the creation and behavior of the snake's body segments.
        /// </summary>
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
                        // Initialize the body animation system
                        bodyAnimation = new BodyAnimation();

                        // Set the initial spawn position based on the snake's head
                        lastSpawnPosition = snakeHead.position;
                        Vector3 alignedStartPos = GridUtility.AlignToGrid(lastSpawnPosition, gridSize, spawnCellOffset);
                        bodyPositions.Add(alignedStartPos);
                }

                private void FixedUpdate()
                {
                        // Get the current position of the snake's head
                        Vector3 currentHeadPosition = snakeHead.position;

                        // Check if the head has moved at least the minimum threshold distance
                        if (Vector3.Distance(currentHeadPosition, lastSpawnPosition) >= minSpawnThreshold)
                        {
                                // Align the last spawn position with the grid
                                Vector3 spawnPosition = GridUtility.AlignToGrid(lastSpawnPosition, gridSize, spawnCellOffset);

                                // Insert the new spawn position at the beginning of the body positions list
                                bodyPositions.Insert(0, spawnPosition);

                                // Animate the creation of the body segment at the spawn position
                                bodyAnimation.AnimateBodySegmentSpawn(spawnPosition, bodyPrefab, gridSize);

                                // Update the last spawn position to the current head position
                                lastSpawnPosition = currentHeadPosition;

                                // Remove excess body segments if the list exceeds the max body length
                                if (bodyPositions.Count > maxBodyLength)
                                {
                                        bodyPositions.RemoveAt(bodyPositions.Count - 1);
                                }
                        }
                }
        }
}