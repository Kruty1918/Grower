using UnityEngine;

namespace Grower
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputStrategy inputStrategy;

        private IInputProcessor inputProcessor;

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
                HandleMovement(direction);
            }
        }

        private void HandleMovement(Vector2 direction)
        {
            Debug.Log($"Direction: {direction.ToCardinalDirection()}");
        }
    }
}