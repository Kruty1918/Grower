namespace Grower
{
    public interface ICollisionListener
    {
        /// <summary>
        /// Called when a collision event occurs.
        /// </summary>
        /// <param name="collisionData">Data about the collision event.</param>
        void CollisionNotify(CollisionData collisionData);
    }
}
