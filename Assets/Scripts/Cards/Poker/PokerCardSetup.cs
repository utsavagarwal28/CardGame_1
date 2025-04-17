using UnityEngine;
using Game.Cards.Poker.Variables;
using Game.Helpers;

namespace Game.Cards.Poker
{
    public class PokerCardSetup : CardSetup
    {
        public SpriteRenderer frontImage;
        public SpriteRenderer backImage;

        public PokerSuit Suit { get; private set; }
        public int Rank { get; private set; }

        public SpriteTransform spriteTransform;
        public int SortingOrderInLayer = 10;

        void start()
        {
            // Making the card Perfectly Opaque
            spriteTransform.SetSpriteRendererOpaque(frontImage);
            spriteTransform.SetSpriteRendererOpaque(backImage);

            // Settomg the sorting order to highest in the list
            spriteTransform.SetSpriteRendererSortingOrder(frontImage, SortingOrderInLayer);
            spriteTransform.SetSpriteRendererSortingOrder(backImage, SortingOrderInLayer);
        }

        // Automatically configures the visual elements of the card when a ScriptableObject is assigned.
        public override void Apply(CardDefinition data)
        {
            if (data is PokerCardDefinition pokerCard)
            {
                // Applies the front and back artwork to the card's GameObject.
                frontImage.sprite = pokerCard.FrontArt;
                if (pokerCard.BackArt != null)
                    backImage.sprite = pokerCard.BackArt;
                // Stores suit and rank for gameplay logic (e.g., comparison, sorting).e.
                Suit = pokerCard.Suit;
                Rank = (int)pokerCard.Rank;
            }
        }
    }
}
