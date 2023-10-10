using UnityEngine;

public class ColorUtils
{
    public static string ColorToRGBAString(Color color)
    {
        return "#" + ColorUtility.ToHtmlStringRGBA(color);
    }

    public static Color ColorFromRGBAString(string colorString)
    {
        Color color;
        ColorUtility.TryParseHtmlString(colorString, out color);
        return color;
    }

    public static bool AreColorArraysTheSame(Color[] colors1,
        Color[] colors2,
        int precision)
    {
        if ((colors1 != null && colors2 == null)
            || (colors2 != null && colors1 == null))
        {
            return false;
        }
        if (colors1.Length == colors2.Length)
        {
            for (int i = 0; i < colors1.Length; i++)
            {
                for (int j = 0; j < colors2.Length; j++)
                {
                    if (!AreColorsTheSame(colors1[i], colors2[i], precision))
                    {
                        return false;
                    }
                }
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Checks if the colors are the samme, given a precision.
    /// </summary>
    /// <param name="c1">First color</param>
    /// <param name="c2">Second color</param>
    /// <param name="precision">The bigger this int, the lower the precision
    /// of the color comparison.
    /// A value of 0 means that we check for exact matching.
    /// A value of 255 or more means that we loosen the strictness of
    /// the match.</param>
    /// <returns>Returns true if the values are the same or similar.</returns>
    public static bool AreColorsTheSame(Color c1, Color c2, int precision)
    {
        Color32 cc1 = c1;
        Color32 cc2 = c2;
        return Mathf.Abs(cc1.r - cc2.r) <= precision
            && Mathf.Abs(cc1.g - cc2.g) <= precision
            && Mathf.Abs(cc1.b - cc2.b) <= precision
            && Mathf.Abs(cc1.a - cc2.a) <= precision;
    }

    public static Vector4[] GetVector4ArrayFromColorArray(Color[] colors)
    {
        Vector4[] colorsVector4 = new Vector4[colors.Length];
        Color currentColor;
        for (int i = 0; i < colorsVector4.Length; i++)
        {
            currentColor = colors[i];
            colorsVector4[i] = new Vector4(
                currentColor.r,
                currentColor.g,
                currentColor.b,
                currentColor.a
            );
        }
        return colorsVector4;
    }
}
