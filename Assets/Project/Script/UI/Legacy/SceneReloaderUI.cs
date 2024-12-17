using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Grower
{
    /// <summary>
    /// UI component for handling scene transition effects, including fade-in and fade-out.
    /// </summary>
    public class SceneReloaderUI : MonoBehaviour, ISceneReloaderUI
    {
        [SerializeField] private CanvasGroup canvasGroup; // For controlling transparency
        [SerializeField] private float transitionInDuration; // Duration for the fade-in effect
        [SerializeField] private float transitionOutDuration; // Duration for the fade-out effect

        private Image backgroundImage;

        public float TransitionInDuration => transitionInDuration;
        public float TransitionOutDuration => transitionOutDuration;

        private void Start()
        {
            // Ensure that the CanvasGroup and Image components are correctly assigned
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();

            backgroundImage = canvasGroup.GetComponent<Image>();

            // Set the initial transparency to 0 (fully transparent)
            canvasGroup.alpha = 0f;
            canvasGroup.gameObject.SetActive(true); // Make sure the UI element is active
            backgroundImage.raycastTarget = false; // Disable raycasting to avoid UI interaction during the transition
        }

        /// <summary>
        /// Starts the transition-in effect.
        /// </summary>
        public void TransitionIn()
        {
            StartCoroutine(FadeIn());
        }

        /// <summary>
        /// Starts the transition-out effect.
        /// </summary>
        public void TransitionOut()
        {
            StartCoroutine(FadeOut());
        }

        /// <summary>
        /// Fades the UI in over the specified duration.
        /// </summary>
        private IEnumerator FadeIn()
        {
            // Enable the canvas and allow interaction
            canvasGroup.gameObject.SetActive(true);
            backgroundImage.raycastTarget = true;

            float elapsedTime = 0f;

            // Gradually increase alpha from 0 to 1 (fully visible)
            while (elapsedTime < transitionInDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / transitionInDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the alpha reaches 1 after the transition
            canvasGroup.alpha = 1f;
        }

        /// <summary>
        /// Fades the UI out over the specified duration.
        /// </summary>
        private IEnumerator FadeOut()
        {
            float elapsedTime = 0f;

            // Gradually decrease alpha from 1 to 0 (fully transparent)
            while (elapsedTime < transitionOutDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / transitionOutDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Ensure the alpha reaches 0 after the transition
            canvasGroup.alpha = 0f;

            // Disable raycasting and hide the UI element after the transition
            backgroundImage.raycastTarget = false;
        }
    }
}