using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using Game.Authentication;

namespace Game.Lobby.LobbyCanvas
{
    public class ShowJoinCodeUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text joinCodeText;
        [SerializeField] private Button copyButton;

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

        private void Update()
        {
            if (LobbyManager.Instance.CurrentLobby != null)
            {
                copyButton.interactable = true;
                joinCodeText.text = LobbyManager.Instance.CurrentLobby.LobbyCode;
            }
            else
            {
                copyButton.interactable = false;
                joinCodeText.text = "Code";
            }
        }

        private void Start()
        {
            copyButton.onClick.AddListener(() =>
            {
                GUIUtility.systemCopyBuffer = joinCodeText.text;
            });
        }
    }
}