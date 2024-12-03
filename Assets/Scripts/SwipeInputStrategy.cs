using UnityEngine;

namespace Grower
{
    [CreateAssetMenu(fileName = "SwipeInputStrategy", menuName = "Grower/Input Strategy/Swipe")]
    public class SwipeInputStrategy : InputStrategy
    {
        [SerializeField] private float minDist = 0.2f;
        [SerializeField] private float maxTime = 1f;
        [SerializeField] private float dirThreshold = 0.9f;

        public override IInputProcessor GetInputProcessor()
        {
            return new SwipeInputProcessor(minDist, maxTime, dirThreshold);
        }
    }
}