using SGS29.Utilities;
using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Class responsible for calling a process by its ID.
    /// </summary>
    public class ProcessCaller : MonoBehaviour
    {
        [SerializeField] private string processID;

        /// <summary>
        /// Calls the process associated with the specified process ID.
        /// </summary>
        public void Call()
        {
            // Executes the process using the GameStateMachine with the specified process ID
            SM.Instance<GameStateMachine>().ExecuteProcess(processID);
        }
    }
}