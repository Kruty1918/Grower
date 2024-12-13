using System.Collections.Generic;
using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Default implementation of the observer service.
    /// </summary>
    public class DefaultObserverService : IObserverService
    {
        private readonly Dictionary<string, IProcessObserver> cachedHandlers = new Dictionary<string, IProcessObserver>();

        public IProcessObserver GetObserverById(string observerID)
        {
            if (string.IsNullOrWhiteSpace(observerID))
            {
                Debug.LogWarning("Observer ID is null or empty.");
                return null;
            }

            if (!cachedHandlers.TryGetValue(observerID, out var handler))
            {
                handler = FindHandlerById(observerID);
                if (handler != null)
                {
                    cachedHandlers[observerID] = handler;
                }
            }

            return handler;
        }

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
    }
}