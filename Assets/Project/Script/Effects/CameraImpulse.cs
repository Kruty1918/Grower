using UnityEngine;
using System.Collections;

namespace Grower
{
    /// <summary>
    /// Manages camera impulse effects triggered by collisions.
    /// The camera briefly shifts in the direction of the collision and then smoothly returns to its original position.
    /// </summary>
    public class CameraImpulse : MonoBehaviour
    {
        [SerializeField, Tooltip("Strength of the camera impulse effect.")]
        private float cameraImpulseStrength = 0.5f;

        [SerializeField, Tooltip("Duration of the camera impulse effect before returning to the original position.")]
        private float cameraImpulseDuration = 0.3f;

        [SerializeField, Tooltip("Speed at which the camera returns to its original position.")]
        private float cameraReturnSpeed = 5f;

        private Vector3 originalCameraPosition; // Stores the original position of the camera
        private bool isCameraInImpulse = false; // Prevents multiple impulses from triggering simultaneously
        private Camera mainCamera; // Reference to the main camera

        private void Awake()
        {
            mainCamera = Camera.main ?? throw new MissingReferenceException("Main Camera not found.");
            originalCameraPosition = mainCamera.transform.position; // Record the original camera position
        }

        /// <summary>
        /// Triggers the camera impulse effect in the direction opposite to the collision normal.
        /// </summary>
        /// <param name="collisionNormal">The normal vector of the collision direction.</param>
        public void TriggerImpulse(Vector3 collisionNormal)
        {
            if (!isCameraInImpulse)
            {
                StartCoroutine(CameraImpulseCoroutine(collisionNormal));
            }
        }

        /// <summary>
        /// Coroutine for handling the camera impulse effect and smooth return.
        /// </summary>
        /// <param name="collisionNormal">The normal vector of the collision direction.</param>
        private IEnumerator CameraImpulseCoroutine(Vector3 collisionNormal)
        {
            isCameraInImpulse = true; // Set flag to indicate the impulse effect is active

            // Calculate target position by applying impulse in the opposite direction of the collision normal
            Vector3 impulseDirection = collisionNormal * cameraImpulseStrength;
            Vector3 targetCameraPosition = originalCameraPosition - impulseDirection;

            // Move the camera to the target position instantly
            mainCamera.transform.position = targetCameraPosition;

            // Wait for the duration of the impulse effect
            yield return new WaitForSeconds(cameraImpulseDuration);

            // Smoothly return the camera to its original position
            while (Vector3.Distance(mainCamera.transform.position, originalCameraPosition) > 0.01f)
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, originalCameraPosition, cameraReturnSpeed * Time.deltaTime);
                yield return null;
            }

            // Ensure the camera's position is precisely restored
            mainCamera.transform.position = originalCameraPosition;
            isCameraInImpulse = false; // Reset flag to allow future impulses
        }
    }
}
