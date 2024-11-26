using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Controls the movement of an object using Rigidbody. 
    /// Allows direction changes and stops on collision with specific tagged objects.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class HeadMover : MonoBehaviour
    {
        #region Movement Settings
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Title("Movement Settings")]
        [Sirenix.OdinInspector.Title("The speed at which the object moves.")]
        [Range(1f, 20f)]
#else
        [Header("Movement Settings")]
        [Tooltip("The speed at which the object moves.")]
        [Range(1f, 20f)]
#endif
        [SerializeField]
        private float moveSpeed = 5f;
        #endregion

        #region State
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Title("State")]
        [Tooltip("Indicates if the object is currently moving.")]
        [Sirenix.OdinInspector.ReadOnly]
        [Sirenix.OdinInspector.ShowInInspector]
#else
        [Header("State")]
        [Tooltip("Indicates if the object is currently moving.")]
#endif
        private bool isMoving;

#if ODIN_INSPECTOR
        [Tooltip("Indicates if the object can change direction.")]
        [Sirenix.OdinInspector.ReadOnly]
        [Sirenix.OdinInspector.ShowInInspector]
#else
        [Tooltip("Indicates if the object can change direction.")]
#endif
        private bool canChangeDirection = true;
        #endregion

        private Rigidbody rb; // The Rigidbody component used for movement
        private Vector3 currentDirection = Vector3.zero; // The current movement direction

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            // Apply velocity based on movement state
            rb.velocity = isMoving ? currentDirection * moveSpeed : Vector3.zero;

            // Handle directional input when allowed
            if (canChangeDirection)
                HandleInput();
        }

        /// <summary>
        /// Processes input to determine movement direction.
        /// </summary>
        private void HandleInput()
        {
            if (Input.GetKey(KeyCode.W))
                SetDirection(Vector3.forward);
            else if (Input.GetKey(KeyCode.S))
                SetDirection(Vector3.back);
            else if (Input.GetKey(KeyCode.A))
                SetDirection(Vector3.left);
            else if (Input.GetKey(KeyCode.D))
                SetDirection(Vector3.right);
        }

        /// <summary>
        /// Sets the current movement direction if allowed.
        /// </summary>
        /// <param name="direction">The new movement direction.</param>
        private void SetDirection(Vector3 direction)
        {
            if (!canChangeDirection || direction == Vector3.zero)
                return;

            currentDirection = direction.normalized;
            isMoving = true;
            canChangeDirection = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                StopMovement();
            }
        }

        /// <summary>
        /// Stops all movement and resets the state.
        /// </summary>
        private void StopMovement()
        {
            isMoving = false;
            currentDirection = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Allow direction change after a brief delay
            Invoke(nameof(AllowDirectionChange), 0.1f);
        }

        /// <summary>
        /// Allows direction changes after stopping.
        /// </summary>
        private void AllowDirectionChange()
        {
            canChangeDirection = true;
        }

        #region Debug and Runtime Controls (Optional)
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button("Stop Movement")]
#else
        [ContextMenu("Stop Movement")]
#endif
        private void DebugStopMovement()
        {
            StopMovement();
        }

        #endregion
    }
}