using UnityEngine.Events;

namespace Grower
{
    /// <summary>
    /// A static class to hold and manage events related to the Grower game.
    /// It includes events for level completion and collisions.
    /// </summary>
    public static class GrowerEvents
    {
        /// <summary>
        /// Event triggered when the level is completed.
        /// The event provides the result of the level including details like the time taken,
        /// the final position, and the movement path.
        /// </summary>
        public static UnityEvent<LevelResult> OnLevelEnd = new UnityEvent<LevelResult>();

        /// <summary>
        /// Event triggered when the player's head collides with an object.
        /// The event provides collision data, including force and the side of the collision.
        /// </summary>
        public static UnityEvent<CollisionData> OnHeadCollision = new UnityEvent<CollisionData>();

        public static UnityEvent<GameStateType> OnGameStateChange = new UnityEvent<GameStateType>();

        public static UnityEvent OnStartGame = new UnityEvent();
    }
}