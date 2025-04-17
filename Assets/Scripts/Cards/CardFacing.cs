using UnityEngine;

namespace Game.Cards
{
    public abstract class CardFacing : MonoBehaviour
    {
        public abstract void SwapCardFace(CardDefinition data);

        public abstract void SetCardFaceUp(CardDefinition data);

        public abstract void SetCardFaceDown(CardDefinition data);
    }
}
