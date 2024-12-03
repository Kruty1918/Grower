using UnityEngine;

namespace Grower
{
    /// <summary>
    /// A strategy for automatic input handling based on the platform.
    /// </summary>
    [CreateAssetMenu(fileName = "AutoInputStrategy", menuName = "Grower/Input Strategy/Auto")]
    public class AutoInputStrategy : InputStrategy
    {
        #region Methods

        /// <summary>
        /// Returns the appropriate input processor based on the platform.
        /// </summary>
        /// <returns>An instance of <see cref="IInputProcessor"/> corresponding to the platform.</returns>
        public override IInputProcessor GetInputProcessor()
        {
#if UNITY_EDITOR
            // In the editor, use the keyboard input processor.
            return new KeyboardInputProcessor();
#elif UNITY_ANDROID || UNITY_IOS
            // For mobile platforms, use the swipe input processor.
            return new SwipeInputProcessor(0.2f, 1f, 0.9f);
#else
            // For other platforms, default to the keyboard input processor.
            return new KeyboardInputProcessor();
#endif
        }

        #endregion
    }
}