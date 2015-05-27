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

}
