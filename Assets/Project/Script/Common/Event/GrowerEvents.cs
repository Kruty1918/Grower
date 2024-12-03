using UnityEngine.Events;
using static Grower.LevelResult;

namespace Grower
{
    public static class GrowerEvents
    {
        public static UnityEvent<LevelResult> OnLevelEnd = new UnityEvent<LevelResult>();
        public static UnityEvent<CollisionData> OnHeadCollision = new UnityEvent<CollisionData>();
    }
}