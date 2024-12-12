using SGS29.Utilities;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Grower
{
    public abstract class MoverBase : MonoBehaviour
    {
        #region Events

        public event Action<Vector3> OnMoveStart;
        public event Action OnMoveStop;
        public event Action<Vector3> OnDirectionChange;
        public event Action OnLevelComplete;

        #endregion

        #region Fields

        public Vector3 CurrentDirection { get; protected set; } = Vector3.zero;
        public Vector3 TargetPosition { get; protected set; } = Vector3.zero;
        public bool IsMoving { get; protected set; } = false;
        public bool CanChangeDirection { get; protected set; } = true;

        protected Vector3 previousPosition;
        protected float movementStartTime;
        protected float totalMovementTime;

        #endregion

        #region Unity Lifecycle

        protected virtual void Start()
        {
            previousPosition = transform.position;
        }

        protected virtual void FixedUpdate()
        {
            if (IsMoving)
            {
                MoveToTarget();
                CalculateSpeed();
            }
        }

        #endregion

        #region Movement Logic

        protected abstract void MoveToTarget();

        protected void CalculateSpeed()
        {
            float distanceMoved = Vector3.Distance(transform.position, previousPosition);
            float currentSpeed = distanceMoved / Time.fixedDeltaTime;
            previousPosition = transform.position;
        }

        protected void StopMovement()
        {
            IsMoving = false;
            totalMovementTime += Time.time - movementStartTime;
            ResetDirection();
            OnMoveStop?.Invoke();
        }

        protected void ResetDirection()
        {
            CurrentDirection = Vector3.zero;
        }

        protected void UpdateDirectionAndStartMovement(Vector3 direction, Vector3 targetPosition)
        {
            SetNewDirection(direction);
            SetMovementTarget(targetPosition);
        }

        protected void SetNewDirection(Vector3 direction)
        {
            if (CurrentDirection != direction)
            {
                CurrentDirection = direction;
                OnDirectionChange?.Invoke(CurrentDirection);
            }
        }

        protected void SetMovementTarget(Vector3 targetPosition)
        {
            TargetPosition = targetPosition;
            IsMoving = true;
            CanChangeDirection = false;
            movementStartTime = Time.time;
            OnMoveStart?.Invoke(CurrentDirection);
        }

        #endregion

        #region Grid Alignment

        protected void AlignToNearestGrid()
        {
            transform.position = Utility.AlignToGrid(transform.position, null); // Or use specific settings if needed
        }

        #endregion
    }

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

        public bool CanMove { get; private set; } = false;

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
            if (IsMoving && CanMove)
            {
                MoveToTarget();
                CalculateSpeed();
            }
        }

        private void CalculateSpeed()
        {
            float distanceMoved = Vector3.Distance(transform.position, previousPosition);
            CurrentSpeed = distanceMoved / Time.fixedDeltaTime;

            previousPosition = transform.position;
        }


        /// <summary>
        /// Called when the object is enabled. Subscribes to the OnLevelComplete event of the HeadMover component.
        /// </summary>
        private void OnEnable()
        {
            OnLevelComplete += HandleLevelComplete;
            GrowerEvents.OnGameStateChange.AddListener(OnGameStateChange);
        }

        /// <summary>
        /// Called when the object is disabled. Unsubscribes from the OnLevelComplete event of the HeadMover component.
        /// </summary>
        private void OnDisable()
        {
            OnLevelComplete -= HandleLevelComplete;
            GrowerEvents.OnGameStateChange.RemoveListener(OnGameStateChange);

            if (SM.HasSingleton<InputManager>())
                SM.Instance<InputManager>().RemoveListener(this);
        }

        #endregion

        #region Swipe Handling

        public void OnSwipe(Vector2 direction)
        {
            if (!CanMove) return;

            direction = GetPrimaryDirection(direction);
            Vector3 nDirection = new Vector3(direction.x, 0, direction.y);

            if (CanChangeDirection)
                TrySetDirection(nDirection);
        }

        private Vector2 GetPrimaryDirection(Vector2 direction)
        {
            return Mathf.Abs(direction.x) > Mathf.Abs(direction.y)
                ? new Vector2(direction.x, 0)
                : new Vector2(0, direction.y);
        }

        #endregion

        #region Direction Setting

        private void TrySetDirection(Vector3 direction)
        {
            if (!CanAttemptDirectionChange(direction))
                return;

            Vector3 potentialTarget = CalculateTargetPosition(direction);

            if (IsValidTargetPosition(potentialTarget))
                UpdateDirectionAndStartMovement(direction, potentialTarget);
        }

        private bool CanAttemptDirectionChange(Vector3 direction)
        {
            return CanChangeDirection && direction != Vector3.zero && !IsMoving;
        }

        private Vector3 CalculateTargetPosition(Vector3 direction)
        {
            return Utility.AlignToGrid(transform.position + direction, settings);
        }

        private bool IsValidTargetPosition(Vector3 targetPosition)
        {
            return !IsObstacleAt(targetPosition);
        }

        private void UpdateDirectionAndStartMovement(Vector3 direction, Vector3 targetPosition)
        {
            SetNewDirection(direction);
            SetMovementTarget(targetPosition);
        }

        private void SetNewDirection(Vector3 direction)
        {
            if (CurrentDirection != direction)
            {
                CurrentDirection = direction;
                OnDirectionChange?.Invoke(CurrentDirection);
            }
        }

        private void SetMovementTarget(Vector3 targetPosition)
        {
            TargetPosition = targetPosition;
            IsMoving = true;
            CanChangeDirection = false;
            movementStartTime = Time.time;
            OnMoveStart?.Invoke(CurrentDirection);
        }

        #endregion

        #region Movement Logic

        private void MoveToTarget()
        {
            movementStrategy.Move(transform, TargetPosition);

            if (HasReachedTarget())
                HandleTargetReached();
        }

        private void HandleTargetReached()
        {
            AlignToNearestGrid();
            TrackMovementPath();

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

        private void TrackMovementPath()
        {
            Vector2Int currentGridCoord = Utility.ConvertToGridCoords(transform.position, settings);

            if (movementPathTracker.Count == 0 || movementPathTracker[^1] != currentGridCoord)
            {
                movementPathTracker.Add(currentGridCoord);
            }
        }

        #endregion

        #region Collision Handling

        private void CollisionEnter()
        {
            CollisionData data = PrepareCollisionData();
            GrowerEvents.OnHeadCollision?.Invoke(data);
        }

        private CollisionData PrepareCollisionData()
        {
            Vector3 nextTarget = Utility.AlignToGrid(transform.position + CurrentDirection, settings);
            Vector2Int headCoordinates = Utility.ConvertToGridCoords(transform.position, settings);
            Vector2Int objectCoordinates = Utility.ConvertToGridCoords(nextTarget, settings);

            float collisionForce = CalculateCollisionForce(settings.objectMass, CurrentSpeed, Time.fixedDeltaTime);
            CollisionSide side = DetermineCollisionSide(headCoordinates, objectCoordinates);
            Cell collidedObject = SM.Instance<Grid>().GetCell(objectCoordinates);

            return new CollisionData(
                headCoordinates,
                objectCoordinates,
                collisionForce,
                side,
                collidedObject);
        }

        private float CalculateCollisionForce(float mass, float speedBeforeCollision, float time)
        {
            return mass * (speedBeforeCollision / time);
        }

        private CollisionSide DetermineCollisionSide(Vector2Int headCoordinates, Vector2Int objectCoordinates)
        {
            Vector2Int delta = objectCoordinates - headCoordinates;

            return delta switch
            {
                { y: 1 } => CollisionSide.Top,
                { y: -1 } => CollisionSide.Bottom,
                { x: -1 } => CollisionSide.Left,
                { x: 1 } => CollisionSide.Right,
                _ => CollisionSide.Top
            };
        }

        #endregion

        #region Movement Stop Logic

        private void StopMovement()
        {
            IsMoving = false;
            totalMovementTime += Time.time - movementStartTime;

            ResetDirection();
            AlignToNearestGrid();
            AllowDirectionChange();
            OnMoveStop?.Invoke();

            if (!HasValidMoves())
            {
                OnLevelComplete?.Invoke();
            }
        }

        private void ResetDirection()
        {
            CurrentDirection = Vector3.zero;
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

        private void OnGameStateChange(GameStateType state)
        {
            switch (state)
            {
                case GameStateType.Playing:
                    CanMove = true;
                    break;
                case GameStateType.MainMenu:
                    CanMove = false;
                    break;
                case GameStateType.ReloadingScene:
                    CanMove = false;
                    break;

                    // Додати більше відслідковування станів за потреби 
            }
        }

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
}