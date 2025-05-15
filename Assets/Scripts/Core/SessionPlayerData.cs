using System;
using Unity.Collections;
using Unity.Netcode;

namespace Game.Core
{
    [System.Serializable]
    public struct SessionPlayerData : INetworkSerializable, IEquatable<SessionPlayerData>
    {
        public FixedString64Bytes UID;
        public FixedString64Bytes DisplayName;
        public bool IsHost;
        public int LobbyPlayerNumber;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref UID);
            serializer.SerializeValue(ref DisplayName);
            serializer.SerializeValue(ref IsHost);
            serializer.SerializeValue(ref LobbyPlayerNumber);
        }

        public bool Equals(SessionPlayerData other)
        {
            return UID.Equals(other.UID)
                && DisplayName.Equals(other.DisplayName)
                && IsHost == (other.IsHost)
                && LobbyPlayerNumber == (other.LobbyPlayerNumber);
        }
    }
}
