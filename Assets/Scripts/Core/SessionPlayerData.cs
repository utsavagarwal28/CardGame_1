using System;

namespace Game.Core
{
    [System.Serializable]
    public class SessionPlayerData
    {
        public string UID;
        public string DisplayName;
        public bool IsHost;
        public int PlayerNumber;
    }    
}
