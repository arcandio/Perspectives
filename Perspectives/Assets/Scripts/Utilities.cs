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
}
