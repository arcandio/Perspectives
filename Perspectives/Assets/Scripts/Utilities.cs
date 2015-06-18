using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A static class for containing our oft-used utilities.
/// </summary>
static public class Utilities {

    /// <summary>
    /// Returns a list of all children transforms
    /// </summary>
    /// <param name="parent">parent transform</param>
    /// <returns>List of child transforms</returns>
    public static List<Transform> ListChildren (this Transform parent)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            children.Add(child);
        }
        return children;
    }

    /// <summary>
    /// Position to local space of the rectTrasform
    /// </summary>
    /// <param name="position">Worldspace position</param>
    /// <param name="rectTransform">RectTransform to use</param>
    /// <returns>Screenspace position in rectTransform</returns>
    public static Vector3 MangleByTransform (this Vector3 position, RectTransform rectTransform )
    {
        Vector3 pos = position;
        pos.x = (pos.x * rectTransform.sizeDelta.x) - (rectTransform.sizeDelta.x * 0.5f);
        pos.y = (pos.y * rectTransform.sizeDelta.y) - (rectTransform.sizeDelta.y * 0.5f);
        pos.z = 0;

        return pos;
    }

    public static float Luminance(this Color c)
    {
        return c.r * 0.3f + c.g * 0.59f + c.b * 0.11f;
    }

    public static Color ColorFromHSL(float hue, float saturation, float lightness)
    {
        float chroma = (1 - Mathf.Abs(2 * lightness - 1)) * saturation;
        float prime = hue / 60;
        float x = chroma * (1 - Mathf.Abs(prime % 2 - 1));
        Color baseColor = new Color(lightness, lightness, lightness);
        if (0 <= prime && prime < 1)
        {
            baseColor = new Color(chroma, x, 0);
        }
        else if (1 <= prime && prime < 2)
        {
            baseColor = new Color(x, chroma, 0);
        }
        else if (2 <= prime && prime < 3)
        {
            baseColor = new Color(0, chroma, x);
        }
        else if (3 <= prime && prime < 4)
        {
            baseColor = new Color(0, x, chroma);
        }
        else if (4 <= prime && prime < 5)
        {
            baseColor = new Color(x, 0, chroma);
        }
        else if (5 <= prime && prime < 6)
        {
            baseColor = new Color(chroma, 0, x);
        }
        float m = lightness - 0.5f * chroma;
        Color newColor = new Color(baseColor.r + m, baseColor.g + m, baseColor.b + m);
        return newColor;
    }

}
