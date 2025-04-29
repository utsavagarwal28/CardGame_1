using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using Game.Authentication;

namespace Game.Lobby.LobbyCanvas
{
    public class JoinSessionByCodeUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField codeInput;
        [SerializeField] private Button joinButton;

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
            joinButton.onClick.AddListener(async () =>
            {
                string code = codeInput.text;
                await LobbyManager.Instance.JoinLobbyByCodeAsync(code);
            });
        }
    }
}