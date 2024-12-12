using UnityEngine;
using UnityEngine.InputSystem;

namespace Grower
{
    /// <summary>
    /// Processes keyboard input for directional movement.
    /// </summary>
    public class KeyboardInputProcessor : IInputProcessor
    {
        #region Methods

        /// <summary>
        /// Gets the direction of the movement based on keyboard input.
        /// </summary>
        /// <returns>A normalized <see cref="Vector2"/> representing the direction.</returns>
        public Vector2 GetInputDirection()
        {
            float x = 0;

            // Check for horizontal input (A/D keys)
            if (Keyboard.current.aKey.isPressed) x -= 1;
            if (Keyboard.current.dKey.isPressed) x += 1;

            float y = 0;

            // Check for vertical input (W/S keys)
            if (Keyboard.current.wKey.isPressed) y += 1;
            if (Keyboard.current.sKey.isPressed) y -= 1;

            return new Vector2(x, y).normalized; // Return normalized direction vector
        }

        #endregion
    }
}