using UnityEngine;
using Game.Cards.Poker.Variables;

namespace Game.Cards.Poker
{
    public class PokerCard: CardSetup
    {
        public SpriteRenderer Image;
        public SpriteRenderer BackImage;

        public PokerSuit Suit { get; private set; }
        public int Rank { get; private set; }

        // Automatically configures the visual elements of the card when a ScriptableObject is assigned.
        public override void Apply(CardDefinition data)
        {
            if (data is PokerCardDefinition pokerCard)
            {
                // Applies the front and back artwork to the card's GameObject.
                Image.sprite = pokerCard.FrontArt;
                if (pokerCard.BackArt != null)
                    BackImage.sprite = pokerCard.BackArt;
                // Stores suit and rank for gameplay logic (e.g., comparison, sorting).e.
                Suit = pokerCard.Suit;
                Rank = (int)pokerCard.Rank;
            }
        }
    }    
}
