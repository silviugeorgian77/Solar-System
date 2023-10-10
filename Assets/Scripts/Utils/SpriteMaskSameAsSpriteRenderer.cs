using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(SpriteMask))]
[ExecuteInEditMode]
public class SpriteMaskSameAsSpriteRenderer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpriteMask spriteMask;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteMask = GetComponent<SpriteMask>();
    }

    private void Update()
    {
        if (spriteMask.sprite != spriteRenderer.sprite)
        {
            spriteMask.sprite = spriteRenderer.sprite;
        }
    }
}
