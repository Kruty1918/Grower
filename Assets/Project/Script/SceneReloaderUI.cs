using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Grower
{
    public class SceneReloaderUI : MonoBehaviour, ISceneReloaderUI
    {
        [SerializeField] private CanvasGroup canvasGroup; // Для управління прозорістю
        [SerializeField] private float transitionInDuration;
        [SerializeField] private float transitionOutDuration;

        private Image backgroundImage;

        public float TransitionInDuration => transitionInDuration;
        public float TransitionOutDuration => transitionOutDuration;

        private void Start()
        {
            if (canvasGroup == null)
                canvasGroup = GetComponent<CanvasGroup>();

            backgroundImage = canvasGroup.GetComponent<Image>();
            canvasGroup.alpha = 0f; // Початкова прозорість
            canvasGroup.gameObject.SetActive(true);
            backgroundImage.raycastTarget = false;
        }

        public void TransitionIn()
        {
            StartCoroutine(FadeIn());
        }

        public void TransitionOut()
        {
            StartCoroutine(FadeOut());
        }

        private IEnumerator FadeIn()
        {
            // Включаємо затемнення
            canvasGroup.gameObject.SetActive(true);
            backgroundImage.raycastTarget = true;

            float elapsedTime = 0f;
            while (elapsedTime < transitionInDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / transitionInDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = 1f;
        }

        private IEnumerator FadeOut()
        {
            float elapsedTime = 0f;
            while (elapsedTime < transitionOutDuration)
            {
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / transitionOutDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Вимикаємо затемнення після проявлення
            canvasGroup.alpha = 0f;
            backgroundImage.raycastTarget = false;
        }
    }
}
