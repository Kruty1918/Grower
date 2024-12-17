using System.Collections.Generic;
using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Default implementation of the observer service.
    /// </summary>
    public class DefaultObserverService : IObserverService
    {
        // A dictionary to cache the observers by their ID
        private readonly Dictionary<string, IProcessObserver> cachedHandlers = new Dictionary<string, IProcessObserver>();

        /// <summary>
        /// Retrieves an observer by its ID.
        /// </summary>
        /// <param name="observerID">The ID of the observer to retrieve.</param>
        /// <returns>The observer associated with the given ID, or null if not found.</returns>
        public IProcessObserver GetObserverById(string observerID)
        {
            if (string.IsNullOrWhiteSpace(observerID))
            {
                Debug.LogWarning("Observer ID is null or empty.");
                return null;
            }

            // Try to find the observer in the cache
            if (!cachedHandlers.TryGetValue(observerID, out var handler))
            {
                handler = FindHandlerById(observerID); // If not found in cache, look for it in the scene
                if (handler != null)
                {
                    cachedHandlers[observerID] = handler; // Cache the handler if found
                }
            }

            return handler;
        }

        /// <summary>
        /// Finds a handler by its observer ID by searching the scene for the GameObject with the given ID.
        /// </summary>
        /// <param name="observerID">The ID of the observer to find.</param>
        /// <returns>The handler associated with the given observer ID, or null if not found.</returns>
        private IProcessObserver FindHandlerById(string observerID)
        {
            var observerObject = GameObject.Find(observerID);
            if (observerObject == null)
            {
                Debug.LogWarning($"GameObject with ID {observerID} not found.");
                return null;
            }

            var handler = observerObject.GetComponent<IProcessObserver>();
            if (handler == null)
            {
                Debug.LogWarning($"Handler with ID {observerID} is missing or invalid.");
            }

            return handler;
        }

        /// <summary>
        /// Cleans up the cached observers, keeping only those that are "non-deletable".
        /// </summary>
        public void CleanupObservers()
        {
            var keysToRemove = new List<string>();

            // Check each cached observer and mark those that should be removed
            foreach (var entry in cachedHandlers)
            {
                if (entry.Value == null || !IsNonDeletable(entry.Value))
                {
                    keysToRemove.Add(entry.Key); // Mark for removal if not "non-deletable"
                }
            }

            // Remove the marked observers from the cache
            foreach (var key in keysToRemove)
            {
                cachedHandlers.Remove(key);
            }
        }

        /// <summary>
        /// Checks whether the observer is "non-deletable", i.e., it is still attached to the scene.
        /// </summary>
        /// <param name="observer">The observer to check.</param>
        /// <returns>True if the observer is non-deletable, otherwise false.</returns>
        private bool IsNonDeletable(IProcessObserver observer)
        {
            if (observer is MonoBehaviour monoBehaviour)
            {
                // Check if the object is still attached to the scene and is not destroyed
                return monoBehaviour != null && monoBehaviour.gameObject != null && monoBehaviour.gameObject.scene.rootCount > 0;
            }
            return false;
        }
    }
}