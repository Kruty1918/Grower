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
                #region Serialized Fields

                /// <summary>
                /// Reference to the snake's head transform.
                /// </summary>
#if ODIN_INSPECTOR
                [TitleGroup("References", "Objects required for BodyBuilder operation", TitleAlignments.Centered)]
                [LabelText("Snake Head"), Required, Tooltip("Reference to the snake's head")]
#else
        [Header("References")]
        [Tooltip("Reference to the snake's head")]
#endif
                [SerializeField] private Transform snakeHead;

                /// <summary>
                /// Prefab for the snake body segments.
                /// </summary>
#if ODIN_INSPECTOR
                [LabelText("Body Prefab"), Required, Tooltip("Prefab for the snake body segments")]
#endif
                [SerializeField] private Cell bodyPrefab;

                /// <summary>
                /// Size of the grid for alignment.
                /// </summary>
#if ODIN_INSPECTOR
                [TitleGroup("Settings", "Spawn and body behavior settings", TitleAlignments.Centered)]
                [BoxGroup("Settings/Grid Settings")]
                [LabelText("Grid Size"), MinValue(0.1f), Tooltip("Grid size for alignment")]
#else
        [Header("Settings")]
        [Tooltip("Grid size for alignment")]
        [Min(0.1f)]
#endif
                [SerializeField] private float gridSize = 1f;

                /// <summary>
                /// Offset applied during body segment spawning.
                /// </summary>
#if ODIN_INSPECTOR
                [BoxGroup("Settings/Grid Settings")]
                [LabelText("Spawn Cell Offset"), Tooltip("Offset applied during body segment spawning")]
#endif
                [SerializeField] private Vector3 spawnCellOffset;

                /// <summary>
                /// Maximum length of the snake's body.
                /// </summary>
#if ODIN_INSPECTOR
                [BoxGroup("Settings/Body Settings")]
                [LabelText("Max Body Length"), MinValue(1), Tooltip("Maximum length of the snake's body")]
#else
        [Min(1), Tooltip("Maximum length of the snake's body")]
#endif
                [SerializeField] private int maxBodyLength = 10;

                /// <summary>
                /// Minimum movement threshold to spawn a new body segment.
                /// </summary>
#if ODIN_INSPECTOR
                [BoxGroup("Settings/Body Settings")]
                [LabelText("Min Spawn Threshold"), MinValue(0.01f), Tooltip("Minimum movement threshold to spawn a new body segment")]
#endif
                [SerializeField] private float minSpawnThreshold = 0.1f;

                #endregion

                #region Runtime Data

                /// <summary>
                /// Last position where a body segment was spawned.
                /// </summary>
                [ShowInInspector, ReadOnly, BoxGroup("Runtime Data"), Tooltip("Last position where a body segment was spawned")]
                private Vector3 lastSpawnPosition;

                /// <summary>
                /// List of current body segment positions.
                /// </summary>
                [ShowInInspector, ReadOnly, BoxGroup("Runtime Data"), Tooltip("List of current body segment positions")]
                private List<Vector3> bodyPositions = new List<Vector3>();

                #endregion

                /// <summary>
                /// Interface for handling body animation.
                /// </summary>
                private IBodyAnimation bodyAnimation;

                /// <summary>
                /// Initializes body animation and sets up initial spawn position.
                /// </summary>
                private void Awake()
                {
                        // Initialize the body animation system
                        bodyAnimation = new BodyAnimation();

                        // Set the initial spawn position based on the snake's head
                        lastSpawnPosition = snakeHead.position;
                        Vector3 alignedStartPos = GridUtility.AlignToGrid(lastSpawnPosition, gridSize, spawnCellOffset);
                        bodyPositions.Add(alignedStartPos);
                }

                /// <summary>
                /// Updates the body builder logic at fixed intervals.
                /// </summary>
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