using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Grower
{
    [CreateAssetMenu(fileName = "StartGame", menuName = "Grower/UI/Process/StartGame", order = 0)]
    public class StartGameProcess : ProcessBase
    {
        [InfoBox("Enter the names of the observers who will be on the active stage when the process is launched.")]
        [SerializeField] private List<string> observer_Ids;

        public override void Execute(params object[] args)
        {
            // Використання базової логіки для обробки спостерігачів
            NotifyObservers(observer_Ids, args);
        }
    }
}