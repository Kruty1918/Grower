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

        // Stores the result builder instance for building results related to movement and collisions
        private ResultBuilder resultBuilder;

        #endregion

        #region Unity Lifecycle

        /// <summary>
        /// Called when the script is first initialized. Aligns the object to the nearest grid on startup.
        /// </summary>
        private void Awake()
        {
            // Ensure the object is aligned to the grid when the game starts
            AlignToNearestGrid();
        }

        private void Start()
        {
            // Register the InputManager listener to handle user input
            SM.Instance<InputManager>().AddListener(this);

            // Initialize the ResultBuilder, passing the current component and the level validator
            resultBuilder = new ResultBuilder(this, levelValidator);
        }

        /// <summary>
        /// Called on every fixed frame. Moves the object towards the target if it's moving, and calculates its speed.
        /// Also processes input if the direction can be changed.
        /// </summary>
        private void FixedUpdate()
        {
            // If the object is moving and movement is allowed, move it towards the target and calculate the speed
            if (IsMoving && CanMove)
            {
                MoveToTarget();
                CalculateSpeed();
            }
        }

        /// <summary>
        /// Calculates the current speed of the object based on the distance moved in the last fixed frame.
        /// </summary>
        private void CalculateSpeed()
        {
            // Calculate the distance moved in this frame
            float distanceMoved = Vector3.Distance(transform.position, previousPosition);

            // Calculate the speed by dividing the distance moved by the time passed (fixedDeltaTime)
            CurrentSpeed = distanceMoved / Time.fixedDeltaTime;

            // Store the current position to calculate the distance in the next frame
            previousPosition = transform.position;
        }

        /// <summary>
        /// Called when the object is enabled. Subscribes to the OnLevelComplete event of the HeadMover component.
        /// </summary>
        private void OnEnable()
        {
            // Subscribe to the OnLevelComplete event and the GameStateChange event
            OnLevelComplete += HandleLevelComplete;
            GrowerEvents.OnGameStateChange.AddListener(OnGameStateChange);
        }

        /// <summary>
        /// Called when the object is disabled. Unsubscribes from the OnLevelComplete event of the HeadMover component.
        /// </summary>
        private void OnDisable()
        {
            // Unsubscribe from the OnLevelComplete and GameStateChange events
            OnLevelComplete -= HandleLevelComplete;
            GrowerEvents.OnGameStateChange.RemoveListener(OnGameStateChange);

            // If InputManager exists, remove the current listener
            if (SM.HasSingleton<InputManager>())
                SM.Instance<InputManager>().RemoveListener(this);
        }

        #endregion

        #region Swipe Handling

        /// <summary>
        /// Handles swipe input and sets the movement direction based on the swipe direction.
        /// </summary>
        /// <param name="direction">The swipe direction represented as a 2D vector.</param>
        public void OnSwipe(Vector2 direction)
        {
            // If movement is not allowed, return without doing anything
            if (!CanMove) return;

            // Get the primary direction from the swipe input
            direction = GetPrimaryDirection(direction);

            // Convert the 2D swipe direction to a 3D direction on the XZ plane
            Vector3 nDirection = new Vector3(direction.x, 0, direction.y);

            // If direction change is allowed, attempt to set the new direction
            if (CanChangeDirection)
                TrySetDirection(nDirection);
        }

        /// <summary>
        /// Determines the primary direction of the swipe based on the absolute values of the x and y components.
        /// </summary>
        /// <param name="direction">The swipe direction as a 2D vector.</param>
        /// <returns>A 2D vector representing the primary direction.</returns>
        private Vector2 GetPrimaryDirection(Vector2 direction)
        {
            // If the x component has a greater absolute value than the y component, use x as the primary direction
            // Otherwise, use y as the primary direction
            return Mathf.Abs(direction.x) > Mathf.Abs(direction.y)
                ? new Vector2(direction.x, 0) // Only keep x-axis movement
                : new Vector2(0, direction.y); // Only keep y-axis movement
        }

        #endregion

        #region Direction Setting

        /// <summary>
        /// Attempts to set the movement direction if it's valid and possible.
        /// </summary>
        /// <param name="direction">The direction to attempt to set.</param>
        private void TrySetDirection(Vector3 direction)
        {
            // Check if the direction change is allowed
            if (!CanAttemptDirectionChange(direction))
                return;

            // Calculate the potential target position based on the direction
            Vector3 potentialTarget = CalculateTargetPosition(direction);

            // Check if the target position is valid (i.e., no obstacles)
            if (IsValidTargetPosition(potentialTarget))
                UpdateDirectionAndStartMovement(direction, potentialTarget); // Update direction and start movement
        }

        /// <summary>
        /// Checks if a direction change can be attempted.
        /// </summary>
        /// <param name="direction">The direction to check.</param>
        /// <returns>True if the direction change is allowed, false otherwise.</returns>
        private bool CanAttemptDirectionChange(Vector3 direction)
        {
            return CanChangeDirection && direction != Vector3.zero && !IsMoving;
        }

        /// <summary>
        /// Calculates the target position based on the given direction.
        /// </summary>
        /// <param name="direction">The direction to move in.</param>
        /// <returns>The calculated target position.</returns>
        private Vector3 CalculateTargetPosition(Vector3 direction)
        {
            return Utility.AlignToGrid(transform.position + direction, settings); // Aligns the target to the grid
        }

        /// <summary>
        /// Checks if the given target position is valid (i.e., no obstacles).
        /// </summary>
        /// <param name="targetPosition">The target position to check.</param>
        /// <returns>True if the target position is valid, false if there is an obstacle.</returns>
        private bool IsValidTargetPosition(Vector3 targetPosition)
        {
            return !IsObstacleAt(targetPosition); // Returns true if there is no obstacle at the target position
        }

        /// <summary>
        /// Updates the direction and sets the movement target to the specified position.
        /// </summary>
        /// <param name="direction">The direction to move in.</param>
        /// <param name="targetPosition">The target position to move towards.</param>
        private void UpdateDirectionAndStartMovement(Vector3 direction, Vector3 targetPosition)
        {
            SetNewDirection(direction);        // Set the new direction
            SetMovementTarget(targetPosition); // Set the target position and start moving
        }

        /// <summary>
        /// Sets a new direction for movement if it's different from the current direction.
        /// </summary>
        /// <param name="direction">The new direction to set.</param>
        private void SetNewDirection(Vector3 direction)
        {
            if (CurrentDirection != direction)
            {
                CurrentDirection = direction;               // Update the current direction
                OnDirectionChange?.Invoke(CurrentDirection); // Invoke the direction change event
            }
        }

        /// <summary>
        /// Sets the movement target position and starts the movement process.
        /// </summary>
        /// <param name="targetPosition">The target position to move towards.</param>
        private void SetMovementTarget(Vector3 targetPosition)
        {
            TargetPosition = targetPosition;   // Set the target position
            IsMoving = true;                    // Mark the object as moving
            CanChangeDirection = false;         // Prevent direction changes while moving
            movementStartTime = Time.time;      // Record the time when the movement starts
            OnMoveStart?.Invoke(CurrentDirection); // Invoke the movement start event
        }

        #endregion

        #region Movement Logic

        /// <summary>
        /// Moves the object towards the target position using the current movement strategy.
        /// </summary>
        private void MoveToTarget()
        {
            movementStrategy.Move(transform, TargetPosition); // Apply the movement strategy to move the object

            // Check if the object has reached the target
            if (HasReachedTarget())
                HandleTargetReached(); // Handle the target reached event if the target is reached
        }

        /// <summary>
        /// Handles the actions required when the target is reached.
        /// </summary>
        private void HandleTargetReached()
        {
            AlignToNearestGrid();  // Align the object to the nearest grid position
            TrackMovementPath();   // Track the movement path for the object

            // Calculate the next target position after the current movement
            Vector3 nextTarget = Utility.AlignToGrid(transform.position + CurrentDirection, settings);

            // Check if there is an obstacle at the next target position
            if (IsObstacleAt(nextTarget))
            {
                CollisionEnter();    // Handle the collision event
                StopMovement();      // Stop the movement if an obstacle is present
            }
            else
            {
                TargetPosition = nextTarget; // Set the new target position if no obstacle is detected
            }
        }

        /// <summary>
        /// Tracks the movement path of the object by recording its grid coordinates.
        /// </summary>
        private void TrackMovementPath()
        {
            Vector2Int currentGridCoord = Utility.ConvertToGridCoords(transform.position, settings);

            // Add the current grid coordinate to the movement path if it's not already the last entry
            if (movementPathTracker.Count == 0 || movementPathTracker[^1] != currentGridCoord)
            {
                movementPathTracker.Add(currentGridCoord);
            }
        }

        #endregion

        #region Collision Handling

        /// <summary>
        /// Handles the collision by preparing and invoking the collision data.
        /// </summary>
        private void CollisionEnter()
        {
            CollisionData data = PrepareCollisionData(); // Prepare the collision data
            GrowerEvents.OnHeadCollision?.Invoke(data); // Invoke the collision event with the data
        }

        /// <summary>
        /// Prepares the collision data, including target coordinates, collision force, and collision side.
        /// </summary>
        /// <returns>The prepared collision data.</returns>
        private CollisionData PrepareCollisionData()
        {
            // Calculate the next target position aligned to the grid
            Vector3 nextTarget = Utility.AlignToGrid(transform.position + CurrentDirection, settings);

            // Get the current and next object coordinates in the grid system
            Vector2Int headCoordinates = Utility.ConvertToGridCoords(transform.position, settings);
            Vector2Int objectCoordinates = Utility.ConvertToGridCoords(nextTarget, settings);

            // Calculate the collision force based on mass, speed, and time
            float collisionForce = CalculateCollisionForce(settings.objectMass, CurrentSpeed, Time.fixedDeltaTime);

            // Determine the side of the collision based on coordinates
            CollisionSide side = DetermineCollisionSide(headCoordinates, objectCoordinates);

            // Get the collided object from the grid
            Cell collidedObject = SM.Instance<Grid>().GetCell(objectCoordinates);

            // Return the prepared collision data
            return new CollisionData(
                headCoordinates,
                objectCoordinates,
                collisionForce,
                side,
                collidedObject);
        }

        /// <summary>
        /// Calculates the collision force based on the mass, speed before the collision, and time.
        /// </summary>
        /// <param name="mass">The mass of the object involved in the collision.</param>
        /// <param name="speedBeforeCollision">The speed of the object before the collision.</param>
        /// <param name="time">The time period during which the collision happens.</param>
        /// <returns>The calculated collision force.</returns>
        private float CalculateCollisionForce(float mass, float speedBeforeCollision, float time)
        {
            return mass * (speedBeforeCollision / time); // Formula for force: F = m * (v / t)
        }

        /// <summary>
        /// Determines the side of the collision based on the change in coordinates between the head and the target.
        /// </summary>
        /// <param name="headCoordinates">The coordinates of the object’s head.</param>
        /// <param name="objectCoordinates">The coordinates of the target object.</param>
        /// <returns>The side of the collision.</returns>
        private CollisionSide DetermineCollisionSide(Vector2Int headCoordinates, Vector2Int objectCoordinates)
        {
            // Calculate the difference in coordinates to determine the collision side
            Vector2Int delta = objectCoordinates - headCoordinates;

            return delta switch
            {
                { y: 1 } => CollisionSide.Top,    // If delta.y is 1, it's a collision from the top
                { y: -1 } => CollisionSide.Bottom, // If delta.y is -1, it's a collision from the bottom
                { x: -1 } => CollisionSide.Left,   // If delta.x is -1, it's a collision from the left
                { x: 1 } => CollisionSide.Right,   // If delta.x is 1, it's a collision from the right
                _ => CollisionSide.Top             // Default to top if no specific match is found
            };
        }

        #endregion

        #region Movement Stop Logic

        /// <summary>
        /// Stops the movement by updating relevant flags, resetting direction, aligning to the nearest grid, and invoking necessary events.
        /// </summary>
        private void StopMovement()
        {
            IsMoving = false; // Set the movement flag to false
            totalMovementTime += Time.time - movementStartTime; // Update the total movement time

            ResetDirection(); // Reset the current direction
            AlignToNearestGrid(); // Align the object to the nearest grid position
            AllowDirectionChange(); // Allow direction changes after stopping

            OnMoveStop?.Invoke(); // Invoke the move stop event

            // Check if there are no valid moves left
            if (!HasValidMoves())
            {
                OnLevelComplete?.Invoke(); // Invoke the level complete event
            }
        }

        /// <summary>
        /// Resets the current movement direction to zero.
        /// </summary>
        private void ResetDirection()
        {
            CurrentDirection = Vector3.zero; // Set the current direction to zero
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
        /// Handles the change of game state and adjusts movement ability accordingly.
        /// </summary>
        /// <param name="state">The new game state.</param>
        private void OnGameStateChange(GameStateType state)
        {
            switch (state)
            {
                case GameStateType.Playing:
                    CanMove = true; // Movement enabled when playing
                    break;
                case GameStateType.MainMenu:
                    CanMove = false; // Movement disabled in main menu
                    break;
                case GameStateType.ReloadingScene:
                    CanMove = false; // Movement disabled during scene reload
                    break;

                    // Add more state tracking if needed
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
            // Define potential movement directions
            Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };

            // Check each direction for obstacles
            foreach (var direction in directions)
            {
                Vector3 potentialTarget = Utility.AlignToGrid(transform.position + direction, settings);

                // If no obstacle is found, movement is possible in this direction
                if (!IsObstacleAt(potentialTarget))
                    return true;
            }

            return false; // No valid directions left
        }

        /// <summary>
        /// Checks if the object has reached the target position.
        /// </summary>
        /// <returns>True if the object is at the target position, false otherwise.</returns>
        private bool HasReachedTarget()
        {
            // Return true if the distance to the target position is small enough
            return Vector3.Distance(transform.position, TargetPosition) <= 0.01f;
        }

        /// <summary>
        /// Allows direction changes after movement stops.
        /// </summary>
        private void AllowDirectionChange()
        {
            CanChangeDirection = true; // Enable direction change after movement stops
        }

        #endregion
    }
}