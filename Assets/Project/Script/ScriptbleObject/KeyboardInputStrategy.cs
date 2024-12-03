using UnityEngine;

namespace Grower
{
    /// <summary>
    /// A strategy for handling input using the keyboard.
    /// </summary>
    [CreateAssetMenu(fileName = "KeyboardInputStrategy", menuName = "Grower/Input Strategy/Keyboard")]
    public class KeyboardInputStrategy : InputStrategy
    {
        #region Methods

        /// <summary>
        /// Returns the keyboard input processor.
        /// </summary>
        /// <returns>An instance of <see cref="KeyboardInputProcessor"/> for processing keyboard input.</returns>
        public override IInputProcessor GetInputProcessor()
        {
            return new KeyboardInputProcessor();
        }

        #endregion
    }
}