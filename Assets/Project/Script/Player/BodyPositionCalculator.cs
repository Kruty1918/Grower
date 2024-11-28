using UnityEngine;

namespace Grower
{
    public class BodyPositionCalculator : IBodyPositionCalculator
    {
        public Vector3 CalculateAlignedPosition(Vector3 position, float gridSize, Vector3 offset)
        {
            return GridUtility.AlignToGrid(position, gridSize, offset);
        }

        public bool HasHeadMoved(Vector3 currentGridPosition, Vector3 lastGridPosition)
        {
            return currentGridPosition != lastGridPosition;
        }
    }
}
