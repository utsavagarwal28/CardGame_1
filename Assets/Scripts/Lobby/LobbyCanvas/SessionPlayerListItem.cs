using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;
using AuthenticationManager = Game.Authentication.AuthenticationManager;

namespace Game.Lobby.LobbyCanvas
{
    public class SessionPlayerListItem : MonoBehaviour
    {
        [SerializeField] private Image panel;
        [SerializeField] private TMP_Text playerNameText;
        //[SerializeField] private TMP_Text playerTagNumber;
        [SerializeField] Button kickButton;

        private Player player;

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

        public void SetPlayer(Player p, bool hostControl)
        {
            player = p;

            string displayName = p.Data != null && p.Data.ContainsKey("DisplayName")
                ? p.Data["DisplayName"].Value
                : p.Id;

            playerNameText.text = displayName;
            //playerTagNumber.text = playerTagValue.ToString();

            bool isLocalP = p.Id == AuthenticationManager.Instance.GetPlayerID();
            bool isHostP = p.Id == LobbyManager.Instance.CurrentLobby.HostId;

            if (isHostP)
                panel.color = new Color(0.94f, 0.57f, 0.24f, 1.0f);
            else if (isLocalP)
                panel.color = new Color(0.25f, 1.0f, 0.5f, 1.0f);

            kickButton.gameObject.SetActive(!isHostP && hostControl);
            if (!hostControl) return;

            kickButton.onClick.RemoveAllListeners();
            kickButton.onClick.AddListener(() =>
            {
                LobbyManager.Instance.KickPlayer(p.Id);
            });
        }
    }
}