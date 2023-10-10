using UnityEngine;

public class ScaleRelativeTo : MonoBehaviour
{
    public float scaleX;
    public float scaleY;
    public Transform relativeToTransform;

    private void Update()
    {
        if (relativeToTransform != null)
        {
            transform.localScale = new Vector3(
                relativeToTransform.localScale.x * scaleX,
                relativeToTransform.localScale.y * scaleY,
                relativeToTransform.localScale.z
            );
        }
    }
}
