using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Controls the zoom of an orthographic camera based on player movement and collision.
    /// This script adjusts the orthographic camera's size dynamically, zooming in or out depending on whether
    /// the player is moving or not.
    /// </summary>
    public class CameraZoomController : MonoBehaviour
    {
        /// <summary>
        /// The camera to be zoomed.
        /// </summary>
        [SerializeField, Tooltip("The camera to be zoomed.")]
        private Camera targetCamera;

        /// <summary>
        /// Reference to the HeadMover component controlling player movement.
        /// </summary>
        [SerializeField, Tooltip("Reference to the HeadMover component controlling player movement.")]
        private HeadMover headMover;

        /// <summary>
        /// Speed of camera size adjustment. Determines how fast the camera zooms in or out.
        /// </summary>
        [SerializeField, Tooltip("Speed of camera size adjustment.")]
        private float zoomSpeed = 1f;

        /// <summary>
        /// Maximum camera size when zooming out.
        /// </summary>
        [SerializeField, Tooltip("Maximum camera size when zooming out.")]
        private float zoomOut = 6f;

        /// <summary>
        /// The default camera size when not zooming in or out.
        /// </summary>
        private float defaultSize;

        /// <summary>
        /// Flag to control zoom-out state based on player movement.
        /// </summary>
        private bool isZoomingOut = false;

        /// <summary>
        /// Validates the references for the target camera and the head mover on Awake.
        /// Sets the default camera size at the start.
        /// </summary>
        private void Awake()
        {
            // Validate required references
            if (targetCamera == null)
                Debug.LogError("CameraZoomController: Target camera is not assigned.");
            if (headMover == null)
                Debug.LogError("CameraZoomController: HeadMover is not assigned.");
        }

        /// <summary>
        /// Stores the default camera size at the start of the game.
        /// </summary>
        private void Start()
        {
            defaultSize = targetCamera.orthographicSize;
        }

        /// <summary>
        /// Checks if the player is moving and adjusts the zoom state accordingly.
        /// It updates the camera's zoom based on the movement status.
        /// </summary>
        private void Update()
        {
            // Check if the player is moving
            if (headMover != null && headMover.IsMoving && headMover.CanMove)
            {
                isZoomingOut = true;
            }
            else
            {
                isZoomingOut = false;
            }

            // Adjust the camera size accordingly
            AdjustCameraSize();
        }

        /// <summary>
        /// Adjusts the size of the camera based on the zoom state.
        /// The camera zooms in when the player is not moving and zooms out when the player is moving.
        /// </summary>
        private void AdjustCameraSize()
        {
            if (targetCamera == null || !targetCamera.orthographic)
                return;

            if (isZoomingOut)
            {
                // Smoothly increase the camera size
                targetCamera.orthographicSize = Mathf.Lerp(targetCamera.orthographicSize, targetCamera.orthographicSize + zoomOut, zoomSpeed * Time.deltaTime);
            }
            else
            {
                // Smoothly return the camera size to the default
                targetCamera.orthographicSize = Mathf.Lerp(targetCamera.orthographicSize, defaultSize, zoomSpeed * Time.deltaTime);
            }
        }
    }
}