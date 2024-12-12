namespace Grower
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class CollisionBroadcaster : MonoBehaviour
    {
        private List<ICollisionListener> collisionListeners = new List<ICollisionListener>();

        private void Awake()
        {
            RegisterListeners();
        }

        private void OnEnable()
        {
            GrowerEvents.OnHeadCollision.AddListener(NotifyCollision);
        }

        private void OnDisable()
        {
            GrowerEvents.OnHeadCollision.RemoveListener(NotifyCollision);
        }

        /// <summary>
        /// Registers all components on this GameObject that implement ICollisionListener.
        /// </summary>
        private void RegisterListeners()
        {
            collisionListeners = GetComponents<ICollisionListener>().ToList();
            if (collisionListeners.Count == 0)
            {
                Debug.LogWarning($"[CollisionBroadcaster] No listeners implementing ICollisionListener found on '{gameObject.name}'.");
            }
        }

        /// <summary>
        /// Notifies all registered listeners about the collision.
        /// </summary>
        /// <param name="collisionData">Data about the collision event.</param>
        private void NotifyCollision(CollisionData collisionData)
        {
            foreach (var listener in collisionListeners)
            {
                try
                {
                    listener.CollisionNotify(collisionData);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"[CollisionBroadcaster] Error notifying listener {listener.GetType().Name}: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Updates the list of listeners dynamically if new components are added at runtime.
        /// </summary>
        public void UpdateListeners()
        {
            RegisterListeners();
        }
    }
}
