using UnityEngine;

namespace Grower
{
    [CreateAssetMenu(fileName = "AcceleratedMovement", menuName = "Grower/Player/Movement/Accelerated", order = 1)]
    /// <summary>
    /// Movement strategy that accelerates the player towards a target with smooth acceleration and deceleration.
    /// </summary>
    public class AcceleratedMovement : MovementStrategyBase
    {
        [Header("Acceleration Settings")]
        [Tooltip("Maximum movement speed.")]
        [SerializeField] private float maxSpeed = 5f;

        [Tooltip("Time to reach maximum speed.")]
        [SerializeField] private float accelerationTime = 1f;

        [Tooltip("Time to decelerate to zero speed.")]
        [SerializeField] private float decelerationTime = 1f;

        private float currentSpeed = 0f;
        private Vector3 previousTarget;

        /// <summary>
        /// Moves the object towards the target with acceleration and deceleration.
        /// </summary>
        /// <param name="transform">The transform of the object to move.</param>
        /// <param name="target">The target position to move towards.</param>
        public override void Move(Transform transform, Vector3 target)
        {
            if (target != previousTarget)
            {
                // Reset speed when target changes
                previousTarget = target;
                currentSpeed = 0f;
            }

            // Calculate distance to target
            float distanceToTarget = Vector3.Distance(transform.position, target);

            // Accelerate if the object hasn't reached the target
            if (distanceToTarget > 0.1f)
            {
                currentSpeed += maxSpeed / accelerationTime * Time.fixedDeltaTime;
                currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
            }
            else // Decelerate upon reaching the target
            {
                currentSpeed -= maxSpeed / decelerationTime * Time.fixedDeltaTime;
                currentSpeed = Mathf.Max(currentSpeed, 0f);
            }

            // Move towards the target
            transform.position = Vector3.MoveTowards(transform.position, target, currentSpeed * Time.fixedDeltaTime);
        }
    }
}