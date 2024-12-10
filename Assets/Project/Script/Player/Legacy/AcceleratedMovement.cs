using UnityEngine;

namespace Grower
{
    [CreateAssetMenu(fileName = "AcceleratedMovement", menuName = "Grower/Player/Movement/Accelerated", order = 1)]
    public class AcceleratedMovement : MovementStrategyBase
    {
        [Header("Acceleration Settings")]
        [Tooltip("Максимальна швидкість руху.")]
        [SerializeField] private float maxSpeed = 5f;

        [Tooltip("Час, за який досягається максимальна швидкість.")]
        [SerializeField] private float accelerationTime = 1f;

        [Tooltip("Час, за який швидкість падає до нуля при зупинці.")]
        [SerializeField] private float decelerationTime = 1f;

        private float currentSpeed = 0f;
        private Vector3 previousTarget;

        public override void Move(Transform transform, Vector3 target)
        {
            if (target != previousTarget)
            {
                // Перевірка на зміну цілі та скидання швидкості
                previousTarget = target;
                currentSpeed = 0f;
            }

            // Розрахунок відстані до цілі
            float distanceToTarget = Vector3.Distance(transform.position, target);

            // Прискорення, якщо об'єкт ще не досягнув цілі
            if (distanceToTarget > 0.1f)
            {
                currentSpeed += maxSpeed / accelerationTime * Time.fixedDeltaTime;
                currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
            }
            else // Сповільнення при досягненні цілі
            {
                currentSpeed -= maxSpeed / decelerationTime * Time.fixedDeltaTime;
                currentSpeed = Mathf.Max(currentSpeed, 0f);
            }

            // Рух до цілі
            transform.position = Vector3.MoveTowards(transform.position, target, currentSpeed * Time.fixedDeltaTime);
        }
    }
}