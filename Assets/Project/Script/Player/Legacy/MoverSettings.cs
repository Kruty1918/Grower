using UnityEngine;

namespace Grower
{
    [CreateAssetMenu(fileName = "MoverSettings", menuName = "Grower/Player/DataSettings", order = 0)]
    public class MoverSettings : ScriptableObject
    {
        /// <summary>
        /// The mass of the object, which affects its collision behavior.
        /// A higher value results in a more significant impact force during collisions.
        /// </summary>
        [Tooltip("The mass of the object.")]
        public float objectMass = 1.0f;

        /// <summary>
        /// The size of the grid cells, which determines the grid's resolution.
        /// This value should match the grid's cell size for proper alignment.
        /// </summary>
        [Tooltip("The size of the grid cells.")]
        public float gridSize = 1f;

        /// <summary>
        /// The starting offset for grid alignment.
        /// This value offsets the starting position to ensure proper grid alignment.
        /// </summary>
        [Tooltip("The starting offset for grid alignment.")]
        public Vector3 gridOffset = Vector3.zero;
    }
}