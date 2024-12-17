namespace Grower
{
    using UnityEngine;

    /// <summary>
    /// Handles particle effects upon collision with a box collider by subscribing to GrowerEvents.
    /// Spawns particle effects at the collision point when a collision occurs and aligns the particles according to the side of the collision.
    /// </summary>
    public class CollisionParticles : MonoBehaviour
    {
        /// <summary>
        /// The particle system prefab to spawn on collision.
        /// </summary>
        [Tooltip("The particle system prefab to spawn on collision.")]
        [SerializeField] private GameObject particlePrefab;

        /// <summary>
        /// Subscribes to the OnHeadCollision event when the object is enabled.
        /// </summary>
        private void OnEnable()
        {
            GrowerEvents.OnHeadCollision.AddListener(HandleCollision);
        }

        /// <summary>
        /// Unsubscribes from the OnHeadCollision event when the object is disabled.
        /// </summary>
        private void OnDisable()
        {
            GrowerEvents.OnHeadCollision.RemoveListener(HandleCollision);
        }

        /// <summary>
        /// Handles the collision event and spawns particles at the collision point.
        /// This method is triggered when the OnHeadCollision event occurs.
        /// </summary>
        /// <param name="collisionData">Data about the collision, including the coordinates and side of the collision.</param>
        private void HandleCollision(CollisionData collisionData)
        {
            // Validate the necessary references
            if (particlePrefab == null || collisionData.CollidedObject == null)
            {
                Debug.LogWarning("Particle prefab or collided object is missing.");
                return;
            }

            // Get the position of the collision and set a fixed height for spawning particles
            Vector3 collisionPosition = new Vector3(
                collisionData.ObjectCoordinates.x,
                2f, // Fixed height for particle spawn
                collisionData.ObjectCoordinates.y
            );

            // Spawn particles at the collision position
            GameObject particles = Instantiate(particlePrefab, collisionPosition, Quaternion.identity);

            // Align particles with the side of the collision
            Vector3 direction = GetDirectionFromCollisionSide(collisionData.Side);
            if (direction != Vector3.zero)
            {
                particles.transform.rotation = Quaternion.LookRotation(direction);
            }

            // Optionally destroy particles after a set time to clean up the scene
            Destroy(particles, 2f);
        }

        /// <summary>
        /// Maps the CollisionSide enum to a direction vector.
        /// This method determines the direction in which the particles should face based on the side of the collision.
        /// </summary>
        /// <param name="side">The side of the collision, represented as an enum value.</param>
        /// <returns>A Vector3 representing the direction of the collision.</returns>
        private Vector3 GetDirectionFromCollisionSide(CollisionSide side)
        {
            return side switch
            {
                CollisionSide.Top => Vector3.forward,
                CollisionSide.Bottom => Vector3.back,
                CollisionSide.Left => Vector3.left,
                CollisionSide.Right => Vector3.right,
                _ => Vector3.zero,
            };
        }
    }
}