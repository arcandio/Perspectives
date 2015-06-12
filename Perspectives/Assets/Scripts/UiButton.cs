using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiButton : MonoBehaviour {
    public Text buttonText;
    public Element element;
    public Image background;
    bool selected = false;

    public void SetIcon ()
    {
        // do some stuff
    }

    public void SelectElement()
    {
        if (element.nodeInteraction != null)
        {
            element.nodeInteraction.ClickedElement();
        }
        if (element.edgeInteraction != null)
        {
            element.edgeInteraction.ClickedElement();
        }
    }
    public bool Selected
    {
        get
        {
            return selected;
        }
        set
        {
            selected = value;

        }
    }
}
