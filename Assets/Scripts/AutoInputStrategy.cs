using UnityEngine;

namespace Grower
{
    [CreateAssetMenu(fileName = "AutoInputStrategy", menuName = "Grower/Input Strategy/Auto")]
    public class AutoInputStrategy : InputStrategy
    {
        public override IInputProcessor GetInputProcessor()
        {
#if UNITY_EDITOR
            // У редакторі використовуємо клавіатуру
            return new KeyboardInputProcessor();
#elif UNITY_ANDROID || UNITY_IOS
        // Для мобільних платформ використовуємо SwipeInputProcessor
        return new SwipeInputProcessor(0.2f, 1f, 0.9f);
#else
            // Для інших платформ за замовчуванням використовуємо клавіатуру
            return new KeyboardInputProcessor();
#endif
        }
    }
}