using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Game.Authentication;

namespace Game.Lobby.LobbyCanvas
{
    public class LeaveSessionUI : MonoBehaviour
    {
        public Button leaveButton;

        private async void Awake()
        {
            await InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            while (AuthenticationManager.Instance == null || !AuthenticationManager.Instance.IsAuthenticated)
                await Task.Yield();

            while (LobbyManager.Instance == null)
                await Task.Yield();
        }

        private void Start()
        {
            leaveButton.onClick.AddListener(async () =>
            {
                await LobbyManager.Instance.LeaveLobby();
            });
        }
    }
}