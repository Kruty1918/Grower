using SGS29.Utilities;

namespace Grower
{
    public class GameStateMachine : MonoSingleton<GameStateMachine>
    {
        public GameStateType CurrentState { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            OnGameState();
        }

        public void SetGameState(GameStateType stateType)
        {
            CurrentState = stateType;
            GrowerEvents.OnGameStateChange.Invoke(CurrentState);
        }

        private void OnGameState()
        {
            GrowerEvents.OnStartGame.AddListener(() => SetGameState(GameStateType.Playing));
            GrowerEvents.OnLevelEnd.AddListener((levelResult) => SetGameState(GameStateType.ReloadingScene));
        }
    }
}