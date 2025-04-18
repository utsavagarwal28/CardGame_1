using UnityEngine;
using System.Collections.Generic;
using Game.Cards.Poker;
using Game.Core;
using Game.Temporary;
//using Game.Helpers;

namespace Game.Cards.Poker.Deck
{
    public class PokerDeckSpawner : MonoBehaviour
    {
        // Prefab for visualization of Poker Card
        public GameObject pokerCardPrefab;
        // GaneSetup Class object to get Player 1
        //public GameSetup gameSetup;
        public int localPlayerNumber;
        // ScriptableObjects to store the data related to the cards
        public List<PokerCardDefinition> cardDefinition;
        // PokerDeckSpawner GameObject in GameScene
        // Parent for all cards in pokerDeck
        public Transform pokerDeckSpawnnerGameObject;

        // List to store all the Poker Cards
        // Keeping it static for creating a single instance for all the players
        public static List<GameObject> newPokerDeck { get; private set; } = new List<GameObject>();



        void Awake()
        {
            localPlayerNumber = PlayerTagReader.LocalPlayerNumber;
            if (newPokerDeck.Count == 0 && localPlayerNumber == 1)
                SpawnDeck();
        }


        public void SpawnDeck()
        {
            for (int i = 0; i < cardDefinition.Count; i++)
            {
                // Spawning the PokerCard Prefab as base of the card in the GameScene.
                // Then placing it as child of PokerDeckSpawner GameObject in the hierarchy.
                var cardObj = Instantiate(pokerCardPrefab, pokerDeckSpawnnerGameObject);

                // Changing the name of each card in the formate Suit_Card.
                cardObj.name = $"{cardDefinition[i].Suit}_{cardDefinition[i].Rank}";

                // Applying the particular CardDefinition data to the card spawnned.
                cardObj.GetComponent<PokerCardSetup>().Apply(cardDefinition[i]);

                cardObj.GetComponent<PokerCardFacing>().SetCardFaceDown(cardDefinition[i]);

                // Hiding the Card untill it's required
                //cardObj.SetActive(false);


                //Adding the card to the PlayerDeck List
                newPokerDeck.Add(cardObj);


            }
        }
    }
}
