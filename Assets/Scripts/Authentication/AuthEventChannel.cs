using UnityEngine;
using UnityEngine.Events;

namespace Game.Authentication
{
    [CreateAssetMenu(fileName = "AuthEventChannel", menuName = "Scriptable Objects/AuthEventChannel")]
    public class AuthEventChannel : ScriptableObject
    {
        public UnityEvent OnLoginSuccess;
        public UnityEvent<string> OnLoginFailed;
    }
}
