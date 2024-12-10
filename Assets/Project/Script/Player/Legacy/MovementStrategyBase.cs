using UnityEngine;
using System;

namespace Grower
{
    public abstract class MovementStrategyBase : ScriptableObject
    {
        /// <summary>
        /// The speed at which the object moves.
        /// This value determines how fast the object moves across the grid.
        /// </summary>
        [Tooltip("The speed at which the object moves.")]
        [Range(1f, 20f)]
        [SerializeField] protected float moveSpeed = 5f;

        public abstract void Move(Transform transform, Vector3 target);
    }
}