using UnityEngine;

namespace Grower
{
    // Клас для обробки імпульсів камери
    public class CameraImpulse : MonoBehaviour, ICollisionListener
    {
        [SerializeField, Tooltip("Camera movement strategy.")]
        private CameraMovementStrategy movementStrategy; // Вибір стратегії руху камери

        public Vector3 originalCameraPosition; // Початкова позиція камери
        public bool isCameraInImpulse = false; // Перевірка, чи активний імпульс
        public Camera mainCamera; // Посилання на головну камеру

        private void Awake()
        {
            mainCamera = Camera.main ?? throw new MissingReferenceException("Main Camera not found.");
            originalCameraPosition = mainCamera.transform.position; // Запам'ятовуємо початкову позицію
            if (movementStrategy == null)
            {
                Debug.LogError("CameraMovementStrategy is not assigned!", this);
            }
        }

        /// <summary>
        /// Тригерить імпульс руху камери в залежності від нормалі зіткнення.
        /// </summary>
        /// <param name="collisionNormal">Нормаль до поверхні зіткнення.</param>
        /// <param name="forceMultiplier">Множник сили зіткнення.</param>
        public void TriggerImpulse(Vector3 collisionNormal, float forceMultiplier)
        {
            if (!isCameraInImpulse)
            {
                StartCoroutine(movementStrategy.CameraImpulseCoroutine(this, collisionNormal, forceMultiplier));
                isCameraInImpulse = true;
            }
        }

        /// <summary>
        /// Реагує на сповіщення про зіткнення та тригерить ефект імпульсу.
        /// </summary>
        /// <param name="collisionData">Дані про зіткнення.</param>
        public void CollisionNotify(CollisionData collisionData)
        {
            // Обчислюємо напрямок імпульсу та тригеримо ефект
            Vector3 collisionNormal = ((Vector2)(collisionData.ObjectCoordinates - collisionData.HeadCoordinates)).normalized;
            collisionNormal = new Vector3(collisionNormal.x, 0, collisionNormal.y); // Для 2D Top Down

            float forceMultiplier = Mathf.Clamp01(collisionData.CollisionForce / 10f); // Нормалізуємо силу зіткнення
            TriggerImpulse(collisionNormal, forceMultiplier);
        }
    }
}
