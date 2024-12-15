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

        /// <summary>
        /// Очищає кеш спостерігачів, залишаючи лише ті, які є "незнищуваними".
        /// </summary>
        public void CleanupObservers()
        {
            var keysToRemove = new List<string>();

            foreach (var entry in cachedHandlers)
            {
                if (entry.Value == null || !IsNonDeletable(entry.Value))
                {
                    keysToRemove.Add(entry.Key);
                }
            }

            // Видаляємо всі ключі, які не відповідають умовам
            foreach (var key in keysToRemove)
            {
                cachedHandlers.Remove(key);
            }
        }

        /// <summary>
        /// Перевіряє, чи є об'єкт "незнищуваним".
        /// </summary>
        private bool IsNonDeletable(IProcessObserver observer)
        {
            if (observer is MonoBehaviour monoBehaviour)
            {
                // Перевірка, чи об'єкт не знищений і прив'язаний до сцени
                return monoBehaviour != null && monoBehaviour.gameObject != null && monoBehaviour.gameObject.scene.rootCount > 0;
            }
            return false;
        }
    }
}
