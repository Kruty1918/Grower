using UnityEngine;
using UnityEngine.InputSystem;

namespace Grower
{
    public class KeyboardInputProcessor : IInputProcessor
    {
        public Vector2 GetInputDirection()
        {
            float x = 0;
            if (Keyboard.current.aKey.isPressed) x -= 1;
            if (Keyboard.current.dKey.isPressed) x += 1;

            float y = 0;
            if (Keyboard.current.wKey.isPressed) y += 1;
            if (Keyboard.current.sKey.isPressed) y -= 1;

            return new Vector2(x, y).normalized;
        }
    }
}