using UnityEngine;
//using Firebase;
//using Firebase.Auth;
//using Google;
using System.Threading.Tasks;
using UnityEngine.Events;


namespace Game.Authentication
{
    public class FirebaseAuthManager : MonoBehaviour
    {
        [Header("Scriptable Object")]
        public PlayerData playerData;

        [Header("Google Sign-In Config")]
        [SerializeField] private string webClientID;

        [Header("Events")]
        public UnityEvent OnLoginSuccess;
        public UnityEvent<string> OnLoginFailed;

        //private FirebaseAuth firebaseAuth;
        //private GoogleSignInConfiguration googleConfig;
    }
}
