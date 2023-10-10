using UnityEngine;

public class SortingOrderUtils
{
    public static void SetMinSortingOrderRecursively(
        GameObject gameObject,
        int minSortingOrder)
    {
        int currentMinSortingOrder = GetMinSortingOrderRecursively(gameObject);
        int changeSortingOrderBy = minSortingOrder - currentMinSortingOrder;
        SetSortingOrderRecursively(gameObject, changeSortingOrderBy);
    }

    public static void SetSortingOrderRecursively(
        GameObject gameObject,
        int changeSortingOrderBy)
    {
        SetToSpriteRenderer(gameObject, changeSortingOrderBy);
        SetToMeshRenderer(gameObject, changeSortingOrderBy);
        SetToSpriteMask(gameObject, changeSortingOrderBy);
        SetToTrailRenderer(gameObject, changeSortingOrderBy);
        SetToParticleSystemRenderer(gameObject, changeSortingOrderBy);

        foreach (Transform child in gameObject.transform)
        {
            SetSortingOrderRecursively(child.gameObject, changeSortingOrderBy);
        }
    }

    public static int GetMinSortingOrderRecursively(GameObject gameObject)
    {
        int minSortingOrder = int.MaxValue;
        int currentSortingOrder;

        currentSortingOrder = GetSpriteRendererSortingOrder(gameObject);
        if (currentSortingOrder < minSortingOrder)
        {
            minSortingOrder = currentSortingOrder;
        }
        currentSortingOrder = GetMeshRendererSortingOrder(gameObject);
        if (currentSortingOrder < minSortingOrder)
        {
            minSortingOrder = currentSortingOrder;
        }
        currentSortingOrder = GetTrailRendererSortingOrder(gameObject);
        if (currentSortingOrder < minSortingOrder)
        {
            minSortingOrder = currentSortingOrder;
        }
        currentSortingOrder = GetSpriteMaskMinSortingOrder(gameObject);
        if (currentSortingOrder < minSortingOrder)
        {
            minSortingOrder = currentSortingOrder;
        }
        currentSortingOrder = GetParticleSystemSortingOrder(gameObject);
        if (currentSortingOrder < minSortingOrder)
        {
            minSortingOrder = currentSortingOrder;
        }

        foreach (Transform child in gameObject.transform)
        {
            currentSortingOrder
                = GetMinSortingOrderRecursively(child.gameObject);
            if (currentSortingOrder < minSortingOrder)
            {
                minSortingOrder = currentSortingOrder;
            }
        }

        return minSortingOrder;
    }

    public static int GetMaxSortingOrderRecursively(GameObject gameObject)
    {
        int maxSortingOrder = int.MinValue;
        int currentSortingOrder;

        currentSortingOrder = GetSpriteRendererSortingOrder(gameObject);
        if (currentSortingOrder > maxSortingOrder)
        {
            maxSortingOrder = currentSortingOrder;
        }
        currentSortingOrder = GetMeshRendererSortingOrder(gameObject);
        if (currentSortingOrder > maxSortingOrder)
        {
            maxSortingOrder = currentSortingOrder;
        }
        currentSortingOrder = GetTrailRendererSortingOrder(gameObject);
        if (currentSortingOrder > maxSortingOrder)
        {
            maxSortingOrder = currentSortingOrder;
        }
        currentSortingOrder = GetSpriteMaskMinSortingOrder(gameObject);
        if (currentSortingOrder > maxSortingOrder)
        {
            maxSortingOrder = currentSortingOrder;
        }
        currentSortingOrder = GetParticleSystemSortingOrder(gameObject);
        if (currentSortingOrder > maxSortingOrder)
        {
            maxSortingOrder = currentSortingOrder;
        }

        foreach (Transform child in gameObject.transform)
        {
            currentSortingOrder
                = GetMinSortingOrderRecursively(child.gameObject);
            if (currentSortingOrder > maxSortingOrder)
            {
                maxSortingOrder = currentSortingOrder;
            }
        }

        return maxSortingOrder;
    }

    private static int GetSpriteRendererSortingOrder(GameObject gameObject)
    {
        SpriteRenderer spriteRenderer
            = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            return spriteRenderer.sortingOrder;
        }
        return int.MaxValue;
    }

    private static int GetMeshRendererSortingOrder(GameObject gameObject)
    {
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            return meshRenderer.sortingOrder;
        }
        return int.MaxValue;
    }

    private static int GetTrailRendererSortingOrder(GameObject gameObject)
    {
        TrailRenderer trailRenderer = gameObject.GetComponent<TrailRenderer>();
        if (trailRenderer != null)
        {
            return trailRenderer.sortingOrder;
        }
        return int.MaxValue;
    }

    private static int GetSpriteMaskMinSortingOrder(GameObject gameObject)
    {
        SpriteMask spriteMask
            = gameObject.GetComponent<SpriteMask>();
        if (spriteMask != null)
        {
            return spriteMask.backSortingOrder;
        }
        return int.MaxValue;
    }

    private static int GetParticleSystemSortingOrder(GameObject gameObject)
    {
        ParticleSystemRenderer particleSystemRenderer
            = gameObject.GetComponent<ParticleSystemRenderer>();
        if (particleSystemRenderer != null)
        {
            return particleSystemRenderer.sortingOrder;
        }
        return int.MaxValue;
    }

    private static void SetToSpriteRenderer(
        GameObject gameObject,
        int changeSortingOrderBy)
    {
        SpriteRenderer spriteRenderer
            = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = GetOrderValue(
                changeSortingOrderBy, spriteRenderer.sortingOrder
            );
        }
    }

    private static void SetToMeshRenderer(
        GameObject gameObject,
        int changeSortingOrderBy)
    {
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.sortingOrder = GetOrderValue(
                changeSortingOrderBy, meshRenderer.sortingOrder
            );
        }
    }

    private static void SetToTrailRenderer(
        GameObject gameObject,
        int changeSortingOrderBy)
    {
        TrailRenderer trailRenderer = gameObject.GetComponent<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.sortingOrder = GetOrderValue(
                changeSortingOrderBy, trailRenderer.sortingOrder
            );
        }
    }

    /// <summary>
    /// The order of operations matters here. Unity automatically subtracts from
    /// back sorting order if it is bigger than front sorting order.
    /// </summary>
    private static void SetToSpriteMask(
        GameObject gameObject,
        int changeSortingOrderBy)
    {
        SpriteMask spriteMask
            = gameObject.GetComponent<SpriteMask>();
        if (spriteMask != null)
        {
            if (changeSortingOrderBy >= 0)
            {
                spriteMask.frontSortingOrder = GetOrderValue(
                    changeSortingOrderBy, spriteMask.frontSortingOrder
                );
                spriteMask.backSortingOrder = GetOrderValue(
                    changeSortingOrderBy, spriteMask.backSortingOrder
                );
            }
            else
            {
                spriteMask.backSortingOrder = GetOrderValue(
                    changeSortingOrderBy, spriteMask.backSortingOrder
                );
                spriteMask.frontSortingOrder = GetOrderValue(
                    changeSortingOrderBy, spriteMask.frontSortingOrder
                );
            }
        }
    }

    private static void SetToParticleSystemRenderer(
      GameObject gameObject,
      int changeSortingOrderBy)
    {
        ParticleSystemRenderer particleSystemRenderer
            = gameObject.GetComponent<ParticleSystemRenderer>();
        if (particleSystemRenderer != null)
        {
            particleSystemRenderer.sortingOrder = GetOrderValue(
                changeSortingOrderBy, particleSystemRenderer.sortingOrder
            );
        }
    }

    private static int GetOrderValue(
        int newOrder,
        int initOrder)
    {
        return newOrder + initOrder;
    }
}
