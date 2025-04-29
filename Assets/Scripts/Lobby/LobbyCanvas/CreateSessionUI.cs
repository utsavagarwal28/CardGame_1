using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using Game.Authentication;

namespace Game.Lobby.LobbyCanvas
{
    public class CreateSessionUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField sessionNameInput;
        [SerializeField] private Button createButton;

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
            createButton.onClick.AddListener(async () =>
            {
                string sessionName = sessionNameInput.text;
                await LobbyManager.Instance.CreateLobbyWithHeartbeatAsync(sessionName);
            });
        }

        
    }
}
