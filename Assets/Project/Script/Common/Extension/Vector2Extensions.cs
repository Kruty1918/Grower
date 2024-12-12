using UnityEngine;

namespace Grower.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="Vector2"/> struct to provide additional functionality.
    /// This class includes methods for converting a vector to a cardinal direction.
    /// </summary>
    public static class Vector2Extensions
    {
        /// <summary>
        /// Determines the primary cardinal direction for the given vector.
        /// The direction is calculated based on the vector's X and Y components.
        /// </summary>
        /// <param name="vector">The input vector to determine the direction.</param>
        /// <param name="threshold">Threshold to consider when determining the direction. Default is 0.1f.</param>
        /// <returns>The primary cardinal direction based on the vector.</returns>
        public static CardinalDirection ToCardinalDirection(this Vector2 vector, float threshold = 0.1f)
        {
            // If the vector's magnitude is below the threshold, return None
            if (vector.magnitude < threshold)
                return CardinalDirection.None;

            // Determine if the vector is more horizontal or vertical
            if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
            {
                // Return Right or Left based on the X component
                return vector.x > 0 ? CardinalDirection.Right : CardinalDirection.Left;
            }
            else
            {
                // Return Up or Down based on the Y component
                return vector.y > 0 ? CardinalDirection.Up : CardinalDirection.Down;
            }
        }
    }
}