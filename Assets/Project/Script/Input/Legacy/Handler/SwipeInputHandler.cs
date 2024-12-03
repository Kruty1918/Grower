using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Handles swipe input detection and triggers corresponding events.
    /// </summary>
    public class SwipeInputHandler : InputHandler
    {
        #region Fields

        private readonly float minimumDistance;
        private readonly float maximumTime;
        private readonly float directionThreshold;
        private Vector2 startPosition;
        private float startTime;

        #endregion

        #region Events

        /// <summary>
        /// Event triggered when a valid swipe is detected.
        /// </summary>
        /// <param name="direction">The cardinal direction of the swipe.</param>
        /// <param name="vector">The swipe direction as a normalized <see cref="Vector2"/>.</param>
        public event System.Action<CardinalDirection, Vector2> OnSwipe;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SwipeInputHandler"/> class.
        /// </summary>
        /// <param name="minDist">Minimum distance for a valid swipe.</param>
        /// <param name="maxTime">Maximum time duration for a valid swipe.</param>
        /// <param name="dirThreshold">Threshold for directional detection.</param>
        public SwipeInputHandler(float minDist, float maxTime, float dirThreshold)
        {
            minimumDistance = minDist;
            maximumTime = maxTime;
            directionThreshold = dirThreshold;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the start of an input interaction.
        /// </summary>
        /// <param name="position">The start position of the touch input.</param>
        /// <param name="time">The start time of the touch input.</param>
        public override void HandleInputStart(Vector2 position, float time)
        {
            startPosition = position;
            startTime = time;
        }

        /// <summary>
        /// Handles the end of an input interaction.
        /// </summary>
        /// <param name="position">The end position of the touch input.</param>
        /// <param name="time">The end time of the touch input.</param>
        public override void HandleInputEnd(Vector2 position, float time)
        {
            if (IsValidSwipe(position, time))
            {
                Vector2 direction = (position - startPosition).normalized;
                DetectSwipeDirection(direction);
            }
        }

        /// <summary>
        /// Validates if the swipe meets the required distance and time criteria.
        /// </summary>
        /// <param name="position">The end position of the swipe.</param>
        /// <param name="time">The end time of the swipe.</param>
        /// <returns>True if the swipe is valid; otherwise, false.</returns>
        private bool IsValidSwipe(Vector2 position, float time)
        {
            float distance = Vector2.Distance(startPosition, position);
            float duration = time - startTime;

            return distance >= minimumDistance && duration <= maximumTime;
        }

        /// <summary>
        /// Detects the cardinal direction of a swipe based on the input vector.
        /// </summary>
        /// <param name="direction">The swipe direction as a normalized <see cref="Vector2"/>.</param>
        private void DetectSwipeDirection(Vector2 direction)
        {
            // Enhanced direction detection using threshold
            if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
            {
                OnSwipe?.Invoke(CardinalDirection.Up, direction);
            }
            else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
            {
                OnSwipe?.Invoke(CardinalDirection.Down, direction);
            }
            else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
            {
                OnSwipe?.Invoke(CardinalDirection.Left, direction);
            }
            else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
            {
                OnSwipe?.Invoke(CardinalDirection.Right, direction);
            }
            else
            {
                Debug.Log("No significant swipe detected");
            }
        }

        #endregion
    }
}