using UnityEngine;

namespace Game.Cards
{
    public abstract class CardSetup : MonoBehaviour
    {
        public abstract void Apply(CardDefinition data);
    }
}
