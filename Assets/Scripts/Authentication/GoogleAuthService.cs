using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Game.Utils;

namespace Game.Authentication
{
    public class GoogleAuthService : MonoBehaviour
    {
        public AuthEventChannel authEvents;
        //public Game.Utils.EnvironmentSelector environmentSelector;

        async void Start()
        {
            await UnityServices.InitializeAsync(
                new InitializationOptions().SetEnvironmentName(EnvironmentSelector.Environment)
            );

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                try
                {
                    //await AuthenticationService.Instance.SignInWithGoogleAsync();
                    //authEvents.OnLoginSuccess?.Invoke();
                }
                catch
                {

                }
            }

        }

    }
}
