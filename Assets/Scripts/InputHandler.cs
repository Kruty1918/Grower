using UnityEngine;

namespace Grower
{
    public abstract class InputHandler
    {
        event System.Action<SwipeType, Vector2> OnSwipe;
        public abstract void HandleInputStart(Vector2 position, float time);
        public abstract void HandleInputEnd(Vector2 position, float time);
    }

    public enum SwipeType
    {
        Up,
        Down,
        Left,
        Right
    }

    public interface ISwipeListener
    {
        void OnSwipe(SwipeType type, Vector2 direction);
    }

}