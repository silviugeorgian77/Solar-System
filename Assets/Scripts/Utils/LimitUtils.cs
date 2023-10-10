using System.Collections.Generic;
using UnityEngine;

public class LimitUtils
{
    public enum Mode
    {
        LOCAL,
        GLOBAL
    }

    public static Vector4 GetLimits(Mode mode, List<Transform> limitTransforms)
    {
        Vector4 limits = new Vector4();
        limits.w = GetMinX(mode, limitTransforms);
        limits.x = GetMaxX(mode, limitTransforms);
        limits.y = GetMinY(mode, limitTransforms);
        limits.z = GetMaxY(mode, limitTransforms);
        return limits;
    }

    public static float GetMinX(Mode mode, List<Transform> limitTransforms)
    {
        var minX = float.MaxValue;
        foreach (var limitTransform in limitTransforms)
        {
            if (mode == Mode.LOCAL)
            {
                if (limitTransform.localPosition.x < minX)
                {
                    minX = limitTransform.localPosition.x;
                }
            }
            else
            {
                if (limitTransform.position.x < minX)
                {
                    minX = limitTransform.position.x;
                }
            }
        }
        return minX;
    }

    public static float GetMaxX(Mode mode, List<Transform> limitTransforms)
    {
        var maxX = float.MinValue;
        foreach (var limitTransform in limitTransforms)
        {
            if (mode == Mode.LOCAL)
            {
                if (limitTransform.localPosition.x > maxX)
                {
                    maxX = limitTransform.localPosition.x;
                }
            }
            else
            {
                if (limitTransform.position.x > maxX)
                {
                    maxX = limitTransform.position.x;
                }
            }
        }
        return maxX;
    }

    public static float GetMinY(Mode mode, List<Transform> limitTransforms)
    {
        var minY = float.MaxValue;
        foreach (var limitTransform in limitTransforms)
        {
            if (mode == Mode.LOCAL)
            {
                if (limitTransform.localPosition.y < minY)
                {
                    minY = limitTransform.localPosition.y;
                }
            }
            else
            {
                if (limitTransform.position.y < minY)
                {
                    minY = limitTransform.position.y;
                }
            }
        }
        return minY;
    }

    public static float GetMaxY(Mode mode, List<Transform> limitTransforms)
    {
        var maxY = float.MinValue;
        foreach (var limitTransform in limitTransforms)
        {
            if (mode == Mode.LOCAL)
            {
                if (limitTransform.localPosition.y > maxY)
                {
                    maxY = limitTransform.localPosition.y;
                }
            }
            else
            {
                if (limitTransform.position.y > maxY)
                {
                    maxY = limitTransform.position.y;
                }
            }
        }
        return maxY;
    }
}
