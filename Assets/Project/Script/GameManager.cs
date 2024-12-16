using SGS29.Utilities;

namespace Grower
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private void Start()
        {
            SetGameState(GameStateType.MainMenu);
        }

        //Feedback from the last level 
        public void LastLevelFeedback(LevelResult feedbackResult)
        {
            if (!feedbackResult.LevelComplete)
            {
                SetGameState(GameStateType.Playing);
                GrowerEvents.OnLevelRestart.Invoke();
            }
        }

        private void SetGameState(GameStateType type)
        {
            SM.Instance<GameStateMachine>().SetGameState(type);
        }
    }
}