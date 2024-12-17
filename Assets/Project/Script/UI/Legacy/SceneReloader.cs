using System.Collections;
using System.Threading.Tasks;
using SGS29.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Grower
{
    /// <summary>
    /// Singleton class responsible for reloading scenes and handling the transition UI.
    /// </summary>
    public class SceneReloader : MonoSingleton<SceneReloader>
    {
        [SerializeField] private SceneValidatorBase sceneValidator; // Validator for next scene selection
        private ISceneReloaderUI reloaderUI;
        private LevelResult currentLevelResult;

        private void Start()
        {
            // Initialize the reloader UI
            reloaderUI = GetComponent<ISceneReloaderUI>();

            // Ensure the object persists across scene loads
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            // Subscribe to the level end event
            GrowerEvents.OnLevelEnd.AddListener(OnLevelEnd);
        }

        private void OnDisable()
        {
            // Unsubscribe from the level end event
            GrowerEvents.OnLevelEnd.RemoveListener(OnLevelEnd);
        }

        /// <summary>
        /// Triggered at the end of a level.
        /// </summary>
        /// <param name="result">The result of the level.</param>
        private void OnLevelEnd(LevelResult result)
        {
            currentLevelResult = result;

            // Start screen transition
            reloaderUI?.TransitionIn();

            // Begin scene reload after transition duration
            Invoke(nameof(StartSceneReload), reloaderUI?.TransitionInDuration ?? 0f);
        }

        /// <summary>
        /// Starts the scene reload process.
        /// </summary>
        private async void StartSceneReload()
        {
            if (sceneValidator == null)
            {
                Debug.LogError("SceneValidator is not assigned.");
                return;
            }

            // Get the index of the next scene based on the level result
            int nextSceneIndex = sceneValidator.GetNextSceneBuildIndex(currentLevelResult);

            // Validate the scene index
            if (nextSceneIndex < 0 || nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.LogError($"Invalid scene index: {nextSceneIndex}");
                return;
            }

            // Load the next scene asynchronously
            await LoadSceneAsync(nextSceneIndex);

            // Provide feedback after the scene is loaded
            OnSceneLoadedFeedback(currentLevelResult);
        }

        /// <summary>
        /// Loads the next scene asynchronously.
        /// </summary>
        /// <param name="nextSceneIndex">The index of the next scene to load.</param>
        private async Task LoadSceneAsync(int nextSceneIndex)
        {
            // Load the scene asynchronously
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneIndex);

            // Wait until the scene is fully loaded
            while (!asyncLoad.isDone)
            {
                await Task.Yield();
            }

            // Trigger the transition out after the scene is loaded
            reloaderUI.TransitionOut();
        }

        /// <summary>
        /// Provides feedback after the scene is loaded.
        /// </summary>
        /// <param name="result">The result of the level.</param>
        private void OnSceneLoadedFeedback(LevelResult result)
        {
            // Pass feedback to the GameManager after the scene is loaded
            SM.Instance<GameManager>().LastLevelFeedback(result);
        }
    }
}