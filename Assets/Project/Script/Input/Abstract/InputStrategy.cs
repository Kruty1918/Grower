using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Abstract class representing an input strategy for handling input processing.
    /// </summary>
    [CreateAssetMenu(fileName = "InputStrategy", menuName = "Grower/Input Strategy")]
    public abstract class InputStrategy : ScriptableObject
    {
        #region Methods

        /// <summary>
        /// Returns the appropriate input processor based on the strategy implementation.
        /// </summary>
        /// <returns>An instance of <see cref="IInputProcessor"/> for processing input.</returns>
        public abstract IInputProcessor GetInputProcessor();

        #endregion
    }
}
