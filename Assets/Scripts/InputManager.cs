using System.Collections.Generic;
using UnityEngine;

namespace Grower
{
    public interface IInputListener
    {
        void OnSwipe(Vector2 direction);
    }

    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputStrategy inputStrategy;

        private IInputProcessor inputProcessor;
        private List<IInputListener> listeners = new List<IInputListener>();

        public void AddListener(IInputListener listener)
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        public void RemoveListener(IInputListener listener)
        {
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }

        private void Awake()
        {
            if (inputStrategy != null)
            {
                inputProcessor = inputStrategy.GetInputProcessor();
            }
            else
            {
                Debug.LogError("Input Strategy is not set!");
            }
        }

        private void Update()
        {
            if (inputProcessor == null) return;

            Vector2 direction = inputProcessor.GetInputDirection();
            if (direction != Vector2.zero)
            {
                InvokeInput(direction);
            }
        }

        private void InvokeInput(Vector2 direction)
        {
            foreach (var listener in listeners)
            {
                listener.OnSwipe(direction);
            }
        }
    }
}