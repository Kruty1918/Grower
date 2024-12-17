using UnityEngine;

namespace Grower
{
    /// <summary>
    /// This class handles camera impulses triggered by collisions. It modifies the camera's position
    /// based on the collision normal and force, using the assigned camera movement strategy.
    /// </summary>
    public class CameraImpulse : MonoBehaviour, ICollisionListener
    {
        [SerializeField, Tooltip("Camera movement strategy.")]
        private CameraMovementStrategy movementStrategy; // The strategy for handling camera movement during the impulse

        /// <summary>
        /// The original position of the camera before the impulse is applied.
        /// </summary>
        public Vector3 originalCameraPosition;

        /// <summary>
        /// Flag indicating whether the camera is currently under the effect of an impulse.
        /// </summary>
        public bool isCameraInImpulse = false;

        /// <summary>
        /// Reference to the main camera in the scene.
        /// </summary>
        public Camera mainCamera;

        /// <summary>
        /// Called when the script is initialized. Ensures the main camera is found and the movement strategy is assigned.
        /// </summary>
        private void Awake()
        {
            mainCamera = Camera.main ?? throw new MissingReferenceException("Main Camera not found.");
            originalCameraPosition = mainCamera.transform.position; // Store the initial camera position
            if (movementStrategy == null)
            {
                Debug.LogError("CameraMovementStrategy is not assigned!", this);
            }
        }

        /// <summary>
        /// Triggers a camera impulse based on the collision normal and force multiplier.
        /// </summary>
        /// <param name="collisionNormal">The normal vector of the collision surface, indicating the direction of the impact.</param>
        /// <param name="forceMultiplier">A multiplier that adjusts the intensity of the impulse based on the collision force.</param>
        public void TriggerImpulse(Vector3 collisionNormal, float forceMultiplier)
        {
            // Only trigger the impulse if it is not already active
            if (!isCameraInImpulse)
            {
                // Start the coroutine to handle the camera impulse movement
                StartCoroutine(movementStrategy.CameraImpulseCoroutine(this, collisionNormal, forceMultiplier));
                isCameraInImpulse = true; // Set the flag to indicate the impulse is active
            }
        }

        /// <summary>
        /// Responds to collision notifications and triggers the camera impulse.
        /// </summary>
        /// <param name="collisionData">The collision data containing the details of the collision, such as coordinates and force.</param>
        public void CollisionNotify(CollisionData collisionData)
        {
            // Calculate the direction of the impulse based on the collision data
            Vector3 collisionNormal = ((Vector2)(collisionData.ObjectCoordinates - collisionData.HeadCoordinates)).normalized;

            // Convert to 3D vector assuming a top-down 2D view (set y to 0)
            collisionNormal = new Vector3(collisionNormal.x, 0, collisionNormal.y);

            // Normalize the collision force (clamping it between 0 and 1)
            float forceMultiplier = Mathf.Clamp01(collisionData.CollisionForce / 10f);

            // Trigger the camera impulse with the calculated normal and force
            TriggerImpulse(collisionNormal, forceMultiplier);
        }
    }
}