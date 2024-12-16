using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Grower
{
    public class TextFadeUpAnimation : MonoBehaviour, IProcessObserver
    {
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [SerializeField] private float animationDuration = 1f; // Тривалість анімації
        [SerializeField] private float moveDistanceY = 50f;     // Відстань руху тексту вгору
        [SerializeField] private float fadeOutDuration = 1f;   // Тривалість зникання (прозорості)
        [SerializeField] private UnityEvent onAnimationComplete;

        private void Awake()
        {
            if (textMeshPro == null)
            {
                textMeshPro = GetComponent<TextMeshProUGUI>(); // Якщо не вказано, пробуємо знайти компонент на тому ж об'єкті
            }
        }

        void OnEnable()
        {
            GrowerEvents.OnLevelRestart.AddListener(OnLevelRestart);
        }

        private void OnLevelRestart()
        {
            GrowerEvents.OnLevelRestart.RemoveListener(OnLevelRestart);
            gameObject.SetActive(false);
        }

        public void OnProcessExecuted(string processID, params object[] args)
        {
            // Переконуємось, що текст видимий
            textMeshPro.alpha = 1f;

            // Створюємо анімацію руху тексту вгору із зниканням
            Sequence animationSequence = DOTween.Sequence();

            animationSequence
                .Append(textMeshPro.transform.DOLocalMoveY(moveDistanceY, animationDuration).SetEase(Ease.OutSine)) // Рух тексту вгору
                .Join(textMeshPro.DOFade(0f, fadeOutDuration)) // Прозорість
                .OnComplete(onAnimationComplete.Invoke); // Викликати UnityEvent після завершення

            // Запуск анімації
            animationSequence.Play();
        }
    }
}