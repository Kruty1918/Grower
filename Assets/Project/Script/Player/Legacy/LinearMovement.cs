using UnityEngine;
using System;

namespace Grower
{
    [CreateAssetMenu(fileName = "LinearMovement", menuName = "Grower/Player/Movement/Linear", order = 0)]
    public class LinearMovement : MovementStrategyBase
    {
        public override void Move(Transform transform, Vector3 target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.fixedDeltaTime);
        }
    }
}