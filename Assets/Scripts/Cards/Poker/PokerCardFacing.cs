using UnityEngine;

namespace Game.Cards.Poker
{
    public class PokerCardFacing : CardFacing
    {

        public GameObject front;
        public GameObject back;

        public override void SwapCardFace(CardDefinition data)
        {
            if (data is PokerCardDefinition pokerCard)
            {
                pokerCard.CardFacingUp = (!pokerCard.CardFacingUp);
                front.SetActive(pokerCard.CardFacingUp);
                back.SetActive(!pokerCard.CardFacingUp);
            }
        }

        public override void SetCardFaceUp(CardDefinition data)
        {
            if (data is PokerCardDefinition pokerCard)
            {
                pokerCard.CardFacingUp = true;
                front.SetActive(true);
                back.SetActive(false);
            }
        }

        public override void SetCardFaceDown(CardDefinition data)
        {
            if (data is PokerCardDefinition pokerCard)
            {
                pokerCard.CardFacingUp = false;
                front.SetActive(false);
                back.SetActive(true);
            }
        }
    }
}
