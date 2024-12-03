using UnityEngine;

namespace Grower
{
    [CreateAssetMenu(fileName = "KeyboardInputStrategy", menuName = "Grower/Input Strategy/Keyboard")]
    public class KeyboardInputStrategy : InputStrategy
    {
        public override IInputProcessor GetInputProcessor()
        {
            return new KeyboardInputProcessor();
        }
    }
}