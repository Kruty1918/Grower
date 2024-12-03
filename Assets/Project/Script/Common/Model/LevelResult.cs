using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Клас, що зберігає результати завершення рівня гри.
    /// </summary>
    [System.Serializable]
    public class LevelResult
    {
        [SerializeField] public int SceneBuildIndex;
        [SerializeField] public int LevelIndex;
        [SerializeField] public Vector2Int LastCellCoord;
        [SerializeField] public float PassageTime;
        [SerializeField] public bool LevelComplete;
        public LevelValidator levelValidator;

        public LevelResult(int sceneBuildIndex, int levelIndex, Vector2Int lastCellCoord, float passageTime, int fillCell, LevelValidator levelValidator)
        {
            SceneBuildIndex = sceneBuildIndex;
            LevelIndex = levelIndex;
            LastCellCoord = lastCellCoord;
            PassageTime = passageTime;

            LevelComplete = levelValidator.LevelComplete(fillCell);

            Debug.Log($"Level Complete: {levelValidator.LevelComplete(fillCell)}");
        }

        /// <summary>
        /// Represents data about a collision between the snake's head and another object.
        /// </summary>
        [System.Serializable]
        public class CollisionData
        {
            [SerializeField] public Vector2Int HeadCoordinates;
            [SerializeField] public Vector2Int ObjectCoordinates;
            [SerializeField] public float CollisionForce;
            [SerializeField] public CollisionSide Side;
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

        /// <summary>
        /// Represents the side of an object where the collision occurred.
        /// </summary>
        public enum CollisionSide
        {
            Top,
            Bottom,
            Left,
            Right
        }
    }
}
