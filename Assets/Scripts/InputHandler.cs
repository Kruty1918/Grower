using UnityEngine;

namespace Grower
{
    public abstract class InputHandler
    {
        event System.Action<CardinalDirection, Vector2> OnSwipe;
        public abstract void HandleInputStart(Vector2 position, float time);
        public abstract void HandleInputEnd(Vector2 position, float time);
    }

    public enum CardinalDirection
    {
        None,
        Up,
        Down,
        Left,
        Right
    }

    public interface ISwipeListener
    {
        void OnSwipe(CardinalDirection type, Vector2 direction);
    }

}