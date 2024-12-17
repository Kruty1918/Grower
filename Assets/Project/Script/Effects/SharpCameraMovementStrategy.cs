namespace Grower
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// "Sharp" camera movement strategy.
    /// This strategy applies a sharp movement of the camera in the direction of the collision, 
    /// then returns the camera smoothly to its original position.
    /// </summary>
    [CreateAssetMenu(fileName = "SharpCameraMovementStrategy", menuName = "Grower/Camera Movement/Sharp", order = 1)]
    public class SharpCameraMovementStrategy : CameraMovementStrategy
    {
        /// <summary>
        /// Executes a coroutine to handle the camera impulse movement based on a collision.
        /// The camera moves in the direction of the collision, then smoothly returns to the original position.
        /// </summary>
        /// <param name="cameraImpulse">Information about the camera impulse, including its original position.</param>
        /// <param name="collisionNormal">The collision normal that defines the direction of the impulse.</param>
        /// <param name="forceMultiplier">A multiplier for the force that increases or decreases the impulse strength.</param>
        /// <returns>A coroutine that handles the camera movement and return to its original position.</returns>
        public override IEnumerator CameraImpulseCoroutine(CameraImpulse cameraImpulse, Vector3 collisionNormal, float forceMultiplier)
        {
            // Calculate the impulse direction based on the collision normal and force multiplier
            Vector3 impulseDirection = collisionNormal * baseImpulseStrength * forceMultiplier;
            Vector3 targetCameraPosition = cameraImpulse.originalCameraPosition - impulseDirection;

            // Move the camera to the target position
            cameraImpulse.mainCamera.transform.position = targetCameraPosition;

            // Wait for the specified impulse duration
            yield return new WaitForSeconds(cameraImpulseDuration);

            // Smoothly return the camera to its original position
            while (Vector3.Distance(cameraImpulse.mainCamera.transform.position, cameraImpulse.originalCameraPosition) > 0.01f)
            {
                cameraImpulse.mainCamera.transform.position = Vector3.Lerp(cameraImpulse.mainCamera.transform.position, cameraImpulse.originalCameraPosition, cameraReturnSpeed * Time.deltaTime);
                yield return null;
            }

            // Ensure the camera returns exactly to the original position
            cameraImpulse.mainCamera.transform.position = cameraImpulse.originalCameraPosition;
            cameraImpulse.isCameraInImpulse = false;
        }
    }
}