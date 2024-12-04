using SGS29.Utilities;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Grower
{
    /// <summary>
    /// Controls movement on a grid with alignment to a starting offset.
    /// The class handles moving an object in grid-based space, taking into account obstacles and grid alignment.
    /// It provides events for movement start, stop, and direction changes.
    /// </summary>
    public class HeadMover : MonoBehaviour, IInputListener
    {
        #region Serialized Fields

        [Header("Movement Settings")]
        [Tooltip("The mass of the object.")]
        [SerializeField] private MovementStrategyBase movementStrategy;

        [SerializeField] private MoverSettings settings;
        /// <summary>
        /// A reference to the LevelValidator that checks the level's state and conditions.
        /// </summary>
        [Tooltip("The LevelValidator used to validate the level's conditions.")]
        [SerializeField] private LevelValidator levelValidator;

        #endregion

        #region Events

        /// <summary>
        /// Triggered when the character starts moving.
        /// </summary>
        public event Action<Vector3> OnMoveStart;

        /// <summary>
        /// Triggered when the character stops moving.
        /// </summary>
        public event Action OnMoveStop;

        /// <summary>
        /// Triggered when the character's direction changes.
        /// </summary>
        public event Action<Vector3> OnDirectionChange;

        /// <summary>
        /// Triggered when the level is completed (no more moves possible).
        /// </summary>
        public event Action OnLevelComplete;

        #endregion

        #region Fields

        /// <summary>
        /// The current movement direction of the object.
        /// </summary>
        public Vector3 CurrentDirection { get; private set; } = Vector3.zero;

        /// <summary>
        /// The target position the object is moving towards.
        /// </summary>
        public Vector3 TargetPosition { get; private set; } = Vector3.zero;

        /// <summary>
        /// A flag indicating whether the object is currently moving.
        /// </summary>
        public bool IsMoving { get; private set; } = false;

        /// <summary>
        /// A flag indicating whether the direction can be changed.
        /// </summary>
        public bool CanChangeDirection { get; private set; } = true;

        /// <summary>
        /// A list that tracks the movement path of the object.
        /// </summary>
        public List<Vector2Int> movementPathTracker { get; private set; } = new List<Vector2Int>();

        /// <summary>
        /// The mass of the object. Used for physics calculations.
        /// </summary>
        public MoverSettings Settings { get { return settings; } }

        /// <summary>
        /// The time when the movement started.
        /// </summary>
        public float movementStartTime { get; private set; }

        /// <summary>
        /// The total time taken for the movement to complete.
        /// </summary>
        public float totalMovementTime { get; private set; }

        /// <summary>
        /// The current speed of the object.
        /// </summary>
        public float CurrentSpeed { get; private set; }

        /// <summary>
        /// The position of the object in the previous frame. Used for calculating speed.
        /// </summary>
        private Vector3 previousPosition;

        private ResultBuilder resultBuilder;

        #endregion

        #region Unity Lifecycle

        /// <summary>
        /// Called when the script is first initialized. Aligns the object to the nearest grid on startup.
        /// </summary>
        private void Awake()
        {
            AlignToNearestGrid(); // Ensure alignment on startup
        }

        private void Start()
        {
            SM.Instance<InputManager>().AddListener(this);

            resultBuilder = new ResultBuilder(this, levelValidator);
        }

        /// <summary>
        /// Called on every fixed frame. Moves the object towards the target if it's moving, and calculates its speed.
        /// Also processes input if the direction can be changed.
        /// </summary>
        private void FixedUpdate()
        {
            if (IsMoving)
            {
                MoveToTarget();
                CalculateSpeed();
            }
        }

        /// <summary>
        /// Called when the object is enabled. Subscribes to the OnLevelComplete event of the HeadMover component.
        /// </summary>
        private void OnEnable()
        {
            var headMover = GetComponent<HeadMover>();
            if (headMover != null)
            {
                headMover.OnLevelComplete += HandleLevelComplete;
            }
        }

        /// <summary>
        /// Called when the object is disabled. Unsubscribes from the OnLevelComplete event of the HeadMover component.
        /// </summary>
        private void OnDisable()
        {
            var headMover = GetComponent<HeadMover>();
            if (headMover != null)
            {
                headMover.OnLevelComplete -= HandleLevelComplete;

                if (SM.HasSingleton<InputManager>())
                    SM.Instance<InputManager>().RemoveListener(this);
            }
        }

        #endregion

        #region Movement Logic

        public void OnSwipe(Vector2 direction)
        {
            if (CanChangeDirection)
                TrySetDirection(direction);
        }

        /// <summary>
        /// Публічний метод для спроби встановити новий напрямок.
        /// </summary>
        /// <param name="direction">Бажаний напрямок.</param>
        private void TrySetDirection(Vector3 direction)
        {
            if (!CanAttemptDirectionChange(direction))
                return;

            Vector3 potentialTarget = CalculateTargetPosition(direction);

            if (!IsValidTargetPosition(potentialTarget))
                return;

            UpdateDirectionAndStartMovement(direction, potentialTarget);
        }

        /// <summary>
        /// Перевіряє, чи можна спробувати змінити напрямок.
        /// </summary>
        /// <param name="direction">Бажаний напрямок.</param>
        /// <returns>True, якщо зміна напрямку можлива.</returns>
        private bool CanAttemptDirectionChange(Vector3 direction)
        {
            return CanChangeDirection && direction != Vector3.zero && !IsMoving;
        }

        /// <summary>
        /// Обчислює нову цільову позицію на основі бажаного напрямку.
        /// </summary>
        /// <param name="direction">Бажаний напрямок.</param>
        /// <returns>Вирівняна до сітки цільова позиція.</returns>
        private Vector3 CalculateTargetPosition(Vector3 direction)
        {
            if (direction.y != 0)
                direction = new Vector3(direction.x, 0, direction.y);

            return Utility.AlignToGrid(transform.position + direction, settings);
        }

        /// <summary>
        /// Перевіряє, чи є цільова позиція допустимою (без перешкод).
        /// </summary>
        /// <param name="targetPosition">Цільова позиція.</param>
        /// <returns>True, якщо позиція допустима.</returns>
        private bool IsValidTargetPosition(Vector3 targetPosition)
        {
            return !IsObstacleAt(targetPosition);
        }

        /// <summary>
        /// Оновлює поточний напрямок і починає рух до нової позиції.
        /// </summary>
        /// <param name="direction">Новий напрямок.</param>
        /// <param name="targetPosition">Цільова позиція.</param>
        private void UpdateDirectionAndStartMovement(Vector3 direction, Vector3 targetPosition)
        {
            if (CurrentDirection != direction)
            {
                CurrentDirection = direction;
                OnDirectionChange?.Invoke(CurrentDirection);
            }

            TargetPosition = targetPosition;

            IsMoving = true;
            CanChangeDirection = false;

            movementStartTime = Time.time;
            OnMoveStart?.Invoke(CurrentDirection);
        }

        /// <summary>
        /// Moves the object towards the target position.
        /// </summary>
        private void MoveToTarget()
        {
            movementStrategy.Move(transform, TargetPosition);

            if (HasReachedTarget())
            {
                AlignToNearestGrid();
                Vector2Int currentGridCoord = Utility.ConvertToGridCoords(transform.position, settings);

                // Add the current position to the movement path tracker if it's a new grid cell
                if (movementPathTracker.Count == 0 || movementPathTracker[^1] != currentGridCoord)
                {
                    movementPathTracker.Add(currentGridCoord);
                }

                Vector3 nextTarget = Utility.AlignToGrid(transform.position + CurrentDirection, settings);

                if (IsObstacleAt(nextTarget))
                {
                    CollisionEnter();
                    StopMovement();
                }
                else
                {
                    TargetPosition = nextTarget;
                }
            }
        }

        /// <summary>
        /// Handles the logic when a collision occurs.
        /// This method calculates the collision force, determines the collision side, and triggers the collision event.
        /// </summary>
        private void CollisionEnter()
        {
            Vector3 nextTarget = Utility.AlignToGrid(transform.position + CurrentDirection, settings);
            Vector2Int headCoordinates = Utility.ConvertToGridCoords(transform.position, settings);
            Vector2Int objectCoordinates = Utility.ConvertToGridCoords(nextTarget, settings);

            // Calculate collision force
            float speedBeforeCollision = CurrentSpeed;
            float speedAfterCollision = 0;
            float collisionTime = Time.fixedDeltaTime;

            float collisionForce = CalculateCollisionForce(settings.objectMass, speedBeforeCollision - speedAfterCollision, collisionTime);

            // Determine the collision side
            CollisionSide side = DetermineCollisionSide(headCoordinates, objectCoordinates);

            // Get the collided object cell
            Cell collidedObject = SM.Instance<Grid>().GetCell(objectCoordinates);

            // Create collision data
            CollisionData data = new CollisionData(
                headCoordinates,
                objectCoordinates,
                collisionForce,
                side,
                collidedObject
            );

            // Trigger collision event
            GrowerEvents.OnHeadCollision?.Invoke(data);
        }

        /// <summary>
        /// Calculates the collision force based on the mass, change in speed, and time.
        /// </summary>
        /// <param name="mass">The mass of the object.</param>
        /// <param name="deltaSpeed">The change in speed during the collision.</param>
        /// <param name="time">The time over which the collision occurred.</param>
        /// <returns>The calculated collision force.</returns>
        private float CalculateCollisionForce(float mass, float deltaSpeed, float time)
        {
            // F = m * (deltaSpeed / time), where deltaSpeed is the change in speed
            return mass * (deltaSpeed / time);
        }

        /// <summary>
        /// Determines the side of the collision based on the relative positions of the object and the head.
        /// </summary>
        /// <param name="headCoordinates">The coordinates of the head.</param>
        /// <param name="objectCoordinates">The coordinates of the collided object.</param>
        /// <returns>The side of the collision.</returns>
        private CollisionSide DetermineCollisionSide(Vector2Int headCoordinates, Vector2Int objectCoordinates)
        {
            Vector2Int delta = objectCoordinates - headCoordinates;

            if (delta == Vector2Int.up)    // Collision from the top
                return CollisionSide.Top;
            else if (delta == Vector2Int.down) // Collision from the bottom
                return CollisionSide.Bottom;
            else if (delta == Vector2Int.left) // Collision from the left
                return CollisionSide.Left;
            else if (delta == Vector2Int.right) // Collision from the right
                return CollisionSide.Right;

            Debug.LogWarning($"Unexpected delta: {delta}. Returning default CollisionSide.");
            return CollisionSide.Top; // Default value
        }

        /// <summary>
        /// Calculates the current speed of the object based on the distance traveled.
        /// </summary>
        private void CalculateSpeed()
        {
            float distanceMoved = Vector3.Distance(transform.position, previousPosition);
            CurrentSpeed = distanceMoved / Time.fixedDeltaTime;

            // Update the previous position
            previousPosition = transform.position;
        }

        /// <summary>
        /// Stops all movement and resets the current direction.
        /// </summary>
        private void StopMovement()
        {
            IsMoving = false;

            // Record the total movement time
            totalMovementTime += Time.time - movementStartTime;

            CurrentDirection = Vector3.zero;

            AlignToNearestGrid();
            AllowDirectionChange();

            OnMoveStop?.Invoke(); // Trigger movement stop event

            // Check if the level is complete
            if (!HasValidMoves())
            {
                OnLevelComplete?.Invoke(); // Trigger level complete event
            }
        }

        #endregion

        #region Grid Alignment and Collision

        /// <summary>
        /// Checks if the given position is blocked by an obstacle.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns>True if an obstacle exists at the position, false otherwise.</returns>
        private bool IsObstacleAt(Vector3 position)
        {
            Vector2Int gridCoord = Utility.ConvertToGridCoords(position, settings);
            Cell cell = SM.Instance<Grid>().GetCell(gridCoord);

            return cell != null && (cell.CellType == CellType.Wall || cell.CellType == CellType.Body);
        }

        /// <summary>
        /// Aligns the object's position to the nearest grid point.
        /// </summary>
        private void AlignToNearestGrid()
        {
            transform.position = Utility.AlignToGrid(transform.position, settings);
        }

        #endregion

        #region Utility Methods 

        /// <summary>
        /// Handles the completion of the level by collecting relevant data and triggering the level end event.
        /// This method is responsible for gathering the scene information, movement data, and level result,
        /// then invoking the event to notify other parts of the game that the level is complete.
        /// </summary>
        private void HandleLevelComplete()
        {
            // Trigger the level end event with the result data
            GrowerEvents.OnLevelEnd?.Invoke(resultBuilder.LevelResultBuild());
        }

        /// <summary>
        /// Checks if there are any valid directions left for movement.
        /// </summary>
        /// <returns>True if there are valid directions, false otherwise.</returns>
        private bool HasValidMoves()
        {
            Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

            foreach (var direction in directions)
            {
                Vector3 potentialTarget = Utility.AlignToGrid(transform.position + direction, settings);

                if (!IsObstacleAt(potentialTarget))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the object has reached the target position.
        /// </summary>
        /// <returns>True if the object is at the target position, false otherwise.</returns>
        private bool HasReachedTarget()
        {
            return Vector3.Distance(transform.position, TargetPosition) <= 0.01f;
        }

        /// <summary>
        /// Allows direction changes after movement stops.
        /// </summary>
        private void AllowDirectionChange()
        {
            CanChangeDirection = true;
        }

        #endregion
    }

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