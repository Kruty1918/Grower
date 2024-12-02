using UnityEngine;
using UnityEngine.InputSystem;

namespace Grower
{
    public class SwipeInputProcessor : IInputProcessor
    {
        private readonly SwipeInputHandler swipeHandler;
        private Vector2 lastDirection;

        public SwipeInputProcessor(float minDist, float maxTime, float dirThreshold)
        {
            swipeHandler = new SwipeInputHandler(minDist, maxTime, dirThreshold);

            // Підписуємося на подію свайпу
            swipeHandler.OnSwipe += (type, direction) => lastDirection = direction;
        }

        public Vector2 GetInputDirection()
        {
            // Перевірка на мобільному пристрої або в редакторі на миші
            if (Application.isMobilePlatform)
            {
                // Якщо є активний дотик
                if (Touchscreen.current.primaryTouch.press.isPressed)
                {
                    Vector2 position = Touchscreen.current.primaryTouch.position.ReadValue();
                    float time = Time.time;
                    swipeHandler.HandleInputStart(position, time);
                }
                else
                {
                    Vector2 position = Touchscreen.current.primaryTouch.position.ReadValue();
                    float time = Time.time;
                    swipeHandler.HandleInputEnd(position, time);
                }
            }
            else
            {
                // Для ПК обробляємо мишу
                if (Mouse.current.leftButton.isPressed)
                {
                    Vector2 position = Mouse.current.position.ReadValue();
                    float time = Time.time;
                    swipeHandler.HandleInputStart(position, time);
                }
                else
                {
                    Vector2 position = Mouse.current.position.ReadValue();
                    float time = Time.time;
                    swipeHandler.HandleInputEnd(position, time);
                }
            }

            // Повертаємо останній напрямок після обробки введення
            Vector2 direction = lastDirection;
            lastDirection = Vector2.zero; // Скидаємо після використання
            return direction;
        }
    }
}
