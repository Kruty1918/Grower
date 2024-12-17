using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Grower
{
    /// <summary>
    /// Handles the text animation that fades up, moves the text upwards, and fades it out.
    /// </summary>
    public class TextFadeUpAnimation : MonoBehaviour, IProcessObserver
    {
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [SerializeField] private float animationDuration = 1f; // Duration of the animation
        [SerializeField] private float moveDistanceY = 50f;     // Distance the text moves upwards
        [SerializeField] private float fadeOutDuration = 1f;   // Duration of the fade-out effect
        [SerializeField] private UnityEvent onAnimationComplete; // Event triggered after the animation completes

        private void Awake()
        {
            // If no TextMeshPro component is assigned, try to find it on the same object
            if (textMeshPro == null)
            {
                textMeshPro = GetComponent<TextMeshProUGUI>();
            }
        }

        private void OnEnable()
        {
            // Subscribe to the level restart event
            GrowerEvents.OnLevelRestart.AddListener(OnLevelRestart);
        }

        private void OnDisable()
        {
            // Unsubscribe from the level restart event when disabled
            GrowerEvents.OnLevelRestart.RemoveListener(OnLevelRestart);
        }

        /// <summary>
        /// Resets the animation when the level is restarted.
        /// </summary>
        private void OnLevelRestart()
        {
            // Disable the game object when the level is restarted
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Executes the animation when the process is executed.
        /// </summary>
        /// <param name="processID">The ID of the process.</param>
        /// <param name="args">Additional arguments passed to the process.</param>
        public void OnProcessExecuted(string processID, params object[] args)
        {
            // Ensure the text is fully visible
            textMeshPro.alpha = 1f;

            // Create a sequence for the text animation
            Sequence animationSequence = DOTween.Sequence();

            // Move the text upwards and fade it out
            animationSequence
                .Append(textMeshPro.transform.DOLocalMoveY(moveDistanceY, animationDuration).SetEase(Ease.OutSine)) // Move the text upwards
                .Join(textMeshPro.DOFade(0f, fadeOutDuration)) // Fade the text out
                .OnComplete(onAnimationComplete.Invoke); // Invoke the UnityEvent after the animation completes

            // Play the animation sequence
            animationSequence.Play();
        }
    }
}