using UnityEngine;

namespace Grower
{
    public class GameStarter : MonoBehaviour
    {
        public void Play()
        {
            GrowerEvents.OnStartGame.Invoke();
        }
    }
}