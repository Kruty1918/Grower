using UnityEngine;
using System;

namespace Grower
{
    /// <summary>
    /// The base class for movement strategies.
    /// This class defines the common properties and methods for all movement strategies.
    /// </summary>
    public abstract class MovementStrategyBase : ScriptableObject
    {
        /// <summary>
        /// The speed at which the object moves.
        /// This value determines how fast the object moves across the grid.
        /// </summary>
        [Tooltip("The speed at which the object moves.")]
        [Range(1f, 20f)]
        [SerializeField]
        protected float moveSpeed = 5f;

        /// <summary>
        /// Abstract method to move the object towards a target position.
        /// Derived classes must implement this method.
        /// </summary>
        /// <param name="transform">The transform of the object to move.</param>
        /// <param name="target">The target position to move towards.</param>
        public abstract void Move(Transform transform, Vector3 target);
    }
}