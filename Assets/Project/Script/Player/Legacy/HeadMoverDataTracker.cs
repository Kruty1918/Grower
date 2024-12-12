using Sirenix.OdinInspector;
using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Tracks and displays data from the <see cref="HeadMover"/> component.
    /// Subscribes to game events and updates tracked data for debugging purposes.
    /// </summary>
    public class HeadMoverDataTracker : MonoBehaviour
    {
        /// <summary>
        /// Reference to the <see cref="HeadMover"/> component.
        /// </summary>
        private HeadMover headMover;

        /// <summary>
        /// Subscribes to game events when the object is enabled.
        /// </summary>
        private void OnEnable()
        {
            GrowerEvents.OnLevelEnd.AddListener(OnLevelEnd);
            GrowerEvents.OnHeadCollision.AddListener(OnHeadCollision);
        }

        /// <summary>
        /// Unsubscribes from game events when the object is disabled.
        /// </summary>
        private void OnDisable()
        {
            GrowerEvents.OnLevelEnd.RemoveListener(OnLevelEnd);
            GrowerEvents.OnHeadCollision.RemoveListener(OnHeadCollision);
        }

        #region Movement Data Fields

        /// <summary>
        /// The current direction of the object’s movement, represented as a string.
        /// </summary>
        [FoldoutGroup("Head Mover Data", true)]
        [Title("Movement Data")]
        [Space(10)]
        [ReadOnly]
        [Tooltip("The current direction of movement.")]
        public string currentDirection;

        /// <summary>
        /// The current position of the object in world space.
        /// </summary>
        [FoldoutGroup("Head Mover Data")]
        [ReadOnly]
        [Tooltip("The current position of the object.")]
        public Vector3 currentPosition;

        /// <summary>
        /// The target position that the object is moving toward.
        /// </summary>
        [FoldoutGroup("Head Mover Data")]
        [ReadOnly]
        [Tooltip("The target position of the object.")]
        public Vector3 targetPosition;

        /// <summary>
        /// The current speed of the object’s movement.
        /// </summary>
        [FoldoutGroup("Head Mover Data")]
        [ReadOnly]
        [Tooltip("The current speed of movement.")]
        public float currentSpeed;

        /// <summary>
        /// Indicates whether the object is currently in motion.
        /// </summary>
        [FoldoutGroup("Head Mover Data")]
        [ReadOnly]
        [Tooltip("Indicates whether the object is currently moving.")]
        public bool isMoving;

        /// <summary>
        /// Indicates whether the object can currently change its direction of movement.
        /// </summary>
        [FoldoutGroup("Head Mover Data")]
        [ReadOnly]
        [Tooltip("Indicates whether the object can change direction.")]
        public bool canChangeDirection;

        /// <summary>
        /// The time at which the current movement began.
        /// </summary>
        [FoldoutGroup("Head Mover Data")]
        [ReadOnly]
        [Tooltip("The timestamp when the current movement started.")]
        public float movementStartTime;

        /// <summary>
        /// The total duration of movement time.
        /// </summary>
        [FoldoutGroup("Head Mover Data")]
        [ReadOnly]
        [Tooltip("The total time spent moving.")]
        public float totalMovementTime;

        /// <summary>
        /// A string representation of the movement path, listing key waypoints.
        /// </summary>
        [FoldoutGroup("Head Mover Data")]
        [ShowInInspector]
        [ReadOnly]
        [Tooltip("Tracks the path of the movement as a series of coordinates.")]
        public string movementPathTracker;

        #endregion

        #region Event Data Fields

        /// <summary>
        /// Stores the result of the most recent level completion event.
        /// </summary>
        [FoldoutGroup("Event Data", true)]
        [ShowInInspector]
        [ReadOnly]
        [Tooltip("Stores the result of the level completion event.")]
        public LevelResult levelEndResult;

        /// <summary>
        /// Stores details about the most recent collision event involving the object’s head.
        /// </summary>
        [FoldoutGroup("Event Data")]
        [ShowInInspector]
        [ReadOnly]
        [Tooltip("Details of the last head collision event.")]
        public CollisionData headCollisionDetails;

        #endregion

        /// <summary>
        /// Initializes the <see cref="HeadMover"/> reference.
        /// </summary>
        private void Start()
        {
            headMover = GetComponent<HeadMover>();
            if (headMover == null)
            {
                Debug.LogError("HeadMover component not found on this object.");
            }
        }

        /// <summary>
        /// Updates tracked data from the <see cref="HeadMover"/> component each frame.
        /// </summary>
        private void Update()
        {
            if (headMover != null)
            {
                currentDirection = headMover.CurrentDirection.ToString();
                currentPosition = headMover.transform.position;
                targetPosition = headMover.TargetPosition;
                currentSpeed = headMover.CurrentSpeed;
                isMoving = headMover.IsMoving;
                canChangeDirection = headMover.CanChangeDirection;
                movementStartTime = headMover.movementStartTime;
                totalMovementTime = headMover.totalMovementTime;
                movementPathTracker = string.Join(", ", headMover.movementPathTracker);
            }
        }

        /// <summary>
        /// Handles the level end event and updates the result.
        /// </summary>
        /// <param name="result">The result of the level completion.</param>
        private void OnLevelEnd(LevelResult result)
        {
            levelEndResult = result;
        }

        /// <summary>
        /// Handles the head collision event and updates collision details.
        /// </summary>
        /// <param name="collisionData">Data about the head collision.</param>
        private void OnHeadCollision(CollisionData collisionData)
        {
            headCollisionDetails = collisionData;
        }
    }
}