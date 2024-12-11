using System.Collections;
using UnityEngine;

namespace Grower
{
    // Стратегія "Smooth" (Плавний рух)
    [CreateAssetMenu(fileName = "SmoothCameraMovementStrategy", menuName = "Grower/Camera Movement/Smooth", order = 2)]
    public class SmoothCameraMovementStrategy : CameraMovementStrategy
    {
        public override IEnumerator CameraImpulseCoroutine(CameraImpulse cameraImpulse, Vector3 collisionNormal, float forceMultiplier)
        {
            // Обчислюємо імпульс
            Vector3 impulseDirection = collisionNormal * baseImpulseStrength * forceMultiplier;
            Vector3 targetCameraPosition = cameraImpulse.originalCameraPosition - impulseDirection * 0.5f;

            // Рухаємо камеру до цільової позиції
            cameraImpulse.mainCamera.transform.position = targetCameraPosition;

            // Чекаємо заданий час імпульсу
            yield return new WaitForSeconds(cameraImpulseDuration);

            // Плавне повернення до початкової позиції
            float elapsedTime = 0f;
            while (elapsedTime < cameraImpulseDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / cameraImpulseDuration);
                cameraImpulse.mainCamera.transform.position = Vector3.Lerp(cameraImpulse.mainCamera.transform.position, cameraImpulse.originalCameraPosition, t);
                yield return null;
            }

            // Забезпечуємо точне відновлення початкової позиції
            cameraImpulse.mainCamera.transform.position = cameraImpulse.originalCameraPosition;
            cameraImpulse.isCameraInImpulse = false;
        }
    }
}
