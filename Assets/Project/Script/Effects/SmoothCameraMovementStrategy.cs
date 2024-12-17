namespace Grower
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// "Smooth" camera movement strategy.
    /// This strategy applies a smooth camera movement after a collision and returns it smoothly to its original position.
    /// </summary>
    [CreateAssetMenu(fileName = "SmoothCameraMovementStrategy", menuName = "Grower/Camera Movement/Smooth", order = 2)]
    public class SmoothCameraMovementStrategy : CameraMovementStrategy
    {
        /// <summary>
        /// Executes a coroutine to handle the camera impulse movement based on a collision.
        /// The camera moves smoothly in the direction of the collision and then smoothly returns to its original position.
        /// </summary>
        /// <param name="cameraImpulse">Information about the camera impulse, including its original position.</param>
        /// <param name="collisionNormal">The collision normal that defines the direction of the impulse.</param>
        /// <param name="forceMultiplier">A multiplier for the force that increases or decreases the impulse strength.</param>
        /// <returns>A coroutine that handles the camera movement and return to its original position.</returns>
        public override IEnumerator CameraImpulseCoroutine(CameraImpulse cameraImpulse, Vector3 collisionNormal, float forceMultiplier)
        {
            // Calculate the impulse direction based on the collision normal and force multiplier
            Vector3 impulseDirection = collisionNormal * baseImpulseStrength * forceMultiplier;
            Vector3 targetCameraPosition = cameraImpulse.originalCameraPosition - impulseDirection * 0.5f;

            // Move the camera to the target position
            cameraImpulse.mainCamera.transform.position = targetCameraPosition;

            // Wait for the specified impulse duration
            yield return new WaitForSeconds(cameraImpulseDuration);

            // Smoothly return the camera to its original position
            float elapsedTime = 0f;
            while (elapsedTime < cameraImpulseDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / cameraImpulseDuration);
                cameraImpulse.mainCamera.transform.position = Vector3.Lerp(cameraImpulse.mainCamera.transform.position, cameraImpulse.originalCameraPosition, t);
                yield return null;
            }

            // Ensure the camera returns exactly to the original position
            cameraImpulse.mainCamera.transform.position = cameraImpulse.originalCameraPosition;
            cameraImpulse.isCameraInImpulse = false;
        }
    }
}