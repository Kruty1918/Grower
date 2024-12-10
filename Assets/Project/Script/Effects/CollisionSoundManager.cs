using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Manages the playback of random sounds upon collision with objects tagged as "Wall".
    /// </summary>
    public class CollisionSoundManager : MonoBehaviour
    {
        [Header("Sound Settings")]
        [Tooltip("Array of audio clips to play randomly upon collision.")]
        [SerializeField]
        private AudioClip[] collisionSounds; // Array of sounds to play on collision

        private AudioSource audioSource; // Audio source component for playing sounds

        private void Awake()
        {
            // Retrieve or add an AudioSource component
            audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
        }

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