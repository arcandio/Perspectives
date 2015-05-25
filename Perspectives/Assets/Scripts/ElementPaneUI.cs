using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ElementPaneUI : MonoBehaviour {
    public RectTransform elementListPanel;
    public RectTransform elementAttributesListPanel;
    public Text elementNameText;

    public List<Element> allElements;
    public List<Element> selectedElements;

    public static ElementPaneUI elementUI;

    void Awake()
    {
        elementUI = this;
        allElements = new List<Element>();
    }

    public void SelectElement(Element e)
    {
        bool additive = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftShift);
        SelectElement(e, additive);
    }

    void SelectElement(Element e, bool additive)
    {
        if (additive)
        {
            SelectElementAddititve(e);
        }
        else
        {
            SelectElementExclusive(e);
        }
    }

    void SelectElementAddititve(Element e)
    {
        if(!selectedElements.Contains(e))
        {
            selectedElements.Add(e);
        }
    }
    void SelectElementExclusive(Element e)
    {
        if (!(selectedElements.Contains(e) && selectedElements.Count == 1))
        {
            ClearSelection();
            selectedElements.Add(e);
        }
    }
    public void AddElement(Element e)
    {
        if (!allElements.Contains(e))
        {
            allElements.Add(e);
        }
    }
    public void RemoveElement(Element e)
    {
        allElements.Remove(e);
        allElements.RemoveAll(null);
    }

    public void ClearSelection()
    {
        selectedElements = new List<Element>();
    }
}
