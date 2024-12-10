using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Handles particle effects upon collision with a box collider.
    /// </summary>
    public class CollisionParticles : MonoBehaviour
    {
        [Tooltip("The particle system prefab to spawn on collision.")]
        [SerializeField] private GameObject particlePrefab;

        private Rigidbody rb;
        private Vector3 lastDirection = Vector3.zero; // Store the last movement direction

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            // Handle directional input to update the last direction
            if (Input.GetKey(KeyCode.W)) lastDirection = Vector3.forward;
            else if (Input.GetKey(KeyCode.S)) lastDirection = Vector3.back;
            else if (Input.GetKey(KeyCode.A)) lastDirection = Vector3.left;
            else if (Input.GetKey(KeyCode.D)) lastDirection = Vector3.right;
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Check if the collision is with a BoxCollider
            if (collision.collider is BoxCollider && particlePrefab != null)
            {
                // Get the collision point and adjust the Y position
                ContactPoint contact = collision.contacts[0];
                Vector3 particlePosition = new Vector3(contact.point.x, 2f, contact.point.z);

                // Calculate the position of the particle effect very close to the center of the front face
                // Decrease the distance by using a smaller factor (e.g. 0.2f instead of 0.5f)
                Vector3 particleSpawnPosition = transform.position + (lastDirection.normalized * 0.4f); // Even closer to the center
                particleSpawnPosition.y = 2f; // Keep Y position constant

                // Instantiate particle effect at the adjusted position
                GameObject particles = Instantiate(particlePrefab, particleSpawnPosition, Quaternion.identity);

                // Align particle effect with the last movement direction
                if (lastDirection != Vector3.zero)
                {
                    particles.transform.rotation = Quaternion.LookRotation(lastDirection);
                }

                // Optional: Destroy particles after some time
                Destroy(particles, 2f);
            }
        }
    }
}
