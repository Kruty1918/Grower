using System.Collections;
using System.Threading.Tasks;
using SGS29.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Grower
{
    public class SceneReloader : MonoSingleton<SceneReloader>
    {
        [SerializeField] private SceneValidatorBase sceneValidator; // Validator for next scene selection
        private ISceneReloaderUI reloaderUI;
        private LevelResult currentLevelResult;

        private void Start()
        {
            reloaderUI = GetComponent<ISceneReloaderUI>();

            // Ensure the object persists across scene loads
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            GrowerEvents.OnLevelEnd.AddListener(OnLevelEnd);
        }

        private void OnDisable()
        {
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

        private async void StartSceneReload()
        {
            if (sceneValidator == null)
            {
                Debug.LogError("SceneValidator is not assigned.");
                return;
            }

            int nextSceneIndex = sceneValidator.GetNextSceneBuildIndex(currentLevelResult);
            if (nextSceneIndex < 0 || nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.LogError($"Invalid scene index: {nextSceneIndex}");
                return;
            }

            await LoadSceneAsync(nextSceneIndex);
            OnSceneLoadedFeedback(currentLevelResult);
        }

        private async Task LoadSceneAsync(int nextSceneIndex)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneIndex);

            while (!asyncLoad.isDone)
            {
                await Task.Yield();
            }

            reloaderUI.TransitionOut();
        }

        private void OnSceneLoadedFeedback(LevelResult result)
        {
            SM.Instance<GameManager>().LastLevelFeedback(result);
        }
    }
}