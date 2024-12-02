using SGS29.Utilities;
using UnityEngine;
using System;
using System.Collections.Generic;
using static Grower.LevelResult;

namespace Grower
{
    /// <summary>
    /// Controls movement on a grid with alignment to a starting offset.
    /// The class handles moving an object in grid-based space, taking into account obstacles and grid alignment.
    /// It provides events for movement start, stop, and direction changes.
    /// </summary>
    public class HeadMover : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Movement Settings")]
        [Tooltip("The speed at which the object moves.")]
        [Range(1f, 20f)]
        [SerializeField] private float moveSpeed = 5f;

        [SerializeField] private float objectMass = 1.0f;

        [Tooltip("The size of the grid cells.")]
        [SerializeField] private float gridSize = 1f;

        [Tooltip("The starting offset for grid alignment.")]
        [SerializeField] private Vector3 gridOffset = Vector3.zero;
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

        public List<Vector2Int> movementPathTracker { get; private set; } = new List<Vector2Int>();

        public float ObjectMass { get { return objectMass; } }

        public float movementStartTime { get; private set; }
        public float totalMovementTime { get; private set; }

        /// <summary>
        /// Поточна швидкість об'єкта.
        /// </summary>
        public float CurrentSpeed { get; private set; }

        /// <summary>
        /// Позиція в попередньому кадрі для обчислення швидкості.
        /// </summary>
        private Vector3 previousPosition;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            AlignToNearestGrid(); // Ensure alignment on startup
        }

        private void FixedUpdate()
        {
            if (IsMoving)
            {
                MoveToTarget();
                CalculateSpeed();
            }

            if (CanChangeDirection)
                ProcessInput();
        }

        private void OnEnable()
        {
            var headMover = GetComponent<HeadMover>();
            if (headMover != null)
            {
                headMover.OnLevelComplete += HandleLevelComplete;
            }
        }

        private void OnDisable()
        {
            var headMover = GetComponent<HeadMover>();
            if (headMover != null)
            {
                headMover.OnLevelComplete -= HandleLevelComplete;
            }
        }

        #endregion

        #region Movement Logic

        /// <summary>
        /// Processes player input to determine the movement direction.
        /// This method listens for WASD input and attempts to change the movement direction.
        /// </summary>
        private void ProcessInput()
        {
            if (Input.GetKey(KeyCode.W))
                TrySetDirection(Vector3.forward);
            else if (Input.GetKey(KeyCode.S))
                TrySetDirection(Vector3.back);
            else if (Input.GetKey(KeyCode.A))
                TrySetDirection(Vector3.left);
            else if (Input.GetKey(KeyCode.D))
                TrySetDirection(Vector3.right);
        }

        /// <summary>
        /// Attempts to set a new movement direction if possible.
        /// Ensures the target position is valid (not blocked by obstacles) and the direction can be changed.
        /// </summary>
        /// <param name="direction">The desired direction.</param>
        private void TrySetDirection(Vector3 direction)
        {
            if (!CanChangeDirection || direction == Vector3.zero || IsMoving)
                return;

            Vector3 potentialTarget = AlignToGrid(transform.position + direction);

            if (IsObstacleAt(potentialTarget))
                return;

            if (CurrentDirection != direction)
            {
                CurrentDirection = direction;
                OnDirectionChange?.Invoke(CurrentDirection); // Trigger direction change event
            }

            TargetPosition = potentialTarget;

            IsMoving = true;
            CanChangeDirection = false;

            // Фіксуємо час початку руху
            movementStartTime = Time.time;

            OnMoveStart?.Invoke(CurrentDirection); // Trigger movement start event
        }

        /// <summary>
        /// Moves the object towards the target position.
        /// </summary>
        private void MoveToTarget()
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, moveSpeed * Time.fixedDeltaTime);

            if (HasReachedTarget())
            {
                AlignToNearestGrid();
                Vector2Int currentGridCoord = ConvertToGridCoords(transform.position);

                // Додавання поточної позиції до трекера шляху, якщо це нова клітинка
                if (movementPathTracker.Count == 0 || movementPathTracker[^1] != currentGridCoord)
                {
                    movementPathTracker.Add(currentGridCoord);
                }

                Vector3 nextTarget = AlignToGrid(transform.position + CurrentDirection);

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

        private void CollisionEnter()
        {
            Vector3 nextTarget = AlignToGrid(transform.position + CurrentDirection);
            Vector2Int headCoordinates = ConvertToGridCoords(transform.position);
            Vector2Int objectCoordinates = ConvertToGridCoords(nextTarget);

            // Визначення сили удару
            float speedBeforeCollision = CurrentSpeed;
            float speedAfterCollision = 0;
            float collisionTime = Time.fixedDeltaTime;

            float collisionForce = CalculateCollisionForce(objectMass, speedBeforeCollision - speedAfterCollision, collisionTime);

            // Визначення сторони зіткнення
            CollisionSide side = DetermineCollisionSide(headCoordinates, objectCoordinates);

            // Отримання об'єкта клітинки, з яким сталося зіткнення
            Cell collidedObject = SM.Instance<Grid>().GetCell(objectCoordinates);

            // Створення даних про зіткнення
            CollisionData data = new CollisionData(
                headCoordinates,
                objectCoordinates,
                collisionForce,
                side,
                collidedObject
            );

            // Виклик події зіткнення
            GrowerEvents.OnHeadCollision?.Invoke(data);
        }


        private float CalculateCollisionForce(float mass, float deltaSpeed, float time)
        {
            // F = m * (deltaSpeed / time), де deltaSpeed - зміна швидкості
            return mass * (deltaSpeed / time);
        }

        private CollisionSide DetermineCollisionSide(Vector2Int headCoordinates, Vector2Int objectCoordinates)
        {
            Vector2Int delta = objectCoordinates - headCoordinates;

            if (delta == Vector2Int.up)    // Зіткнення з об'єкта зверху
                return CollisionSide.Top;
            else if (delta == Vector2Int.down) // Зіткнення з об'єкта знизу
                return CollisionSide.Bottom;
            else if (delta == Vector2Int.left) // Зіткнення з об'єкта зліва
                return CollisionSide.Left;
            else if (delta == Vector2Int.right) // Зіткнення з об'єкта справа
                return CollisionSide.Right;

            Debug.LogWarning($"Unexpected delta: {delta}. Returning default CollisionSide.");
            return CollisionSide.Top; // Значення за замовчуванням
        }


        /// <summary>
        /// Обчислює поточну швидкість об'єкта на основі пройденої відстані.
        /// </summary>
        private void CalculateSpeed()
        {
            float distanceMoved = Vector3.Distance(transform.position, previousPosition);
            CurrentSpeed = distanceMoved / Time.fixedDeltaTime;

            // Оновлення попередньої позиції
            previousPosition = transform.position;
        }

        /// <summary>
        /// Stops all movement and resets the current direction.
        /// </summary>
        private void StopMovement()
        {
            IsMoving = false;

            // Вимірювання загального часу руху
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
            Vector2Int gridCoord = ConvertToGridCoords(position);
            Cell cell = SM.Instance<Grid>().GetCell(gridCoord);

            return cell != null && (cell.CellType == CellType.Wall || cell.CellType == CellType.Body);
        }

        /// <summary>
        /// Aligns the object's position to the nearest grid point.
        /// </summary>
        private void AlignToNearestGrid()
        {
            transform.position = AlignToGrid(transform.position);
        }

        /// <summary>
        /// Aligns a given position to the grid with an offset.
        /// </summary>
        /// <param name="position">The position to align.</param>
        /// <returns>The aligned position.</returns>
        private Vector3 AlignToGrid(Vector3 position)
        {
            position += gridOffset;
            position.x = GridUtility.AlignAxis(position.x, gridSize);
            position.y = gridOffset.y; // Maintain vertical alignment
            position.z = GridUtility.AlignAxis(position.z, gridSize);
            return position;
        }

        /// <summary>
        /// Converts a world position to grid coordinates.
        /// </summary>
        /// <param name="position">The position to convert.</param>
        /// <returns>The grid coordinates.</returns>
        private Vector2Int ConvertToGridCoords(Vector3 position)
        {
            position += gridOffset;
            return new Vector2Int(GridUtility.AlignAxisAsInt(position.x, gridSize),
            GridUtility.AlignAxisAsInt(position.z, gridSize));
        }

        #endregion

        #region Utility Methods 

        private void HandleLevelComplete()
        {
            // Збір даних для LevelResult
            int sceneBuildIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            int levelIndex = 1; // Замініть на актуальний рівень або отримайте його з відповідного джерела
            Vector2Int lastCellCoord = ConvertToGridCoords(transform.position); // Остання позиція
            float passageTime = totalMovementTime; // Використовуємо сумарний час руху
            List<Vector2Int> movementPath = movementPathTracker; // Логіка збереження має бути додана

            // Створення результату рівня
            LevelResult result = new LevelResult(
                sceneBuildIndex,
                levelIndex,
                lastCellCoord,
                passageTime,
                movementPath.Count,
levelValidator
            );

            // Виклик події
            GrowerEvents.OnLevelEnd.Invoke(result);
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
                Vector3 potentialTarget = AlignToGrid(transform.position + direction);

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
}