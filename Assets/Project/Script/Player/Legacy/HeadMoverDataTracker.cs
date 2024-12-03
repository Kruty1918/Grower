using Sirenix.OdinInspector;
using UnityEngine;
using static Grower.LevelResult;

namespace Grower
{
    public class HeadMoverDataTracker : MonoBehaviour
    {
        private HeadMover headMover;

        // Підписка на події
        private void OnEnable()
        {
            GrowerEvents.OnLevelEnd.AddListener(OnLevelEnd);
            GrowerEvents.OnHeadCollision.AddListener(OnHeadCollision);
        }

        private void OnDisable()
        {
            GrowerEvents.OnLevelEnd.RemoveListener(OnLevelEnd);
            GrowerEvents.OnHeadCollision.RemoveListener(OnHeadCollision);
        }

        // Поля для зберігання даних
        [FoldoutGroup("Head Mover Data", true)]
        [Title("Movement Data")] // Виправлення: без TitleAlignments
        [Space(10)]
        [ReadOnly]
        public string currentDirection;

        [FoldoutGroup("Head Mover Data")]
        [ReadOnly]
        public Vector3 currentPosition;

        [FoldoutGroup("Head Mover Data")]
        [ReadOnly]
        public Vector3 targetPosition;

        [FoldoutGroup("Head Mover Data")]
        [ReadOnly]
        public float currentSpeed;

        [FoldoutGroup("Head Mover Data")]
        [ReadOnly]
        public bool isMoving;

        [FoldoutGroup("Head Mover Data")]
        [ReadOnly]
        public bool canChangeDirection;

        [FoldoutGroup("Head Mover Data")]
        [ReadOnly]
        public float movementStartTime;

        [FoldoutGroup("Head Mover Data")]
        [ReadOnly]
        public float totalMovementTime;

        [FoldoutGroup("Head Mover Data")]
        [ShowInInspector]
        [ReadOnly]
        public string movementPathTracker;

        // Додаткові поля для подій
        [FoldoutGroup("Event Data", true)]
        [ShowInInspector]
        [ReadOnly]
        public LevelResult levelEndResult;

        [FoldoutGroup("Event Data")]
        [ShowInInspector]
        [ReadOnly]
        public CollisionData headCollisionDetails;

        // Стартова ініціалізація
        void Start()
        {
            headMover = GetComponent<HeadMover>();
            if (headMover == null)
            {
                Debug.LogError("HeadMover компонент не знайдений на цьому об'єкті.");
            }
        }

        // Оновлення даних кожного кадру
        void Update()
        {
            if (headMover != null)
            {
                // Оновлюємо значення полів з компонента HeadMover
                currentDirection = headMover.CurrentDirection.ToString();
                currentPosition = headMover.transform.position;
                targetPosition = headMover.TargetPosition;
                currentSpeed = headMover.CurrentSpeed;
                isMoving = headMover.IsMoving;
                canChangeDirection = headMover.CanChangeDirection;
                movementStartTime = headMover.movementStartTime;
                totalMovementTime = headMover.totalMovementTime;
                movementPathTracker = string.Join(", ", headMover.movementPathTracker);
            }
        }

        // Метод для обробки події OnLevelEnd
        private void OnLevelEnd(LevelResult result)
        {
            levelEndResult = result; // Оновлюємо результат події
        }

        // Метод для обробки події OnHeadCollision
        private void OnHeadCollision(CollisionData collisionData)
        {
            headCollisionDetails = collisionData; // Оновлюємо інформацію про зіткнення
        }
    }
}
