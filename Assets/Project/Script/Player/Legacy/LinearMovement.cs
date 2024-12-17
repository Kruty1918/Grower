using UnityEngine;
using System;

namespace Grower
{
    /// <summary>
    /// A movement strategy that moves the object linearly towards a target position.
    /// </summary>
    [CreateAssetMenu(fileName = "LinearMovement", menuName = "Grower/Player/Movement/Linear", order = 0)]
    public class LinearMovement : MovementStrategyBase
    {
        /// <summary>
        /// Moves the object linearly towards the target position.
        /// </summary>
        /// <param name="transform">The transform of the object to move.</param>
        /// <param name="target">The target position to move towards.</param>
        public override void Move(Transform transform, Vector3 target)
        {
            // Move the object towards the target at a constant speed, adjusting based on fixed time.
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.fixedDeltaTime);
        }
    }
}