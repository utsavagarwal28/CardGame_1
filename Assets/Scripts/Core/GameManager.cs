using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;
using Game.Lobby;
using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;
using LobbyClass = Unity.Services.Lobbies.Models.Lobby;

namespace Game.Core
{
    public class GameManager : NetworkBehaviour
    {
        // Setup Singleton Class
        public static GameManager Instance;

        //private string playerTag;

        public NetworkList<SessionPlayerData> SessionPlayerDataList;

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            SessionPlayerDataList = new NetworkList<SessionPlayerData>();
        }

        private void Start()
        {
            if (IsClient)
            {
                SessionPlayerDataList.OnListChanged += OnSessionPlayerDataListChanged;
            }
        }

        public void PopulateSessionPlayerDataList()
        {
            if (!IsServer) return;

            SessionPlayerDataList.Clear();

            int i = 1;
            LobbyClass currentLobby = LobbyManager.Instance.CurrentLobby;

            foreach (Player player in currentLobby.Players)
            {
                player.Data.TryGetValue("DisplayName", out PlayerDataObject displayNameData);

                var data = new SessionPlayerData()
                {
                    UID = player.Id,
                    DisplayName = displayNameData?.Value ?? player.Id,
                    IsHost = player.Id == currentLobby.HostId,
                    LobbyPlayerNumber = i
                };

                SessionPlayerDataList.Add(data);
                i++;
            }

            Debug.Log("SessionPlayerDataList populated with " + SessionPlayerDataList.Count + " players.");
        }

        private void OnSessionPlayerDataListChanged(NetworkListEvent<SessionPlayerData> changeEvent)
        {
            Debug.Log("SessionPlayerDataList: " + changeEvent.Type);
        }

    }
}
