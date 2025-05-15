using UnityEngine;
using System.Collections.Generic;
using Game.Core.Players;
using System;

namespace Game.Core
{
    public class GameSetup : MonoBehaviour
    {
        [Header("Player Slot References")]
        public List<PlayerSlotUI> playerSlots = new List<PlayerSlotUI>();

        public Action OnGameSetup;

        public int localPlayerNumber;

        private void Awake()
        {

        }

        public void OnEnable()
        {
            //OnGameSetup += SetupPlayerSlots;

            //OnGameSetup?.Invoke();
        }



        //private void SetupPlayerSlots()
        //{
        //    List<SessionPlayerData> originalList = GameManager.SessionPlayerDataList;

        //    foreach (SessionPlayerData sessionPlayer in originalList)
        //        Debug.Log(sessionPlayer.DisplayName);

        //    if (originalList == null || originalList.Count != 4)
        //    {
        //        Debug.LogError("Player data missing or incomplete!");
        //        return;
        //    }

        //    // Rotated player list based on localPlayerNumber
        //    List<SessionPlayerData> rotatedList = new List<SessionPlayerData>();

        //    for (int i = 0; i < originalList.Count; i++)
        //    {
        //        int index = (localPlayerNumber + i) % originalList.Count;
        //        rotatedList.Add(originalList[index]);
        //    }

        //    //Assign player data to slot UIs
        //    for (int i = 0; i < playerSlots.Count; i++)
        //    {
        //        playerSlots[i].AssignPlayer(rotatedList[i]);
        //    }

        //    Debug.Log("Game setup complete for local player: " + localPlayerNumber);
        //}
    }
}
