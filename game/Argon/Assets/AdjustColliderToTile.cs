using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class AdjustColliderToTileWithTopOffset : MonoBehaviour
{
    public float topOffset = 0.1f; // Adjust this value to set the desired top offset

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        
        if (spriteRenderer != null && boxCollider != null)
        {
            boxCollider.size = new Vector2(spriteRenderer.size.x, spriteRenderer.size.y - topOffset);
            // Adjust the offset to account for the top offset. The offset moves the collider center.
            // Since we're reducing the collider size from the top, we need to move the collider center up by half of the topOffset.
            boxCollider.offset = new Vector2(0, -topOffset / 2);
        }
    }
}
