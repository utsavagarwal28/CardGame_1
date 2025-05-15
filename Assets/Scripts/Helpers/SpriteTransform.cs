using UnityEngine;

namespace Game.Helpers
{
    public class SpriteTransform
    {
        public void SetSpriteRendererOpaque(SpriteRenderer spriteRenderer)
        {
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = 1f;
                spriteRenderer.color = color;
            }
        }

        public void SetSpriteRendererSortingOrder(SpriteRenderer spriteRenderer, int OrderInLayer)
        {

            if (spriteRenderer != null)
                spriteRenderer.sortingOrder = OrderInLayer;
        }
    }
}
