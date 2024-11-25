using UnityEngine;
using UnityEngine.InputSystem;

namespace Grower
{
    public class InputManager : MonoBehaviour, ISwipeListener
    {
        private PlayerControls playerControls;
        private Camera mainCamera;

        private InputHandler inputHandler;

        private void Awake()
        {
            playerControls = new PlayerControls();
            mainCamera = Camera.main;

            // Ініціалізуємо SwipeInputHandler через InputHandler
            inputHandler = new SwipeInputHandler(0.2f, 1f, 0.9f);
            if (inputHandler is SwipeInputHandler swipeInputHandler)
            {
                swipeInputHandler.OnSwipe += OnSwipe;
            }
        }

        private void OnEnable()
        {
            playerControls.Enable();
            playerControls.Touch.PrimaryContact.started += StartInput;
            playerControls.Touch.PrimaryContact.canceled += EndInput;
        }

        private void OnDisable()
        {
            playerControls.Disable();
            playerControls.Touch.PrimaryContact.started -= StartInput;
            playerControls.Touch.PrimaryContact.canceled -= EndInput;
        }

        private void StartInput(InputAction.CallbackContext context)
        {
            Vector2 position = ScreenToWorld(mainCamera, playerControls.Touch.PrimaryPosition.ReadValue<Vector2>());
            inputHandler.HandleInputStart(position, (float)context.startTime);
        }

        private void EndInput(InputAction.CallbackContext context)
        {
            Vector2 position = ScreenToWorld(mainCamera, playerControls.Touch.PrimaryPosition.ReadValue<Vector2>());
            inputHandler.HandleInputEnd(position, (float)context.time);
        }

        public void OnSwipe(SwipeType type, Vector2 direction)
        {
            Debug.Log($"Swipe detected: {type}, Direction: {direction}");
        }

        public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
        {
            position.z = camera.nearClipPlane;
            return camera.ScreenToWorldPoint(position);
        }
    }
}