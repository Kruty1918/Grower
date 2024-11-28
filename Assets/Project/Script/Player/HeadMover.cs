using SGS29.Utilities;
using UnityEngine;
using System;

namespace Grower
{
    /// <summary>
    /// Controls movement on a grid with alignment to a starting offset.
    /// </summary>
    public class HeadMover : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Movement Settings")]
        [Tooltip("The speed at which the object moves.")]
        [Range(1f, 20f)]
        [SerializeField] private float moveSpeed = 5f;

        [Tooltip("The size of the grid cells.")]
        [SerializeField] private float gridSize = 1f;

        [Tooltip("The starting offset for grid alignment.")]
        [SerializeField] private Vector3 gridOffset = Vector3.zero;

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

        #endregion

        #region Fields

        public Vector3 CurrentDirection { get; private set; } = Vector3.zero;
        public Vector3 TargetPosition { get; private set; } = Vector3.zero;
        public bool IsMoving { get; private set; } = false;
        public bool CanChangeDirection { get; private set; } = true;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            AlignToNearestGrid(); // Ensure alignment on startup
        }

        private void FixedUpdate()
        {
            if (IsMoving)
                MoveToTarget();

            if (CanChangeDirection)
                ProcessInput();
        }

        #endregion

        #region Movement Logic

        /// <summary>
        /// Processes player input to determine the movement direction.
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
                Vector3 nextTarget = AlignToGrid(transform.position + CurrentDirection);

                if (IsObstacleAt(nextTarget))
                {
                    StopMovement();
                }
                else
                {
                    TargetPosition = nextTarget;
                }
            }
        }

        /// <summary>
        /// Stops all movement and resets the current direction.
        /// </summary>
        private void StopMovement()
        {
            IsMoving = false;
            CurrentDirection = Vector3.zero;

            AlignToNearestGrid();
            AllowDirectionChange();

            OnMoveStop?.Invoke(); // Trigger movement stop event
        }

        #endregion

        #region Grid Alignment and Collision

        /// <summary>
        /// Checks if the given position is blocked by an obstacle.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns>True if an obstacle exists, false otherwise.</returns>
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
        /// Aligns a position to the grid with an offset.
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