using UnityEngine;

namespace Grower
{
    public class SwipeInputHandler : InputHandler
    {
        private readonly float minimumDistance;
        private readonly float maximumTime;
        private readonly float directionThreshold;
        private Vector2 startPosition;
        private float startTime;

        public event System.Action<SwipeType, Vector2> OnSwipe;

        public SwipeInputHandler(float minDist, float maxTime, float dirThreshold)
        {
            minimumDistance = minDist;
            maximumTime = maxTime;
            directionThreshold = dirThreshold;
        }

        public override void HandleInputStart(Vector2 position, float time)
        {
            startPosition = position;
            startTime = time;
        }

        public override void HandleInputEnd(Vector2 position, float time)
        {
            if (IsValidSwipe(position, time))
            {
                Vector2 direction = (position - startPosition).normalized;
                DetectSwipeDirection(direction);
            }
        }

        private bool IsValidSwipe(Vector2 position, float time)
        {
            float distance = Vector2.Distance(startPosition, position);
            float duration = time - startTime;

            return distance >= minimumDistance && duration <= maximumTime;
        }

        private void DetectSwipeDirection(Vector2 direction)
        {
            if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
                OnSwipe?.Invoke(SwipeType.Up, direction);
            else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
                OnSwipe?.Invoke(SwipeType.Down, direction);
            else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
                OnSwipe?.Invoke(SwipeType.Left, direction);
            else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
                OnSwipe?.Invoke(SwipeType.Right, direction);
        }
    }
}
