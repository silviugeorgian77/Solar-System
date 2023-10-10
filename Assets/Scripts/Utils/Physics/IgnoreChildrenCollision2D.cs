using UnityEngine;

public class IgnoreChildrenCollision2D : MonoBehaviour
{
    private Collider2D[] colliders;
    public bool ignore = true;

    private bool lastIgnore;

    private void Awake()
    {
        RefreshColliders();
    }

    private void Update()
    {
        if (ignore != lastIgnore)
        {
            IgnoreColliders(ignore);
            lastIgnore = ignore;
        }
    }

    public void RefreshColliders()
    {
        colliders = GetComponentsInChildren<Collider2D>();
    }

    public void IgnoreColliders(bool ignore)
    {
        foreach (Collider2D collider1 in colliders)
        {
            foreach (Collider2D collider2 in colliders)
            {
                Physics2D.IgnoreCollision(collider1, collider2, ignore);
            }
        }
    }
}
