using UnityEngine;

namespace Grower
{
    [CreateAssetMenu(fileName = "InputStrategy", menuName = "Grower/Input Strategy")]
    public abstract class InputStrategy : ScriptableObject
    {
        public abstract IInputProcessor GetInputProcessor();
    }
}