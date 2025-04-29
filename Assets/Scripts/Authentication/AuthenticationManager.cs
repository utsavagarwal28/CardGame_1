using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;

namespace Game.Authentication
{
    public class AuthenticationManager : MonoBehaviour
    {
        public static AuthenticationManager Instance;

        public bool IsAuthenticated { get; private set; } = false;

        private async void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            await AuthenticateAsync();
        }

        public async Task AuthenticateAsync()
        {
            try
            {
                await UnityServices.InitializeAsync();

                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    Debug.Log($"Signed in! PlayerID: {AuthenticationService.Instance.PlayerId}");
                }
                IsAuthenticated = true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Authentication Failed: {ex.Message}");
            }
        }

        public string GetPlayerID()
        {
            return AuthenticationService.Instance.PlayerId;
        }
    }
}
