using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementPaneUI : MonoBehaviour {
    public RectTransform elementListPanel;
    public RectTransform elementAttributesListPanel;
    public Text elementNameText;

    [SerializeField]
    List<Element> allElements;
    public List<Element> selectedElements;

    public Button ElementListButtonPrototype;
    public Button AttributeListControl;

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
            GenerateElementList();
        }
    }

    public void RemoveElement(Element e)
    {
        allElements.Remove(e);
        allElements.RemoveAll(null);
        GenerateElementList();
    }

    void GenerateElementList()
    {
        // Get list of all children
        List<Transform> uiButtons = elementListPanel.transform.ListChildren();
        
        // resize the children list to the element list
        //uiButtons.Capacity = allElements.Count;

        // iterate through the ELEMENTS and apply them to the child buttons.

        for (int i = 0; i < allElements.Count; i++)
        {
            // this element
            Element element = allElements[i];
            
            // check if the button in that slot even exists
            if (i > uiButtons.Count - 1)
            {
                // clone the button
                GameObject clone = Instantiate<GameObject>(ElementListButtonPrototype.gameObject);
                // Set the parent
                clone.transform.SetParent(elementListPanel, false);
                // add it to the children list
                uiButtons.Add(clone.transform);
            }

            // apply the element to that button
            UiButton uiButton = uiButtons[i].GetComponent<UiButton>();
            uiButton.gameObject.SetActive(true);
            uiButton.element = element;
            uiButton.buttonText.text = element.name;
            uiButton.SetIcon();
        }
        // now go through and turn off all buttons that are beyond the range of the children
        if (uiButtons.Count > allElements.Count)
        {
            for (int i = allElements.Count - 1; i < uiButtons.Count; i++)
            {
                uiButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void ClearSelection()
    {
        selectedElements = new List<Element>();
    }
}
