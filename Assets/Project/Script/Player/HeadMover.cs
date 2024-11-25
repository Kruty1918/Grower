using UnityEngine;

namespace Grower
{
    public class HeadMover : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f; // Швидкість руху
        private Rigidbody rb;                          // Rigidbody для руху
        private Vector3 currentDirection = Vector3.zero; // Поточний напрямок руху
        private bool isMoving = false;                 // Чи об'єкт у русі
        private bool canChangeDirection = true;         // Чи можна змінити напрямок

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (isMoving)
            {
                rb.velocity = currentDirection * moveSpeed; // Рухаємося в поточному напрямку
            }
            else
            {
                rb.velocity = Vector3.zero; // Зупинка, коли не рухаємось
            }

            if (canChangeDirection) HandleInput(); // Дозволяємо зміну напрямку лише після зупинки
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.W)) SetDirection(Vector3.forward);
            else if (Input.GetKeyDown(KeyCode.S)) SetDirection(Vector3.back);
            else if (Input.GetKeyDown(KeyCode.A)) SetDirection(Vector3.left);
            else if (Input.GetKeyDown(KeyCode.D)) SetDirection(Vector3.right);
        }

        private void SetDirection(Vector3 direction)
        {
            if (!canChangeDirection || direction == Vector3.zero) return;

            currentDirection = direction.normalized; // Оновлюємо напрямок
            isMoving = true; // Починаємо рух
            canChangeDirection = false; // Забороняємо змінювати напрямок під час руху
            Debug.Log($"Direction set to: {currentDirection}");
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                isMoving = false; // Зупиняємо рух
                canChangeDirection = true; // Дозволяємо зміну напрямку
                currentDirection = Vector3.zero; // Обнуляємо поточний напрямок
                Debug.Log("Hit a wall! Movement stopped. Change direction allowed.");
            }
        }
    }
}
