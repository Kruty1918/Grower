using UnityEngine;
using UnityEngine.InputSystem;

namespace Grower
{
    public interface IInputProcessor
    {
        Vector2 GetInputDirection();
    }

    public class InputManager : MonoBehaviour
    {
        private IInputProcessor inputProcessor;
        private PlayerControls playerControls;

        private void Awake()
        {
            playerControls = new PlayerControls();

            // Вибір обробника введення залежно від платформи або середовища
#if UNITY_EDITOR
            inputProcessor = new KeyboardInputProcessor();
#elif UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR && UNITY_SIMULATOR
            inputProcessor = new SwipeInputProcessor(0.2f, 1f, 0.9f);
#else
            inputProcessor = new KeyboardInputProcessor();
#endif
        }

        private void Update()
        {
            Vector2 direction = inputProcessor.GetInputDirection();
            if (direction != Vector2.zero)
            {
                Debug.Log($"Input direction: {direction}");
                HandleMovement(direction);
            }
        }

        private void HandleMovement(Vector2 direction)
        {
            // Обробка руху або іншої дії
        }
    }

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

    public class SwipeInputProcessor : IInputProcessor
    {
        private readonly SwipeInputHandler swipeHandler;
        private Vector2 lastDirection;

        public SwipeInputProcessor(float minDist, float maxTime, float dirThreshold)
        {
            swipeHandler = new SwipeInputHandler(minDist, maxTime, dirThreshold);
            swipeHandler.OnSwipe += (type, direction) => lastDirection = direction;
        }

        public Vector2 GetInputDirection()
        {
            Vector2 direction = lastDirection;
            lastDirection = Vector2.zero; // Скидаємо після використання
            return direction;
        }
    }
}