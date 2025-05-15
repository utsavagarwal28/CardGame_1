using UnityEngine;
using System.Collections.Generic;

namespace Game.Cards.Poker.Deck
{
    public class PokerDeckManager : MonoBehaviour
    {
        // Setup Singleton Class
        public static PokerDeckManager Instance;

        public List<GameObject> shuffledPokerDeck = new();

        public Transform stackParent;

        public bool hasDeckBeenShuggled = false;

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {

        }

        public void SpawnShuffledPokerDeck()
        {
            if (shuffledPokerDeck.Count > 0)
                return;

            shuffledPokerDeck.Clear();

            float stackOffset = 0.001f;
            float currentX = 0f + (PokerDeckSpawner.newPokerDeck.Count - 1) / 2 * stackOffset;
            float currentY = 0f + (PokerDeckSpawner.newPokerDeck.Count - 1) / 2 * stackOffset;
            float currentZ = 0f + (PokerDeckSpawner.newPokerDeck.Count - 1) / 2 * stackOffset;

            for (int i = PokerDeckSpawner.newPokerDeck.Count - 1; i > -1; i--)
            {
                int index = Random.Range(0, PokerDeckSpawner.newPokerDeck.Count);
                var card = PokerDeckSpawner.newPokerDeck[index];

                shuffledPokerDeck.Add(card);
                PokerDeckSpawner.newPokerDeck.Remove(card);

                card.transform.SetParent(stackParent);
                card.transform.localScale = new Vector3(0.07f, 0.045f, 1f);
                card.transform.localPosition = new Vector3(currentX, currentY, currentZ);

                currentX -= stackOffset;
                currentY -= stackOffset;
                currentZ -= stackOffset;
                Debug.Log(card);
            }

            for (int i = shuffledPokerDeck.Count; i > -1; i--)
            {
                Debug.Log($"Shuffled Card: {shuffledPokerDeck[i].GetComponent<PokerCardSetup>().Suit} {shuffledPokerDeck[i].GetComponent<PokerCardSetup>().Rank}");
            }
        }
    }
}
