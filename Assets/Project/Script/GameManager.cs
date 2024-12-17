using SGS29.Utilities;

namespace Grower
{
    /// <summary>
    /// The GameManager class is responsible for managing the game's state, 
    /// including transitioning between states like the main menu and gameplay, 
    /// and providing feedback from the last level to determine if the level is complete.
    /// </summary>
    public class GameManager : MonoSingleton<GameManager>
    {
        /// <summary>
        /// Called at the start of the game to set the initial game state.
        /// The game will start at the main menu.
        /// </summary>
        private void Start()
        {
            // Set the initial game state to MainMenu when the game starts
            SetGameState(GameStateType.MainMenu);
        }

        /// <summary>
        /// Provides feedback from the last level, determining if the level was completed.
        /// If the level was not completed, it triggers the restart process.
        /// </summary>
        /// <param name="feedbackResult">The result of the last level, including whether it was completed.</param>
        public void LastLevelFeedback(LevelResult feedbackResult)
        {
            if (!feedbackResult.LevelComplete)
            {
                // If the level was not completed, set the game state to Playing and restart the level
                SetGameState(GameStateType.Playing);
                GrowerEvents.OnLevelRestart.Invoke(); // Trigger the level restart event
            }
        }

        /// <summary>
        /// Sets the current game state using the GameStateMachine.
        /// </summary>
        /// <param name="type">The type of the game state to set.</param>
        private void SetGameState(GameStateType type)
        {
            // Use the GameStateMachine to set the current game state
            SM.Instance<GameStateMachine>().SetGameState(type);
        }
    }
}