using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Game.Authentication;

namespace Game.Lobby.LobbyCanvas
{
    public class StartGameUI : MonoBehaviour
    {
        [SerializeField] private Button startGameButton;

        private async void Awake()
        {
            await InitializeAsync();
            LobbyManager.Instance.OnLobbyPoll += StartGameButtonActivate;
            LobbyManager.Instance.OnLobbyLeft += StartGameButtonDeactivate;
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
            if (LobbyManager.Instance == null) return;

            LobbyManager.Instance.OnLobbyPoll -= StartGameButtonActivate;
            LobbyManager.Instance.OnLobbyLeft -= StartGameButtonDeactivate;
        }

        private void OnDisable()
        {
            if (LobbyManager.Instance == null) return;

            LobbyManager.Instance.OnLobbyPoll -= StartGameButtonActivate;
            LobbyManager.Instance.OnLobbyLeft -= StartGameButtonDeactivate;
        }

        private void StartGameButtonActivate()
        {
            if (startGameButton == null || startGameButton.gameObject == null)
                return;

            var lobby = LobbyManager.Instance.CurrentLobby;
            bool isHost = LobbyManager.Instance.IsHost();
            bool full = lobby.Players != null && lobby.Players.Count == 4;

            startGameButton.gameObject.SetActive(isHost && full);
        }
        private void StartGameButtonDeactivate()
        {
            if (startGameButton != null && startGameButton.gameObject != null)
                startGameButton.gameObject.SetActive(false);
        }

        private void Start()
        {
            startGameButton.onClick.AddListener(() =>
            {
                LobbyManager.Instance.StartGame();
                startGameButton.gameObject.SetActive(false);
            });
            startGameButton.gameObject.SetActive(false);
        }
    }
}
