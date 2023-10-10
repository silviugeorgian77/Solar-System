using UnityEngine;

public class LayerUtils
{
    public static void ChangeLayersRecursively(
       GameObject gameObject,
       string name)
    {
        int layer = LayerMask.NameToLayer(name);
        ChangeLayersRecursively(gameObject, layer);
    }

    public static void ChangeLayersRecursively(
       GameObject gameObject,
       int layer)
    {
        try
        {
            gameObject.layer = layer;
        }
        catch
        {

        }
        foreach (Transform child in gameObject.transform)
        {
            ChangeLayersRecursively(child.gameObject, layer);
        }
    }

    public static void ChangeSortingLayerRecursively(
        GameObject gameObject,
        string name)
    {
        int layer = SortingLayer.NameToID(name);
        ChangeSortingLayerRecursively(gameObject, layer);
    }

    public static void ChangeSortingLayerRecursively(
        GameObject gameObject,
        int layer)
    {
        SpriteRenderer spriteRenderer
            = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerID = layer;
        }

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.sortingLayerID = layer;
        }

        Canvas canvas = gameObject.GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.sortingLayerID = layer;
        }

        SpriteMask spriteMask = gameObject.GetComponent<SpriteMask>();
        if (spriteMask != null)
        {
            spriteMask.sortingLayerID = layer;
            spriteMask.frontSortingLayerID = layer;
            spriteMask.backSortingLayerID = layer;
        }

        TrailRenderer trailRenderer = gameObject.GetComponent<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.sortingLayerID = layer;
        }

        ParticleSystemRenderer particleSystemRenderer
           = gameObject.GetComponent<ParticleSystemRenderer>();
        if (particleSystemRenderer != null)
        {
            particleSystemRenderer.sortingLayerID = layer;
        }

        foreach (Transform child in gameObject.transform)
        {
            ChangeSortingLayerRecursively(child.gameObject, layer);
        }
    }
}
