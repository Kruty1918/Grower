namespace Grower
{
    using UnityEngine;

    /// <summary>
    /// Manages the playback of random sounds upon collision with objects tagged as "Wall".
    /// The class listens for collision events and plays a random sound from the array of sounds provided.
    /// </summary>
    public class CollisionSoundManager : MonoBehaviour
    {
        /// <summary>
        /// Array of audio clips to play randomly upon collision.
        /// </summary>
        [Header("Sound Settings")]
        [Tooltip("Array of audio clips to play randomly upon collision.")]
        [SerializeField]
        private AudioClip[] collisionSounds; // Array of sounds to play on collision

        /// <summary>
        /// Audio source component for playing sounds.
        /// </summary>
        private AudioSource audioSource; // Audio source component for playing sounds

        /// <summary>
        /// Initializes the AudioSource component.
        /// This method ensures that the object has an AudioSource component attached, either by retrieving an existing one or adding a new one.
        /// </summary>
        private void Awake()
        {
            // Retrieve or add an AudioSource component
            audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        }

        /// <summary>
        /// Called when a collision occurs with another object.
        /// If the collision is with an object tagged as "Wall", a random sound is played.
        /// </summary>
        /// <param name="collision">Information about the collision event.</param>
        private void OnCollisionEnter(Collision collision)
        {
            // Check if the collision is with an object tagged as "Wall"
            if (collision.gameObject.CompareTag("Wall"))
            {
                PlayRandomCollisionSound();
            }
        }

        /// <summary>
        /// Plays a random sound from the collisionSounds array.
        /// This method selects a random sound from the provided array and plays it.
        /// </summary>
        private void PlayRandomCollisionSound()
        {
            if (collisionSounds.Length > 0)
            {
                // Select a random sound from the array
                int randomIndex = Random.Range(0, collisionSounds.Length);
                // Play the selected sound
                audioSource.PlayOneShot(collisionSounds[randomIndex]);
            }
        }
    }
}