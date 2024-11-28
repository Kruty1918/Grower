using SGS29.Utilities;
using UnityEngine;

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

        #region Private Fields

        private Vector3 currentDirection = Vector3.zero;
        private Vector3 targetPosition = Vector3.zero;
        private bool isMoving = false;
        private bool canChangeDirection = true;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            AlignToNearestGrid(); // Ensure alignment on startup
        }

        private void FixedUpdate()
        {
            if (isMoving)
                MoveToTarget();

            if (canChangeDirection)
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
            if (!canChangeDirection || direction == Vector3.zero || isMoving)
                return;

            Vector3 potentialTarget = AlignToGrid(transform.position + direction);

            if (IsObstacleAt(potentialTarget))
                return;

            currentDirection = direction;
            targetPosition = potentialTarget;

            isMoving = true;
            canChangeDirection = false;
        }

        /// <summary>
        /// Moves the object towards the target position.
        /// </summary>
        private void MoveToTarget()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

            if (HasReachedTarget())
            {
                AlignToNearestGrid();
                Vector3 nextTarget = AlignToGrid(transform.position + currentDirection);

                if (IsObstacleAt(nextTarget))
                {
                    StopMovement();
                }
                else
                {
                    targetPosition = nextTarget;
                }
            }
        }

        /// <summary>
        /// Stops all movement and resets the current direction.
        /// </summary>
        private void StopMovement()
        {
            isMoving = false;
            currentDirection = Vector3.zero;

            AlignToNearestGrid();
            AllowDirectionChange();
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
            position.x = AlignAxis(position.x);
            position.y = gridOffset.y; // Maintain vertical alignment
            position.z = AlignAxis(position.z);
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
            return new Vector2Int(AlignAxisAsInt(position.x), AlignAxisAsInt(position.z));
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Aligns a single axis value to the grid.
        /// </summary>
        /// <param name="value">The value to align.</param>
        /// <returns>The aligned value.</returns>
        private float AlignAxis(float value)
        {
            return Mathf.RoundToInt(value / gridSize) * gridSize;
        }

        /// <summary>
        /// Converts a value to grid coordinates as an integer.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The grid-aligned integer.</returns>
        private int AlignAxisAsInt(float value)
        {
            return Mathf.RoundToInt(value / gridSize);
        }

        /// <summary>
        /// Checks if the object has reached the target position.
        /// </summary>
        /// <returns>True if the object is at the target position, false otherwise.</returns>
        private bool HasReachedTarget()
        {
            return Vector3.Distance(transform.position, targetPosition) <= 0.01f;
        }

        /// <summary>
        /// Allows direction changes after movement stops.
        /// </summary>
        private void AllowDirectionChange()
        {
            canChangeDirection = true;
        }

        #endregion
    }
}