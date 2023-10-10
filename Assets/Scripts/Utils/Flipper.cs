using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    public List<Transform> flippables;
    public bool flipX;
    public bool flipY;
    public bool flipZ;

    private bool lastFlipX;
    private bool lastFlipY;
    private bool lastFlipZ;

    private void Update()
    {
        if (ShouldFlip())
        {
            Flip();
        }
    }

    private bool ShouldFlip()
    {
        return flipX != lastFlipX || flipY != lastFlipY || flipZ != lastFlipZ;
    }

    private void Flip()
    {
        lastFlipX = flipX;
        lastFlipY = flipY;
        lastFlipZ = flipZ;

        float scaleXFactor = 1;
        float scaleYFactor = 1;
        float scaleZFactor = 1;
        if (flipX) scaleXFactor = -1;
        if (flipY) scaleYFactor = -1;
        if (flipZ) scaleZFactor = -1;

        foreach (Transform transform in flippables)
        {
            transform.localScale = new Vector3(
                transform.localScale.x * scaleXFactor,
                transform.localScale.y * scaleYFactor,
                transform.localScale.z * scaleZFactor
            );
        }
    }
}
