using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Process to handle the end of a level and notify relevant observers.
    /// </summary>
    [CreateAssetMenu(fileName = "LevelEndProcess", menuName = "Grower/UI/Process/LevelEnd", order = 0)]
    public class LevelEndProcess : ProcessBase
    {
        /// <summary>
        /// List of observer IDs that will be active during the process.
        /// </summary>
        [InfoBox("Enter the names of the observers who will be on the active stage when the process is launched.")]
        [SerializeField] private List<string> observer_Ids;

        /// <summary>
        /// Executes the level end process and notifies all relevant observers.
        /// </summary>
        /// <param name="args">Arguments passed to the observers.</param>
        public override void Execute(params object[] args)
        {
            // Use the base logic to notify observers
            NotifyObservers(observer_Ids, args);
        }
    }
}