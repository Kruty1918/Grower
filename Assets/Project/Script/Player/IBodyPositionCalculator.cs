using UnityEngine;

namespace Grower
{
    public interface IBodyPositionCalculator
    {
        Vector3 CalculateAlignedPosition(Vector3 position, float gridSize, Vector3 offset);
        bool HasHeadMoved(Vector3 currentGridPosition, Vector3 lastGridPosition);
    }
}
