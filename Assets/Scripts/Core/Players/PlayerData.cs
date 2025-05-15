using UnityEngine;
using Unity.Services.Lobbies.Models;
using LobbyClass = Unity.Services.Lobbies.Models.Lobby;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public LobbyClass CurrentLobby { get; private set; }
    Player player;



}
