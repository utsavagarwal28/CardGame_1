using UnityEngine;

namespace Game.Cards
{
    //Base definition of a card;
    //Used by all types of cards i.e. Poker, Rummy, Solitaire, etc.
    public class CardDefinition : ScriptableObject
    {
        [Tooltip("Back art for the card")]
        public Sprite BackArt;

        public bool CardFacingUp = false;
    }
}
