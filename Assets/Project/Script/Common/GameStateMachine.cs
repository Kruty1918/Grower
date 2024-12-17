using System.Collections.Generic;
using SGS29.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Grower
{
    /// <summary>
    /// Manages the game states and transitions between them.
    /// Handles the execution of processes based on the current game state.
    /// </summary>
    public class GameStateMachine : MonoSingleton<GameStateMachine>
    {
        [SerializeField] private List<ProcessBase> processes;  // List of processes to be executed based on game state
        private IProcessExecutor processExecutor;  // Interface to execute processes

        [SerializeField, ReadOnly]
        private GameStateType _currentState;  // Current game state
        public GameStateType CurrentState { get => _currentState; }

        /// <summary>
        /// Called when the object is initialized.
        /// Sets up listeners for game state changes.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            OnGameState();
        }

        /// <summary>
        /// Called at the start of the game to initialize the process executor.
        /// </summary>
        protected void Start()
        {
            processExecutor = new DefaultProcessExecutor(processes);
        }

        /// <summary>
        /// Sets the current game state and invokes the corresponding event.
        /// </summary>
        /// <param name="stateType">The state to set.</param>
        public void SetGameState(GameStateType stateType)
        {
            _currentState = stateType;
            GrowerEvents.OnGameStateChange.Invoke(_currentState);
        }

        /// <summary>
        /// Registers listeners for game state changes and processes.
        /// </summary>
        private void OnGameState()
        {
            GrowerEvents.OnStartGame.AddListener(() => SetGameState(GameStateType.Playing));
            GrowerEvents.OnLevelEnd.AddListener((levelResult) => SetGameState(GameStateType.ReloadingScene));
        }

        /// <summary>
        /// Executes a process by its ID using the ProcessExecutor.
        /// </summary>
        /// <param name="processID">The ID of the process to execute.</param>
        /// <param name="args">Parameters for the process.</param>
        public void ExecuteProcess(string processID, params object[] args)
        {
            processExecutor?.ExecuteProcess(processID, args);
        }
    }
}