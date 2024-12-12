using System.Collections;
using UnityEngine;

namespace Grower
{
    // Абстрактний клас для стратегії руху камери
    public abstract class CameraMovementStrategy : ScriptableObject
    {
        [Header("Impulse Settings")]
        [Tooltip("Base strength of the camera impulse effect.")]
        public float baseImpulseStrength = 0.5f;

        [Tooltip("Duration of the camera impulse effect before returning to the original position.")]
        public float cameraImpulseDuration = 0.3f;

        [Tooltip("Speed at which the camera returns to its original position.")]
        public float cameraReturnSpeed = 5f;

        // Абстрактний метод для реалізації в конкретних стратегіях
        public abstract IEnumerator CameraImpulseCoroutine(CameraImpulse cameraImpulse, Vector3 collisionNormal, float forceMultiplier);
    }
}
