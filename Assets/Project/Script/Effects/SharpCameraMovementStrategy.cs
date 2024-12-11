using System.Collections;
using UnityEngine;

namespace Grower
{
    // Стратегія "Sharp" (Різкий рух)
    [CreateAssetMenu(fileName = "SharpCameraMovementStrategy", menuName = "Grower/Camera Movement/Sharp", order = 1)]
    public class SharpCameraMovementStrategy : CameraMovementStrategy
    {
        public override IEnumerator CameraImpulseCoroutine(CameraImpulse cameraImpulse, Vector3 collisionNormal, float forceMultiplier)
        {
            // Обчислюємо імпульс
            Vector3 impulseDirection = collisionNormal * baseImpulseStrength * forceMultiplier;
            Vector3 targetCameraPosition = cameraImpulse.originalCameraPosition - impulseDirection;

            // Рухаємо камеру до цільової позиції
            cameraImpulse.mainCamera.transform.position = targetCameraPosition;

            // Чекаємо заданий час імпульсу
            yield return new WaitForSeconds(cameraImpulseDuration);

            // Плавне повернення до початкової позиції
            while (Vector3.Distance(cameraImpulse.mainCamera.transform.position, cameraImpulse.originalCameraPosition) > 0.01f)
            {
                cameraImpulse.mainCamera.transform.position = Vector3.Lerp(cameraImpulse.mainCamera.transform.position, cameraImpulse.originalCameraPosition, cameraReturnSpeed * Time.deltaTime);
                yield return null;
            }

            // Забезпечуємо точне відновлення початкової позиції
            cameraImpulse.mainCamera.transform.position = cameraImpulse.originalCameraPosition;
            cameraImpulse.isCameraInImpulse = false;
        }
    }
}
