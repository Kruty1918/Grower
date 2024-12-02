using UnityEngine;

namespace Grower
{
    public static class Vector2Extensions
    {
        /// <summary>
        /// Визначає основний напрямок для даного вектора.
        /// </summary>
        /// <param name="vector">Вхідний вектор.</param>
        /// <param name="threshold">Поріг для визначення напрямку. За замовчуванням 0.1f.</param>
        /// <returns>Основний напрямок як CardinalDirection.</returns>
        public static CardinalDirection ToCardinalDirection(this Vector2 vector, float threshold = 0.1f)
        {
            if (vector.magnitude < threshold)
                return CardinalDirection.None;

            if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
            {
                return vector.x > 0 ? CardinalDirection.Right : CardinalDirection.Left;
            }
            else
            {
                return vector.y > 0 ? CardinalDirection.Up : CardinalDirection.Down;
            }
        }
    }
}