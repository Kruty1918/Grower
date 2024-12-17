namespace Grower
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Broadcasts collision events to all registered listeners implementing the ICollisionListener interface.
    /// This class listens for collision events and notifies all components on the GameObject that implement ICollisionListener.
    /// </summary>
    public class CollisionBroadcaster : MonoBehaviour
    {
        /// <summary>
        /// List of registered collision listeners.
        /// </summary>
        private List<ICollisionListener> collisionListeners = new List<ICollisionListener>();

        /// <summary>
        /// Registers all components on this GameObject that implement ICollisionListener.
        /// </summary>
        private void Awake()
        {
            RegisterListeners();
        }

        /// <summary>
        /// Subscribes to the OnHeadCollision event when the object is enabled.
        /// </summary>
        private void OnEnable()
        {
            GrowerEvents.OnHeadCollision.AddListener(NotifyCollision);
        }

        /// <summary>
        /// Unsubscribes from the OnHeadCollision event when the object is disabled.
        /// </summary>
        private void OnDisable()
        {
            GrowerEvents.OnHeadCollision.RemoveListener(NotifyCollision);
        }

        /// <summary>
        /// Registers all components on this GameObject that implement ICollisionListener.
        /// </summary>
        /// <remarks>
        /// If no listeners are found, a warning message is logged.
        /// </remarks>
        private void RegisterListeners()
        {
            collisionListeners = GetComponents<ICollisionListener>().ToList();
            if (collisionListeners.Count == 0)
            {
                Debug.LogWarning($"[CollisionBroadcaster] No listeners implementing ICollisionListener found on '{gameObject.name}'.");
            }
        }

        /// <summary>
        /// Notifies all registered listeners about the collision event.
        /// </summary>
        /// <param name="collisionData">Data about the collision event, including the object coordinates and force.</param>
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
        /// This method can be called to refresh the listener list after runtime changes.
        /// </summary>
        public void UpdateListeners()
        {
            RegisterListeners();
        }
    }
}