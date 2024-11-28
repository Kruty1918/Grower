using UnityEngine;

namespace Grower
{
    public interface IBodyAnimation
    {
        void AnimateBodySegmentSpawn(Vector3 targetPosition, Cell bodyPrefab, float gridSize);
    }
}
