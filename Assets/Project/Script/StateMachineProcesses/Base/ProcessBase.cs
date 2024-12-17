using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Grower
{
    /// <summary>
    /// Base class for processes that handle specific actions, such as executing events or managing observers.
    /// </summary>
    public abstract class ProcessBase : ScriptableObject, IProcess
    {
        [SerializeField] private string idName;

        /// <summary>
        /// Gets the unique ID for the process.
        /// </summary>
        public string ID => idName;

        protected bool isFirstTimeSceneLoaded = true;

        private IObserverService observerService = new DefaultObserverService();

        /// <summary>
        /// Initializes the observer service with a given instance.
        /// </summary>
        /// <param name="service">The observer service to initialize.</param>
        public void InitializeObserverService(IObserverService service)
        {
            observerService = service;
        }

        /// <summary>
        /// Executes the process with the given arguments.
        /// </summary>
        /// <param name="args">Arguments passed to the observers.</param>
        public abstract void Execute(params object[] args);

        /// <summary>
        /// Notifies observers about the execution of the process.
        /// </summary>
        /// <param name="observerIDs">List of observer IDs to notify.</param>
        /// <param name="args">Arguments to pass to the observers.</param>
        protected void NotifyObservers(List<string> observerIDs, params object[] args)
        {
            foreach (var observerID in observerIDs)
            {
                var observer = observerService.GetObserverById(observerID);
                if (observer != null)
                {
                    observer.OnProcessExecuted(ID, args);
                }
            }
        }

        /// <summary>
        /// Method called when a new scene is loaded.
        /// </summary>
        public virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Call base logic, passing a flag indicating whether it's the first time
            bool isFirstTime = isFirstTimeSceneLoaded;
            if (isFirstTimeSceneLoaded)
            {
                isFirstTimeSceneLoaded = false;
            }

            if (!isFirstTime)
                observerService.CleanupObservers();
        }

        /// <summary>
        /// Subscribes to the scene loading event.
        /// </summary>
        protected void SubscribeToSceneEvents()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        /// <summary>
        /// Unsubscribes from the scene loading event.
        /// </summary>
        protected void UnsubscribeFromSceneEvents()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnEnable()
        {
            SubscribeToSceneEvents();
            Application.quitting += OnApplicationQuit;  // Subscribe to the application quitting event
        }

        private void OnDisable()
        {
            UnsubscribeFromSceneEvents();
            Application.quitting -= OnApplicationQuit;  // Unsubscribe from the application quitting event
        }

        // Handler for the application quitting event
        private void OnApplicationQuit()
        {
            isFirstTimeSceneLoaded = true; // Reset the flag when the game exits
        }
    }
}