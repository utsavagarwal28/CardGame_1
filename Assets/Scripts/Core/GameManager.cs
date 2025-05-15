using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;
using Game.Lobby;
using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;

namespace Game.Core
{
    public class GameManager : NetworkBehaviour
    {
        // Setup Singleton Class
        public static GameManager Instance;

        //private string playerTag;

        private static List<SessionPlayerData> sessionPlayerDataList = new List<SessionPlayerData>();
        public static List<SessionPlayerData> SessionPlayerDataList { get => sessionPlayerDataList; private set => sessionPlayerDataList = value; }

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
