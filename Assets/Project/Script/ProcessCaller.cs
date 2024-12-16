using SGS29.Utilities;
using UnityEngine;

namespace Grower
{
    public class ProcessCaller : MonoBehaviour
    {
        [SerializeField] private string processID;

        public void Call()
        {
            SM.Instance<GameStateMachine>().ExecuteProcess(processID);
        }
    }
}