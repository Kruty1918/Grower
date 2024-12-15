using System.Collections;
using SGS29.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Grower
{
    public class SceneReloader : MonoSingleton<SceneReloader>
    {
        private ISceneReloaderUI reloaderUI;

        private void Start()
        {
            reloaderUI = GetComponent<ISceneReloaderUI>();

            // Забезпечуємо, що об'єкт не буде знищено при зміні сцени
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

        private void OnLevelEnd(LevelResult result)
        {
            // Починаємо анімацію затемнення екрану
            reloaderUI.TransitionIn();

            // Після завершення анімації починаємо завантаження нової сцени
            Invoke(nameof(StartSceneReload), reloaderUI.TransitionInDuration);
        }

        private void StartSceneReload()
        {
            // Завантажуємо сцену асинхронно
            StartCoroutine(LoadSceneAsync());
        }

        private IEnumerator LoadSceneAsync()
        {
            // Отримуємо поточну сцену
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            // Завантажуємо сцену асинхронно
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(currentSceneIndex);

            // Чекаємо завершення завантаження
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // Після завантаження запускаємо проявлення
            reloaderUI.TransitionOut();
        }
    }
}
