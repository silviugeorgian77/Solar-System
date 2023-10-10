using System.Collections.Generic;
using UnityEngine;

public static class TransformUtils
{
    public static Transform FindChildRecursive(
        this Transform parent,
        string name)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(parent);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.name == name)
                return c;
            foreach (Transform t in c)
                queue.Enqueue(t);
        }
        return null;
    }

    public static Transform FindParentRecursive(
        this Transform child,
        string name)
    {
        var parent = child.parent;
        while (parent != null)
        {
            if (parent.name == name)
            {
                return parent;
            }
            parent = parent.parent;
        }
        return null;
    }

    public static Vector2 GetTotalScale(
        this Transform transform
        )
    {
        Vector2 totalScale = Vector2.one;
        while (transform != null)
        {
            totalScale *= transform.localScale;
            transform = transform.parent;
        }
        return totalScale;
    }
}
