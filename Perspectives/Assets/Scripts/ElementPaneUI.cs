using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementPaneUI : MonoBehaviour {
    public RectTransform elementListPanel;
    public RectTransform elementAttributesListPanel;
    public Text elementNameText;

    public Transform efeListTransform;
    public List<ElementFieldEditor> elementFieldEditors;
    public Text efeTitle;

    public Button ElementListButtonPrototype;
    public Button AttributeListControl;

    public static ElementPaneUI elementUI;

    public Color colorUnselected;
    public Color colorSelected;

    public DropDownList dropdown;
    public PerspectiveList perspectiveList;
    public ColorSelector colorSelector;
    public CustomFieldList customFieldList;

    public FileBrowser fileBrowser;

    public FileData initialFile;

    void Awake()
    {
        // Create a blank new file
        FileData.currentFile = initialFile;
        FileData.files.Add(initialFile);

        elementUI = this;
        FileData.currentFile.allElements = new List<Element>();

        // Setup the correct number of field iu elements
        elementFieldEditors = new List<ElementFieldEditor>();
        foreach (Transform child in efeListTransform)
        {
            elementFieldEditors.Add(child.GetComponent<ElementFieldEditor>());
        }
        // setup the field editors
        for (int i = 1; i < 10; i++)
        {
            ElementFieldType type = (ElementFieldType)i;
            //Debug.Log(type.ToString());
            elementFieldEditors[i - 1].SetupField(type);
        }
        SetupFields(false);

        fileBrowser.popupCanvas.gameObject.SetActive(false);
    }

    public void SelectElement(Element e)
    {
        bool additive = Input.GetButton("Control") || Input.GetButton("Shift");
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
        if (!FileData.currentFile.selectedElements.Contains(e))
        {
            FileData.currentFile.selectedElements.Add(e);
            HighlightSelected();
            SetupFields(true);
        }
    }
    void SelectElementExclusive(Element e)
    {
        if (!(FileData.currentFile.selectedElements.Contains(e) && FileData.currentFile.selectedElements.Count == 1))
        {
            ClearSelection();
            FileData.currentFile.selectedElements.Add(e);
            HighlightSelected();
            SetupFields(true);
        }
    }

    void HighlightSelected()
    {
        foreach (Element element in FileData.currentFile.selectedElements)
        {
            element.interaction.Selected = true;
        }
    }

    void SetupFields (bool show)
    {
        foreach (ElementFieldEditor efe in elementFieldEditors)
        {
            efe.gameObject.SetActive(show);
            efe.ElementToField();
        }
        efeTitle.gameObject.SetActive(show);
        if (show)
        {
            UpdateAttributeHeader();
        }
    }

    public void UpdateAttributeHeader()
    {
        // update the ui header
        bool nodes = false;
        bool edges = false;
        efeTitle.text = FileData.currentFile.selectedElements[0].Content;

        foreach (Element e in FileData.currentFile.selectedElements)
        {
            if (e.elementType == ElementType.Node)
            {
                nodes = true;
            }
            else if (e.elementType == ElementType.Edge)
            {
                edges = true;
            }
        }
        string selectionType = "";
        if (edges && nodes)
        {
            selectionType = "mixed";
            // turn off the subtype field
            foreach (ElementFieldEditor efe in elementFieldEditors)
            {
                if (efe.fieldName.text == "SubType")
                {
                    efe.gameObject.SetActive(false);
                }
            }
        }
        else if (edges)
        {
            selectionType = "edge";
        }
        else
        {
            selectionType = "node";
        }
        if (FileData.currentFile.selectedElements.Count > 1 && !(edges && nodes))
        {
            selectionType += "s";
        }

        efeTitle.text += " (" + selectionType + ")";
    }

    public void AddElement(Element e)
    {
        if (!FileData.currentFile.allElements.Contains(e))
        {
            FileData.currentFile.allElements.Add(e);
            GenerateElementList();
        }
    }

    public void RemoveElement(Element e)
    {
        FileData.currentFile.allElements.Remove(e);
        FileData.currentFile.allElements.RemoveAll(null);
        GenerateElementList();
    }

    void GenerateElementList()
    {
        // Get list of all children
        List<Transform> uiButtons = elementListPanel.transform.ListChildren();
        
        // resize the children list to the element list
        //uiButtons.Capacity = allElements.Count;

        // iterate through the ELEMENTS and apply them to the child toggles.

        for (int i = 0; i < FileData.currentFile.allElements.Count; i++)
        {
            // this element
            Element element = FileData.currentFile.allElements[i];
            
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
            uiButton.element.interaction.uiButton = uiButton;
        }
        // now go through and turn off all toggles that are beyond the range of the children
        if (uiButtons.Count > FileData.currentFile.allElements.Count)
        {
            for (int i = FileData.currentFile.allElements.Count - 1; i < uiButtons.Count; i++)
            {
                uiButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void ClearSelection()
    {
        // tell all elements they're deselected
        foreach (Element element in FileData.currentFile.selectedElements)
        {
            element.interaction.Selected = false;
        }

        FileData.currentFile.selectedElements = new List<Element>();
        SetupFields(false);
    }
}
