using UnityEngine;
using Unity.Multiplayer.Playmode;

namespace Game.Temporary
{
    public class PlayerTagReader : MonoBehaviour
    {
        public static int LocalPlayerNumber { get; private set; }

        void Awake()
        {
            string[] playerTags = CurrentPlayer.ReadOnlyTags();

            if (playerTags == null || playerTags.Length == 0)
            {
                Debug.LogWarning("Untagged");
                return;
            }

            foreach (string tag in playerTags)
            {
                Debug.Log("Player Tag: " + tag);

                switch (tag)
                {
                    case "Player 1":
                        LocalPlayerNumber = 0;
                        Debug.Log(tag + ":" + LocalPlayerNumber);
                        break;
                    case "Player 2":
                        LocalPlayerNumber = 1;
                        Debug.Log(tag + ":" + LocalPlayerNumber);
                        break;
                    case "Player 3":
                        LocalPlayerNumber = 2;
                        Debug.Log(tag + ":" + LocalPlayerNumber);
                        break;
                    case "Player 4":
                        LocalPlayerNumber = 3;
                        Debug.Log(tag + ":" + LocalPlayerNumber);
                        break;
                    default:
                        Debug.LogWarning("Unknown tag: " + tag);
                        break;
                }
            }
        }
    }
}
