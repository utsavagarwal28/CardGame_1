using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;
using Game.Authentication;

namespace Game.Lobby.LobbyCanvas
{
    public class SessionPlayerList : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private GameObject playerListItemPrefab;

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

        private void OnDestroy()
        {
            CLearPlayerList();
            LobbyManager.Instance.OnLobbyPoll -= RefreshPlayerListAsync;
            LobbyManager.Instance.OnLobbyLeft -= CLearPlayerList;
        }

        public void CLearPlayerList()
        {
            foreach (Transform child in content)
                Destroy(child.gameObject);
        }

        public  void RefreshPlayerListAsync()
        {
            foreach (Transform child in content)
                Destroy(child.gameObject);

            if (LobbyManager.Instance.CurrentLobby == null) return;

            bool hostControl = LobbyManager.Instance.IsHost();

            foreach (Player player in LobbyManager.Instance.CurrentLobby.Players)
            {
                GameObject item = Instantiate(playerListItemPrefab, content);
                item.SetActive(true);
                item.GetComponent<SessionPlayerListItem>().SetPlayer(player, hostControl);
            }
        }
    }
}
