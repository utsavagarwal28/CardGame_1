using UnityEngine;
using System.Collections.Generic;
using Game.Cards.Poker;

namespace Game.Cards.Spawner
{
    public class CardSpawner : MonoBehaviour
    {
        public GameObject pokerCardPrefab;
        public List<PokerCardDefinition> cardDefinition;

        void Start()
        {
            for (int i = 0; i < cardDefinition.Count; i++)
            {
                var cardObj = Instantiate(pokerCardPrefab, new Vector3(i*2f, 0, 0), Quaternion.identity);
                var pokerCard = cardObj.GetComponent<PokerCard>();
                pokerCard.Apply(cardDefinition[i]);

                //Debug.Log($"Spawned Card: {cardDefinition[i].Suit} {cardDefinition[i].Rank}");
            }
        }
    }   
}
