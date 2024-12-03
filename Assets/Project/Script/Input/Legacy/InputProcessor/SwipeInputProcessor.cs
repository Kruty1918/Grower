using UnityEngine;
using UnityEngine.InputSystem;

namespace Grower
{
    /// <summary>
    /// Processes swipe input and provides directional output.
    /// </summary>
    public class SwipeInputProcessor : IInputProcessor
    {
        #region Fields

        private readonly SwipeInputHandler swipeHandler;
        private Vector2 lastDirection;
        private bool swipeCompleted; // Flag to indicate swipe completion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SwipeInputProcessor"/> class.
        /// </summary>
        /// <param name="minDist">Minimum swipe distance to recognize.</param>
        /// <param name="maxTime">Maximum time duration for a valid swipe.</param>
        /// <param name="dirThreshold">Directional threshold for swipe detection.</param>
        public SwipeInputProcessor(float minDist, float maxTime, float dirThreshold)
        {
            swipeHandler = new SwipeInputHandler(minDist, maxTime, dirThreshold);

            // Subscribe to the swipe event
            swipeHandler.OnSwipe += (type, direction) =>
            {
                lastDirection = direction;
                swipeCompleted = true; // Mark swipe as completed
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the direction of the last valid swipe input.
        /// </summary>
        /// <returns>Direction of the swipe as a <see cref="Vector2"/>. Returns zero vector if no swipe.</returns>
        public Vector2 GetInputDirection()
        {
            // Check for active touch input
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
                    swipeCompleted = false; // Return swipe direction only once
                    return lastDirection;
                }
            }

            return Vector2.zero; // No active swipe
        }

        #endregion
    }
}
