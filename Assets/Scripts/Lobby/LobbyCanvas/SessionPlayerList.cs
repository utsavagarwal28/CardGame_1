using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;
using Game.Authentication;
using UnityEngine.UIElements;
using System.Net.Http.Headers;

namespace Game.Lobby.LobbyCanvas
{
    public class SessionPlayerList : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private GameObject playerListItemPrefab;

        public Dictionary<int, Player> playerTagList { get; private set; }

        private async void Awake()
        {
            await InitializeAsync();

            LobbyManager.Instance.OnLobbyPoll += RefreshPlayerListAsync;
            RefreshPlayerListAsync();
            LobbyManager.Instance.OnLobbyLeft += CLearPlayerList;
        }

        private async Task InitializeAsync()
        {
            while (AuthenticationManager.Instance == null || !AuthenticationManager.Instance.IsAuthenticated)
                await Task.Yield();

            while (LobbyManager.Instance == null)
                await Task.Yield();
        }

        private void OnEnable()
        {
        }

        private void OnDestroy()
        {
            if (LobbyManager.Instance == null) return;

            LobbyManager.Instance.OnLobbyPoll -= RefreshPlayerListAsync;
            LobbyManager.Instance.OnLobbyLeft -= CLearPlayerList;
        }

        public void CLearPlayerList()
        {
            foreach (Transform child in content)
                Destroy(child.gameObject);
        }

        public async void RefreshPlayerListAsync()
        {
            foreach (Transform child in content)
                Destroy(child.gameObject);

            if (playerTagList != null)
            {
                playerTagList.Clear();
            }
            playerTagList = new Dictionary<int, Player>();

            if (LobbyManager.Instance.CurrentLobby == null) return;

            int playerTagValue = 1;
            bool hostControl = LobbyManager.Instance.IsHost();

            foreach (Player player in LobbyManager.Instance.CurrentLobby.Players)
            {
                GameObject item = Instantiate(playerListItemPrefab, content);
                item.SetActive(true);
                item.GetComponent<SessionPlayerListItem>().SetPlayer(player, playerTagValue, hostControl);
                //player.Data.TryGetValue("UID", out var playerUID);
                playerTagList.Add(playerTagValue, player);
                playerTagValue++;
            }

            await LobbyManager.Instance.UpdatePlayerTagsAsync(playerTagList, LobbyManager.Instance.CurrentLobby.Id);
        }
    }
}
