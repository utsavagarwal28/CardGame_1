using UnityEngine;


namespace Game.Authentication
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Game/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public string DisplayName;
        public string UID;
    }
}
