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
        element.interaction.ClickedElement();
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
            if (selected)
            {
                background.color = ElementPaneUI.elementUI.colorSelected;
            }
            else
            {
                background.color = ElementPaneUI.elementUI.colorUnselected;
            }
        }
    }
}
