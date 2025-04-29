using UnityEngine;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using LobbyClass = Unity.Services.Lobbies.Models.Lobby;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Lobby.LobbyCanvas;
using Game.Relay;
using Game.Authentication;
using System;
using Unity.Services.Authentication;
using System.Linq;
using Game.Temporary;
using Newtonsoft.Json.Bson;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices.WindowsRuntime;


namespace Game.Lobby
{
    public class LobbyManager : MonoBehaviour
    {
        public static LobbyManager Instance;

        public LobbyClass CurrentLobby { get; private set; }

        //[SerializeField] private float heartBeatTimer = 15f;
        //[SerializeField] private float lobbyPollTimer = 1f;
        public Action OnLobbyPoll;
        public Action OnLobbyLeft;
        //public bool continuePolling;

        public string currentPlayerId;

        public string PlayerTag;


        async void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            await InitializeAsync();

            currentPlayerId = AuthenticationManager.Instance.GetPlayerID();
        }

        private async Task InitializeAsync()
        {
            while (AuthenticationManager.Instance == null || !AuthenticationManager.Instance.IsAuthenticated)
                await Task.Yield();
        }

        private async void StartHeartbeatLobbyAsync()
        {
            while (CurrentLobby != null)
            {
                await LobbyService.Instance.SendHeartbeatPingAsync(CurrentLobby.Id);
                await Task.Delay(15000);
            }
        }

        private async void StartLobbyPollAsync()
        {
            while (CurrentLobby != null)
            {
                try
                {
                    CurrentLobby.Data.TryGetValue("StartGame", out DataObject startGame);
                    if (startGame.Value == "true") return;

                    LobbyClass newLobby = await LobbyService.Instance.GetLobbyAsync(CurrentLobby.Id);
                    CurrentLobby = newLobby;

                    await JoinGame();

                   

                    OnLobbyPoll?.Invoke();
                }
                catch (LobbyServiceException e)
                {
                    // Player might have been kicked or lost access
                    Debug.LogWarning($"Lobby poll failed: {e.Message}");

                    // Check if the error is due to being kicked
                    if (e.Reason == LobbyExceptionReason.Forbidden)
                    {
                        Debug.Log("You were likely kicked from the lobby.");
                        break; // Exit the polling loop
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Unexpected error in lobby polling: {e.Message}");
                    break;
                }

                await Task.Delay(4000);
            }

            CurrentLobby = null; // Clear the lobby if access is lost
            OnLobbyLeft?.Invoke();
        }


        public async Task UpdatePlayerTagsAsync(Dictionary<int, Player> playerTagList, string lobbyId)
        {
            UpdatePlayerOptions lobbyOptions = new UpdatePlayerOptions();

            foreach (int key in playerTagList.Keys)
            {

                switch (key)
                {
                    case 1:
                        PlayerTag = "Player1";
                        break;

                    case 2:
                        PlayerTag = "Player2";
                        break;

                    case 3:
                        PlayerTag = "Player3";
                        break;

                    case 4:
                        PlayerTag = "Player4";
                        break;
                }

                playerTagList.TryGetValue(key, out Player player);


                if (player.Id == currentPlayerId)
                {

                    lobbyOptions.Data = new Dictionary<string, PlayerDataObject>()
                    {
                        {"PlayerTag", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, value: PlayerTag)},
                        {"IsHost", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, IsHost().ToString())}
                    };
                    break;
                }
            }
            var lobby = await LobbyService.Instance.UpdatePlayerAsync(this.CurrentLobby.Id, currentPlayerId, lobbyOptions);
        }

        public async Task CreateLobbyWithHeartbeatAsync(string lobbyName)
        {
            //Check if player is already in a lobby
            if (CurrentLobby != null)
                await LeaveLobby();

            int maxPlayers = 4;
            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();

            // Set lobby as private
            lobbyOptions.IsPrivate = true;

            // Store lobby's host details
            lobbyOptions.Player = new Player(
                id: AuthenticationManager.Instance.GetPlayerID(),
                data: new Dictionary<string, PlayerDataObject>()
                {
                    {
                        "UID", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, value: currentPlayerId)
                    },
                    {
                        "DisplayName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, value: currentPlayerId)
                    },

                });

            lobbyOptions.Data = new Dictionary<string, DataObject>()
            {
                {"StartGame", new DataObject(DataObject.VisibilityOptions.Public, "false") }
            };

            // Create the lobby with current options and set it as current lobby
            CurrentLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, lobbyOptions);
            Debug.Log(CurrentLobby.LobbyCode + " created");

            StartHeartbeatLobbyAsync();

            OnLobbyPoll?.Invoke();
            StartLobbyPollAsync();

            return;
        }


        public async Task JoinLobbyByCodeAsync(string joinCode)
        {
            //Check if player is already in a lobby
            if (CurrentLobby != null)
                await LeaveLobby();

            JoinLobbyByCodeOptions lobbyOptions = new JoinLobbyByCodeOptions();


            lobbyOptions.Player = new Player(
                id: AuthenticationManager.Instance.GetPlayerID(),
                data: new Dictionary<string, PlayerDataObject>()
                {
                    {
                        "UID", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, value: currentPlayerId)
                    },
                    {
                        "DisplayName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, value: currentPlayerId)
                    },

                });



            //Join lobby with lobbyCode and lobbyOptions
            CurrentLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(joinCode, lobbyOptions);

            OnLobbyPoll?.Invoke();
            StartLobbyPollAsync();

            return;
        }



        public bool IsHost()
        {
            return CurrentLobby != null && CurrentLobby.HostId == AuthenticationManager.Instance.GetPlayerID();
        }

        public async void KickPlayer(string playerId)
        {
            if (!IsHost()) return;

            try
            {
                await LobbyService.Instance.RemovePlayerAsync(CurrentLobby.Id, playerId);
                OnLobbyPoll?.Invoke();
                Debug.Log($"Kicked player with ID: {playerId}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to kick player: {e.Message}");
            }
        }

        public async void StartGame()
        {
            if (IsHost())
            {
                // Initiate Relay Server
                string RelayJoinCode = await RelayManager.Instance.StartHostWithRelayAsync();

                UpdateLobbyOptions lobbyOptions = new UpdateLobbyOptions();

                lobbyOptions.Data = new Dictionary<string, DataObject>()
                {
                    {"RelayJoinCode",  new DataObject(DataObject.VisibilityOptions.Public, RelayJoinCode)},
                    {"StartGame", new DataObject(DataObject.VisibilityOptions.Public, "true") }
                };
                await LobbyService.Instance.UpdateLobbyAsync(CurrentLobby.Id, lobbyOptions);
            }
            //Debug.Log("Starting Game...");
            //Unity.Netcode.NetworkManager.Singleton.SceneManager.LoadScene("GameScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        public async Task JoinGame()
        {
            if (IsHost()) return;

            CurrentLobby.Data.TryGetValue("StartGame", out DataObject startGame);
            if (startGame.Value == "false") return;


            CurrentLobby.Data.TryGetValue("RelayJoinCode", out DataObject relayJoinCode);

            bool gameJoined = await RelayManager.Instance.StartClientWithRelayAsync(relayJoinCode.Value);

            Debug.Log($"{currentPlayerId} join {relayJoinCode.Value} {(gameJoined ? "successfully" : "failed")}");

            return;
        }

        public async Task LeaveLobby()
        {
            if (CurrentLobby != null)
            {
                try
                {
                    await LobbyService.Instance.RemovePlayerAsync(CurrentLobby.Id, AuthenticationManager.Instance.GetPlayerID());
                    Debug.Log("Left the lobby.");
                    CurrentLobby = null;
                    OnLobbyPoll?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error leaving the lobby: {e.Message}");
                }
            }
        }
    }
}
