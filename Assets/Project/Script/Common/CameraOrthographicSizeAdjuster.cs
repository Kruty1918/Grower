using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Adjusts the orthographic size of the camera to fit a specified bounds area.
    /// </summary>
    public class CameraOrthographicSizeAdjuster : MonoBehaviour
    {
        /// <summary>
        /// Minimum bounds of the area to be covered.
        /// </summary>
        public Vector3 boundsMin = new Vector3(-5f, 0f, -5f);

        /// <summary>
        /// Maximum bounds of the area to be covered.
        /// </summary>
        public Vector3 boundsMax = new Vector3(5f, 0f, 5f);

        [Header("Camera Settings")]
        /// <summary>
        /// The camera whose orthographic size will be adjusted.
        /// </summary>
        public Camera mainCamera; // The camera for which the orthographic size will change

        /// <summary>
        /// Draws the bounds in the editor for visualization.
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube((boundsMin + boundsMax) / 2, boundsMax - boundsMin);
        }

        /// <summary>
        /// Initializes the camera and adjusts its size on start.
        /// </summary>
        void Start()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            AdjustCameraSize();
        }

        /// <summary>
        /// Adjusts the camera's orthographic size to fit the specified bounds.
        /// </summary>
        void AdjustCameraSize()
        {
            if (mainCamera.orthographic)
            {
                float width = boundsMax.x - boundsMin.x;
                float height = boundsMax.z - boundsMin.z;

                float requiredSize = Mathf.Max(width / mainCamera.aspect, height) / 2f;

                mainCamera.orthographicSize = requiredSize;
            }
            else
            {
                Debug.LogWarning("Camera is not orthographic. This script only works with orthographic cameras.");
            }
        }

        /// <summary>
        /// Updates the camera size in the editor when bounds are changed.
        /// </summary>
        private void OnValidate()
        {
            if (mainCamera != null)
            {
                AdjustCameraSize();
            }
        }
    }
}