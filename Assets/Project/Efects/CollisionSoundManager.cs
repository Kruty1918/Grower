using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Plays random sounds on collision with objects tagged as "Wall".
    /// </summary>
    public class CollisionSoundManager : MonoBehaviour
    {
        [Header("Sound Settings")]
        [Tooltip("Array of audio clips to be played randomly upon collision.")]
        [SerializeField] private AudioClip[] collisionSounds; // Массив звуков

        private AudioSource audioSource; // Источник звука

        private void Awake()
        {
            // Получаем компонент AudioSource для воспроизведения звуков
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                // Если компонента AudioSource нет, добавляем его
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Проверяем, столкнулся ли объект с объектом с тегом "Wall"
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
                // Выбираем случайный индекс в массиве звуков
                int randomIndex = Random.Range(0, collisionSounds.Length);
                // Воспроизводим выбранный звук
                audioSource.PlayOneShot(collisionSounds[randomIndex]);
            }
        }
    }
}
