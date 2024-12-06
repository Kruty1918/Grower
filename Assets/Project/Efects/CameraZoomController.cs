using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Controls the zoom of an orthographic camera based on player movement and collision.
    /// </summary>
    public class CameraZoomController : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera; // Камера, размер которой нужно изменять
        [SerializeField] private HeadMover headMover; // Ссылка на объект HeadMover
        [SerializeField] private float zoomSpeed = 1f; // Скорость изменения размера камеры
        [SerializeField] private float maxSize = 6f;   // Максимальный размер камеры
        [SerializeField] private float defaultSize = 4f; // Исходный размер камеры

        private bool isZoomingOut = false; // Флаг для управления увеличением камеры

        private void Awake()
        {
            // Проверяем наличие ссылок
            if (targetCamera == null)
                Debug.LogError("CameraZoomController: Target camera is not assigned.");
            if (headMover == null)
                Debug.LogError("CameraZoomController: HeadMover is not assigned.");
        }

        private void Update()
        {
            // Проверяем состояние движения
            if (headMover != null && headMover.IsMoving)
            {
                isZoomingOut = true;
            }
            else
            {
                isZoomingOut = false;
            }

            // Изменяем размер камеры
            AdjustCameraSize();
        }

        private void AdjustCameraSize()
        {
            if (targetCamera == null || !targetCamera.orthographic)
                return;

            if (isZoomingOut)
            {
                // Увеличиваем размер камеры
                targetCamera.orthographicSize = Mathf.Lerp(targetCamera.orthographicSize, maxSize, zoomSpeed * Time.deltaTime);
            }
            else
            {
                // Возвращаем размер камеры к исходному
                targetCamera.orthographicSize = Mathf.Lerp(targetCamera.orthographicSize, defaultSize, zoomSpeed * Time.deltaTime);
            }
        }
    }
}
