using System.Collections.Generic;
using SGS29.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Grower
{
    public class GameStateMachine : MonoSingleton<GameStateMachine>
    {
        [SerializeField] private List<ProcessBase> processes;
        private IProcessExecutor processExecutor;

        [SerializeField, ReadOnly]
        private GameStateType _currentState;
        public GameStateType CurrentState { get => _currentState; }

        protected override void Awake()
        {
            base.Awake();
            OnGameState();
        }

        protected void Start()
        {
            processExecutor = new DefaultProcessExecutor(processes);
        }

        public void SetGameState(GameStateType stateType)
        {
            _currentState = stateType;
            GrowerEvents.OnGameStateChange.Invoke(_currentState);
        }

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