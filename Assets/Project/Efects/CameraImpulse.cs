using UnityEngine;
using System.Collections;

namespace Grower
{
    public class CameraImpulse : MonoBehaviour
    {
        [SerializeField] private float cameraImpulseStrength = 0.5f; // Сила импульса для камеры
        [SerializeField] private float cameraImpulseDuration = 0.3f; // Длительность импульса камеры
        [SerializeField] private float cameraReturnSpeed = 5f; // Скорость возврата камеры

        private Vector3 originalCameraPosition; // Исходная позиция камеры
        private bool isCameraInImpulse = false; // Флаг, чтобы не запускать импульс несколько раз
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main; // Получаем ссылку на камеру
            originalCameraPosition = mainCamera.transform.position; // Запоминаем исходную позицию камеры
        }

        public void TriggerImpulse(Vector3 collisionNormal)
        {
            if (!isCameraInImpulse)
            {
                StartCoroutine(CameraImpulseCoroutine(collisionNormal));
            }
        }

        private IEnumerator CameraImpulseCoroutine(Vector3 collisionNormal)
        {
            // Сдвигаем камеру в сторону удара, инвертируем нормаль для правильного направления импульса
            Vector3 impulseDirection = collisionNormal * cameraImpulseStrength;
            Vector3 targetCameraPosition = originalCameraPosition - impulseDirection; // инвертируем

            // Перемещаем камеру в новое положение (плавно)
            mainCamera.transform.position = targetCameraPosition;

            isCameraInImpulse = true; // Устанавливаем флаг, чтобы начать плавное возвращение камеры

            // Ждем заданную продолжительность
            yield return new WaitForSeconds(cameraImpulseDuration);

            // Камера будет плавно возвращаться на исходную позицию
            while (Vector3.Distance(mainCamera.transform.position, originalCameraPosition) > 0.01f)
            {
                mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, originalCameraPosition, cameraReturnSpeed * Time.deltaTime);
                yield return null;
            }

            // Финальная позиция камеры
            mainCamera.transform.position = originalCameraPosition;
            isCameraInImpulse = false; // Камера вернулась на место
        }
    }
}
