using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Grower
{
    public abstract class ProcessBase : ScriptableObject, IProcess
    {
        [SerializeField] private string idName;
        public string ID => idName;

        protected bool isFirstTimeSceneLoaded = true;

        private IObserverService observerService = new DefaultObserverService();

        public void InitializeObserverService(IObserverService service)
        {
            observerService = service;
        }

        public abstract void Execute(params object[] args);

        /// <summary>
        /// Сповіщає спостерігачів про виконання процесу.
        /// </summary>
        protected void NotifyObservers(List<string> observerIDs, params object[] args)
        {
            foreach (var observerID in observerIDs)
            {
                var observer = observerService.GetObserverById(observerID);
                if (observer != null)
                {
                    observer.OnProcessExecuted(ID, args);
                }
            }
        }

        /// <summary>
        /// Метод, який викликається при завантаженні нової сцени.
        /// </summary>
        public virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Викликаємо базову логіку, але передаємо прапор, чи це перший запуск
            bool isFirstTime = isFirstTimeSceneLoaded;
            if (isFirstTimeSceneLoaded)
            {
                isFirstTimeSceneLoaded = false;
            }

            if (!isFirstTime)
                observerService.CleanupObservers();
        }

        /// <summary>
        /// Підписується на подію завантаження сцени.
        /// </summary>
        protected void SubscribeToSceneEvents()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        /// <summary>
        /// Відписується від подій завантаження сцени.
        /// </summary>
        protected void UnsubscribeFromSceneEvents()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnEnable()
        {
            SubscribeToSceneEvents();
            Application.quitting += OnApplicationQuit;  // Підписка на подію виходу з гри
        }

        private void OnDisable()
        {
            UnsubscribeFromSceneEvents();
            Application.quitting -= OnApplicationQuit;  // Відписка від події виходу з гри
        }

        // Обробник події виходу з гри
        private void OnApplicationQuit()
        {
            isFirstTimeSceneLoaded = true; // Скидаємо прапор під час виходу з гри
        }
    }
}