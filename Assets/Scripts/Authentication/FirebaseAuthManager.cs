//using UnityEngine;
//using Firebase;
//using Firebase.Auth;
//using Google;
//using System.Threading.Tasks;
//using UnityEngine.Events;


//namespace Game.Authentication
//{
//    public class FirebaseAuthManager : MonoBehaviour
//    {
//        [Header("Scriptable Object")]
//        public PlayerData playerData;

//        [Header("Google Sign-In Config")]
//        [SerializeField] private string webClientId;

//        [Header("Events")]
//        public UnityEvent LoginButtonClicked;
//        public UnityEvent OnLoginSuccess;
//        public UnityEvent<string> OnLoginFailed;

//        private FirebaseAuth firebaseAuth;
//        private GoogleSignInConfiguration googleConfig;

//        // Starts when the MainMenu Scene is loaded
//        void Awake()
//        {
//            // Setup Google Sign-In configuration
//            googleConfig = new GoogleSignInConfiguration
//            {
//                WebClientId = webClientId,
//                RequestIdToken = true
//            };

//            // Assign the config globally to GoogleSignIn
//            GoogleSignIn.Configuration = googleConfig;

//            // Sign Out previous sessions
//            GoogleSignIn.DefaultInstance.SignOut();

//            //Initialize Firebase Auth
//            firebaseAuth = FirebaseAuth.DefaultInstance;
//        }

//        // User clicks the LoginButton
//        public void SignInWithGoogle()
//        {
//            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthFinished);
//        }

//        private void OnGoogleAuthFinished(Task<GoogleSignInUser> task)
//        {
//            if (task.IsFaulted || task.IsCanceled)
//            {
//                Debug.LogError("Google Sign-In failed: " + task.Exception);
//                OnLoginFailed?.Invoke("Google Sign-In Failed");
//                return;
//            }

//            var googleUser = task.Result;
//            var credential = GoogleAuthProvider.GetCredential(googleUser.IdToken, null);

//            firebaseAuth.SignInWithCredentialAsync(credential).ContinueWith(OnFirebaseAuthFinished);
//        }

//        private void OnFirebaseAuthFinished(Task<FirebaseUser> task)
//        {
//            if (task.IsFaulted || task.IsCanceled)
//            {
//                Debug.LogError("Firebase Signi-In failed: " + task.Exception);
//                OnLoginFailed?.Invoke("Firebase Sign-In failed");
//                return;
//            }

//            FirebaseUser user = task.Result;

//            //Store data in PlayerData ScriptableObject
//            playerData.UID = user.UserId;
//            playerData.DisplayName = user.DisplayName;

//            Debug.Log($"Login Successful: {user.DisplayName} ({user.UserId})");
//            OnLoginSuccess?.Invoke();
//        }
//    }
//}
