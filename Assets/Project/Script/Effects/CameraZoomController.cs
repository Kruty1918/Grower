using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Controls the zoom of an orthographic camera based on player movement and collision.
    /// </summary>
    public class CameraZoomController : MonoBehaviour
    {
        [SerializeField, Tooltip("The camera to be zoomed.")]
        private Camera targetCamera;

        [SerializeField, Tooltip("Reference to the HeadMover component controlling player movement.")]
        private HeadMover headMover;

        [SerializeField, Tooltip("Speed of camera size adjustment.")]
        private float zoomSpeed = 1f;

        [SerializeField, Tooltip("Maximum camera size when zooming out.")]
        private float maxSize = 6f;

        [SerializeField, Tooltip("Default camera size when not zooming out.")]
        private float defaultSize = 4f;

        private bool isZoomingOut = false; // Flag to control zoom-out state

        private void Awake()
        {
            // Validate required references
            if (targetCamera == null)
                Debug.LogError("CameraZoomController: Target camera is not assigned.");
            if (headMover == null)
                Debug.LogError("CameraZoomController: HeadMover is not assigned.");
        }

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
        /// </summary>
        private void AdjustCameraSize()
        {
            if (targetCamera == null || !targetCamera.orthographic)
                return;

            if (isZoomingOut)
            {
                // Smoothly increase the camera size
                targetCamera.orthographicSize = Mathf.Lerp(targetCamera.orthographicSize, maxSize, zoomSpeed * Time.deltaTime);
            }
            else
            {
                // Smoothly return the camera size to the default
                targetCamera.orthographicSize = Mathf.Lerp(targetCamera.orthographicSize, defaultSize, zoomSpeed * Time.deltaTime);
            }
        }
    }
}