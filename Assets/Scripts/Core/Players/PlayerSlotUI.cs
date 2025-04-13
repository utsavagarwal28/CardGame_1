using UnityEngine;
using TMPro;

namespace Game.Core.Players
{
    public class PlayerSlotUI : MonoBehaviour
    {
        public TextMeshPro nameText;

        public void AssignPlayer(SessionPlayerData data)
        {
            nameText.text = data.DisplayName;
        }
    }
}
