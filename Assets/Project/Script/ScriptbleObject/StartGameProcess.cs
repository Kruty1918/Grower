using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Grower
{
    /// <summary>
    /// Defines the process that starts the game by notifying observers.
    /// </summary>
    [CreateAssetMenu(fileName = "StartGame", menuName = "Grower/UI/Process/StartGame", order = 0)]
    public class StartGameProcess : ProcessBase
    {
        [InfoBox("Enter the names of the observers who will be on the active stage when the process is launched.")]
        [SerializeField] private List<string> observer_Ids;

        /// <summary>
        /// Executes the process, notifying observers of the process execution.
        /// </summary>
        /// <param name="args">Additional arguments for the process.</param>
        public override void Execute(params object[] args)
        {
            // Using the base logic to notify observers
            NotifyObservers(observer_Ids, args);
        }
    }
}