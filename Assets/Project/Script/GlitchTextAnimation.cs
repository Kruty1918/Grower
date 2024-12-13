using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Grower
{
    public class GlitchTextAnimation : MonoBehaviour, IProcessObserver
    {
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [SerializeField] private float glitchDuration = 1f;   // Тривалість анімації
        [SerializeField] private float moveDistance = 50f;    // Відстань руху тексту вниз
        [SerializeField] private float fadeOutDuration = 1f;  // Тривалість зникання (прозорості)
        [SerializeField] private UnityEvent onAnimationComplete;

        private void Awake()
        {
            if (textMeshPro == null)
            {
                textMeshPro = GetComponent<TextMeshProUGUI>(); // Якщо не вказано, пробуємо знайти компонент на тому ж об'єкті
            }
        }

        public void OnProcessExecuted(string processID, params object[] args)
        {
            // Не скидаємо позицію, а просто залишаємо поточну.
            textMeshPro.alpha = 1f;  // Переконуємось, що текст видимий
                                     // textMeshPro.transform.localPosition = Vector3.zero; // Не потрібно це!

            // Глітч-ефект: рухаємо текст вниз та зменшуємо прозорість
            Sequence glitchSequence = DOTween.Sequence();

            glitchSequence
                .Append(textMeshPro.transform.DOLocalMoveY(-moveDistance, glitchDuration).SetEase(Ease.InSine)) // Рух тексту вниз
                .Join(textMeshPro.DOFade(0f, fadeOutDuration))
                .OnComplete(onAnimationComplete.Invoke); // Відповідно текст поступово зникає

            // Можна додати випадковий глітч-ефект для більш піксельного вигляду, якщо хочете
            glitchSequence.AppendCallback(() =>
            {
                // Можна додати зміну стилю або інші ефекти для глітчу
                // Наприклад, випадкові зміни шрифта чи розміру тексту:
                textMeshPro.fontSize = Random.Range(30, 50);
            });

            // Запуск анімації
            glitchSequence.Play();
        }
    }
}