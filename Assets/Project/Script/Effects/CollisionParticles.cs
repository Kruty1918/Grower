using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Handles particle effects upon collision with a box collider by subscribing to GrowerEvents.
    /// </summary>
    public class CollisionParticles : MonoBehaviour
    {
        [Tooltip("The particle system prefab to spawn on collision.")]
        [SerializeField] private GameObject particlePrefab;

        private void OnEnable()
        {
            GrowerEvents.OnHeadCollision.AddListener(HandleCollision);
        }

        private void OnDisable()
        {
            GrowerEvents.OnHeadCollision.RemoveListener(HandleCollision);
        }

        /// <summary>
        /// Handles the collision event and spawns particles at the collision point.
        /// </summary>
        /// <param name="collisionData">Data about the collision.</param>
        private void HandleCollision(CollisionData collisionData)
        {
            if (particlePrefab == null || collisionData.CollidedObject == null)
            {
                Debug.LogWarning("Particle prefab or collided object is missing.");
                return;
            }

            // Get the collision position
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

            // Optional: Destroy particles after some time
            Destroy(particles, 2f);
        }

        /// <summary>
        /// Maps the CollisionSide enum to a direction vector.
        /// </summary>
        /// <param name="side">The side of the collision.</param>
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