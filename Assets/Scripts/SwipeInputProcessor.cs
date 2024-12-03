using UnityEngine;
using UnityEngine.InputSystem;

namespace Grower
{
    public class SwipeInputProcessor : IInputProcessor
    {
        private readonly SwipeInputHandler swipeHandler;
        private Vector2 lastDirection;
        private bool swipeCompleted; // Прапорець завершення свайпу

        public SwipeInputProcessor(float minDist, float maxTime, float dirThreshold)
        {
            swipeHandler = new SwipeInputHandler(minDist, maxTime, dirThreshold);

            // Підписуємося на подію свайпу
            swipeHandler.OnSwipe += (type, direction) =>
            {
                lastDirection = direction;
                swipeCompleted = true; // Позначаємо, що свайп завершено
            };
        }

        public Vector2 GetInputDirection()
        {
            // Перевірка, чи є активний дотик
            if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                Vector2 position = Touchscreen.current.primaryTouch.position.ReadValue();
                float time = Time.time;
                swipeHandler.HandleInputStart(position, time);
            }
            else if (Touchscreen.current.primaryTouch.press.wasReleasedThisFrame)
            {
                Vector2 position = Touchscreen.current.primaryTouch.position.ReadValue();
                float time = Time.time;
                swipeHandler.HandleInputEnd(position, time);

                if (swipeCompleted)
                {
                    swipeCompleted = false; // Повертаємо свайп тільки один раз
                    return lastDirection;
                }
            }

            return Vector2.zero; // Немає активного свайпу
        }
    }
}