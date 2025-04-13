using UnityEngine;
using System.Collections.Generic;

namespace Game.Core
{
    public class GameManager : MonoBehaviour
    {
        // Setup Singleton Class
        public static GameManager Instance;

        public List<SessionPlayerData> sessionPlayers;

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
