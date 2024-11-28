using UnityEngine;
using SGS29.Utilities;

namespace Grower
{
    public class BodyAnimation : IBodyAnimation
    {
        public void AnimateBodySegmentSpawn(Vector3 targetPosition, Cell bodyPrefab, float gridSize)
        {
            Vector2Int cellCoords = new Vector2Int(
                GridUtility.AlignAxisAsInt(targetPosition.x, gridSize),
                GridUtility.AlignAxisAsInt(targetPosition.z, gridSize)
            );

            if (SM.Instance<Grid>().ContainsCell(cellCoords))
            {
                return;
            }

            Cell segment = Object.Instantiate(bodyPrefab, targetPosition, Quaternion.identity);
            segment.PushCell();
        }
    }
}
