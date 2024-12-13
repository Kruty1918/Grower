using System.Collections.Generic;
using UnityEngine;

namespace Grower
{
    public abstract class ProcessBase : ScriptableObject, IProcess
    {
        [SerializeField] private string idName;
        public string ID => idName;

        private IObserverService observerService = new DefaultObserverService();

        /// <summary>
        /// Initializes the UI process with the given observer service.
        /// </summary>
        /// <param name="service">The observer service to use for handling and retrieving observers.</param>
        public void InitializeObserverService(IObserverService service)
        {
            observerService = service;
        }

        /// <summary>
        /// Method to execute the process. Called by the controller.
        /// </summary>
        /// <param name="args">Parameters passed to the process.</param>
        public abstract void Execute(params object[] args);

        /// <summary>
        /// Executes a process for a list of observers.
        /// </summary>
        /// <param name="observerIDs">List of observer IDs.</param>
        /// <param name="args">Process parameters.</param>
        protected void NotifyObservers(IEnumerable<string> observerIDs, params object[] args)
        {
            if (observerService == null)
            {
                Debug.LogError("Observer service is not initialized.");
                return;
            }

            foreach (var observerID in observerIDs)
            {
                var handler = observerService.GetObserverById(observerID);
                if (handler != null)
                {
                    handler.OnProcessExecuted(ID, args);
                }
            }
        }
    }
}