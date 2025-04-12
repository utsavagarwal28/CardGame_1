using UnityEngine;
using Game.Cards.Poker.Variables;

namespace Game.Cards.Poker
{
    [CreateAssetMenu(fileName = "PokerCardDefinition", menuName = "Game/Cards/Poker/PokerCardDefinition")]
    public class PokerCardDefinition : CardDefinition
    {
        public PokerSuit Suit;

        public PokerRank Rank;

        public Sprite FrontArt;

        public bool isTrump;
            
    }    
}
