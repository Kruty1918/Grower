using System.Collections;
using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Abstract class for implementing different camera movement strategies.
    /// This class defines the general settings for the camera impulse effect and the necessary coroutine
    /// for handling the camera movement in response to an impulse. It is meant to be extended by concrete 
    /// strategies that define specific camera movements during impulses.
    /// </summary>
    public abstract class CameraMovementStrategy : ScriptableObject
    {
        [Header("Impulse Settings")]

        /// <summary>
        /// The base strength of the camera impulse effect, which influences how strongly the camera will move.
        /// </summary>
        [Tooltip("Base strength of the camera impulse effect.")]
        public float baseImpulseStrength = 0.5f;

        /// <summary>
        /// The duration for which the camera impulse effect lasts before the camera returns to its original position.
        /// </summary>
        [Tooltip("Duration of the camera impulse effect before returning to the original position.")]
        public float cameraImpulseDuration = 0.3f;

        /// <summary>
        /// The speed at which the camera returns to its original position after the impulse effect.
        /// </summary>
        [Tooltip("Speed at which the camera returns to its original position.")]
        public float cameraReturnSpeed = 5f;

        /// <summary>
        /// An abstract method to implement in derived classes. This method defines the coroutine that handles the camera
        /// impulse movement, including its duration and return to the original position.
        /// </summary>
        /// <param name="cameraImpulse">The CameraImpulse component that triggered the impulse.</param>
        /// <param name="collisionNormal">The normal vector of the collision, indicating the direction of the impulse.</param>
        /// <param name="forceMultiplier">A multiplier based on the collision force, affecting the strength of the impulse.</param>
        /// <returns>A coroutine for managing the camera impulse effect.</returns>
        public abstract IEnumerator CameraImpulseCoroutine(CameraImpulse cameraImpulse, Vector3 collisionNormal, float forceMultiplier);
    }
}