using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Class containing data about a collision between the snake's head and another object.
    /// </summary>
    [System.Serializable]
    public class CollisionData
    {
        /// <summary>
        /// Coordinates of the snake's head.
        /// </summary>
        [SerializeField] public Vector2Int HeadCoordinates;

        /// <summary>
        /// Coordinates of the collided object.
        /// </summary>
        [SerializeField] public Vector2Int ObjectCoordinates;

        /// <summary>
        /// The force of the collision.
        /// </summary>
        [SerializeField] public float CollisionForce;

        /// <summary>
        /// The side of the object that was collided with.
        /// </summary>
        [SerializeField] public CollisionSide Side;

        /// <summary>
        /// The object that was collided with.
        /// </summary>
        [SerializeField] public Cell CollidedObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollisionData"/> class.
        /// </summary>
        /// <param name="headCoordinates">Coordinates of the snake's head.</param>
        /// <param name="objectCoordinates">Coordinates of the collided object.</param>
        /// <param name="collisionForce">The force of the collision.</param>
        /// <param name="side">The side of the object that was collided with.</param>
        /// <param name="collidedObject">The collided object.</param>
        public CollisionData(Vector2Int headCoordinates, Vector2Int objectCoordinates, float collisionForce, CollisionSide side, Cell collidedObject)
        {
            HeadCoordinates = headCoordinates;
            ObjectCoordinates = objectCoordinates;
            CollisionForce = collisionForce;
            Side = side;
            CollidedObject = collidedObject;
        }
    }
}