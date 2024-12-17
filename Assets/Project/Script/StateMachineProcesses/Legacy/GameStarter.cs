using UnityEngine;

namespace Grower
{
    /// <summary>
    /// The GameStarter class observes the execution of processes and starts the game
    /// when the corresponding process is executed, based on the useObserver flag.
    /// </summary>
    public class GameStarter : MonoBehaviour, IProcessObserver
    {
        /// <summary>
        /// A flag to determine whether the observer should be used to trigger the game start.
        /// If true, the Play method will be invoked when a process is executed.
        /// </summary>
        [SerializeField] private bool useObserver;

        /// <summary>
        /// Called when a process is executed.
        /// If the useObserver flag is set to true, the Play method is invoked to start the game.
        /// </summary>
        /// <param name="processID">The ID of the process being executed.</param>
        /// <param name="args">Additional arguments passed with the process execution.</param>
        public void OnProcessExecuted(string processID, params object[] args)
        {
            // If the observer flag is set to true, invoke the Play method
            if (useObserver)
                Play();
        }

        /// <summary>
        /// Starts the game by invoking the OnStartGame event.
        /// This event is broadcast to other systems in the game to trigger the start of the game.
        /// </summary>
        public void Play()
        {
            GrowerEvents.OnStartGame.Invoke(); // Triggers the start of the game
        }
    }
}