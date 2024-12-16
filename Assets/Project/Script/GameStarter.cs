using UnityEngine;

namespace Grower
{
    public class GameStarter : MonoBehaviour, IProcessObserver
    {
        [SerializeField] private bool useObserver;

        public void OnProcessExecuted(string processID, params object[] args)
        {
            if (useObserver)
                Play();
        }

        public void Play()
        {
            GrowerEvents.OnStartGame.Invoke();
        }
    }
}