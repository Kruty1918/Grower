using System.Collections.Generic;
using SGS29.Utilities;
using UnityEngine;

namespace Grower
{
    /// <summary>
    /// The InputManager class handles user input based on the assigned input strategy
    /// and notifies subscribed listeners about input events.
    /// </summary>
    public class InputManager : MonoSingleton<InputManager>
    {
        #region Fields

        [SerializeField]
        [Tooltip("Input strategy to define the source of input.")]
        private InputStrategy inputStrategy;

        private IInputProcessor inputProcessor;
        private List<IInputListener> listeners = new List<IInputListener>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a listener to the list of subscribers.
        /// </summary>
        /// <param name="listener">Listener implementing the IInputListener interface.</param>
        public void AddListener(IInputListener listener)
        {
            if (!listeners.Contains(listener))
                listeners.Add(listener);
        }

        /// <summary>
        /// Removes a listener from the list of subscribers.
        /// </summary>
        /// <param name="listener">Listener implementing the IInputListener interface.</param>
        public void RemoveListener(IInputListener listener)
        {
            if (listeners.Contains(listener))
                listeners.Remove(listener);
        }

        #endregion

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();

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

        #endregion

        #region Private Methods

        /// <summary>
        /// Notifies all subscribed listeners about the input direction.
        /// </summary>
        /// <param name="direction">Direction vector of the input.</param>
        private void InvokeInput(Vector2 direction)
        {
            foreach (var listener in listeners)
            {
                listener.OnSwipe(direction);
            }
        }

        #endregion
    }
}