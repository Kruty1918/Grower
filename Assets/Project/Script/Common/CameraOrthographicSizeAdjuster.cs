using UnityEngine;

namespace Grower
{
    public class CameraOrthographicSizeAdjuster : MonoBehaviour
    {
        // Мінімальні та максимальні межі області, яку потрібно охопити
        public Vector3 boundsMin = new Vector3(-5f, 0f, -5f);
        public Vector3 boundsMax = new Vector3(5f, 0f, 5f);

        [Header("Camera Settings")]
        public Camera mainCamera; // Камера, для якої буде змінюватись розмір ортографії

        private void OnDrawGizmos()
        {
            // Малюємо межі в редакторі для візуалізації
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube((boundsMin + boundsMax) / 2, boundsMax - boundsMin);
        }

        void Start()
        {
            // Якщо камера не призначена, використовуємо камеру за замовчуванням
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            // Визначаємо розмір ортографії, щоб охопити всі об'єкти в межах
            AdjustCameraSize();
        }

        void AdjustCameraSize()
        {
            if (mainCamera.orthographic)
            {
                // Обчислюємо відстань по осі X і Y
                float width = boundsMax.x - boundsMin.x;
                float height = boundsMax.z - boundsMin.z;

                // Визначаємо оптимальний розмір ортографії для охоплення всієї області
                float requiredSize = Mathf.Max(width / mainCamera.aspect, height) / 2f;

                // Задаємо розмір ортографії
                mainCamera.orthographicSize = requiredSize;
            }
            else
            {
                Debug.LogWarning("Camera is not orthographic. This script only works with orthographic cameras.");
            }
        }

        // Викликається, коли зміни в сцені і потрібно адаптувати камеру
        private void OnValidate()
        {
            // Оновлюємо камеру в редакторі при зміні меж
            if (mainCamera != null)
            {
                AdjustCameraSize();
            }
        }
    }
}