using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ElementFieldEditor : MonoBehaviour {
    ElementPaneUI elementUi;
    [SerializeField]
    ElementFieldType fieldType = ElementFieldType.None;

    public Text fieldName;

    public RectTransform fieldsTransform;
    public InputField inputField;
    public Toggle toggleField;
    
    public Toggle dropdownField;
    public Text dropdownText;

    public RectTransform perspectives;
    public PerspectiveList perspectiveList;
    public RectTransform customFields;

    public RectTransform dateTimePicker;
    public InputField yearField;
    public InputField monthField;
    public InputField dayField;
    public InputField hourField;
    public InputField minuteField;
    public InputField secondField;

    DropDownList dropdown;

    /// <summary>
    /// Called by changing the selection in ElementPanelUI. 
    /// </summary>
    /// <param name="n">name of field</param>
    /// <param name="t">type of field</param>
    public void SetupField(ElementFieldType t)
    {
        elementUi = ElementPaneUI.elementUI;
        dropdown = elementUi.dropdown;
        fieldType = t;
        fieldName.text = t.ToString();
        gameObject.name = "FIELD: " + fieldName.text;

        // deactivate all fields
        foreach (Transform child in fieldsTransform.transform)
        {
            child.gameObject.SetActive(false);
        }

        // Activate the correct fields based on the type
        switch (fieldType)
        {
            case ElementFieldType.Content:
                inputField.gameObject.SetActive(true);
                inputField.lineType = InputField.LineType.MultiLineSubmit;
                break;
            case ElementFieldType.Description:
                inputField.gameObject.SetActive(true);
                inputField.lineType = InputField.LineType.MultiLineSubmit;
                break;
            case ElementFieldType.SubType:
                dropdownField.gameObject.SetActive(true);
                break;
            case ElementFieldType.Perspectives:
                elementUi.perspectiveList.transform.SetParent(fieldsTransform, false);
                elementUi.perspectiveList.gameObject.SetActive(true);
                elementUi.perspectiveList.efe = this;
                perspectiveList = elementUi.perspectiveList;
                break;
            case ElementFieldType.Start:
            case ElementFieldType.End:
                dateTimePicker.gameObject.SetActive(true);
                break;
            case ElementFieldType.Age:
                inputField.gameObject.SetActive(true);
                break;
            case ElementFieldType.Color:
                elementUi.colorSelector.transform.SetParent(fieldsTransform, false);
                elementUi.colorSelector.gameObject.SetActive(true);
                elementUi.colorSelector.efe = this;
                elementUi.colorSelector.elementUi = ElementPaneUI.elementUI;
                elementUi.colorSelector.SetupColorButtons();
                break;
            case ElementFieldType.CustomFields:
                customFields.gameObject.SetActive(true);
                break;
        }

        ElementToField();
    }

    public void ElementToField ()
    {
        // abort if there's no selecttion
        if (elementUi.selectedElements.Count == 0)
        {
            return;
        }
        // set the field from the element's data
        switch (fieldType)
        {
            case ElementFieldType.Content:
                inputField.text = elementUi.selectedElements[0].Content;
                break;
            case ElementFieldType.Description:
                inputField.text = elementUi.selectedElements[0].description;
                break;
            case ElementFieldType.SubType:
                dropdownText.text = elementUi.selectedElements[0].elementSubType;
                break;
            case ElementFieldType.Perspectives:
                perspectiveList.ResetList();
                break;
            case ElementFieldType.Start:
                break;
            case ElementFieldType.End:
                break;
            case ElementFieldType.Age:
                break;
            case ElementFieldType.Color:
                break;
            case ElementFieldType.CustomFields:
                break;
        }
    }

    public void FieldToElement()
    {
        foreach (Element e in elementUi.selectedElements)
        {
            switch (fieldType)
            {
                case ElementFieldType.Content:
                    e.Content = inputField.text;
                    elementUi.UpdateAttributeHeader();
                    break;
                case ElementFieldType.Description:
                    e.description = inputField.text;
                    break;
                case ElementFieldType.SubType:
                    DisplayDropdown(dropdownField.isOn, elementUi.selectedElements[0].elementType.ToString());
                    break;
                case ElementFieldType.Perspectives:
                    break;
                case ElementFieldType.Start:
                    break;
                case ElementFieldType.End:
                    break;
                case ElementFieldType.Age:
                    break;
                case ElementFieldType.Color:
                    break;
                case ElementFieldType.CustomFields:
                    break;
            }
        }
    }

    public void DisplayDropdown (bool show, string selected)
    {
        if (show)
        {
            dropdown.gameObject.SetActive(true);
            List<string> combinedList = new List<string>();
            if (elementUi.selectedElements[0].elementType == ElementType.Node)
            {
                combinedList.AddRange(elementUi.nodeTypesDefault);
                combinedList.AddRange(elementUi.nodeTypesCustom);
            }
            else
            {
                combinedList.AddRange(elementUi.edgeTypesDefault);
                combinedList.AddRange(elementUi.edgeTypesCustom);
            }

            dropdown.ResetList(gameObject.GetComponent<RectTransform>(), combinedList);
            dropdown.efe = this;
            dropdown.current = selected;
        }
        else
        {
            dropdown.gameObject.SetActive(false);
            dropdownField.isOn = false;
        }
    }

    public void RecieveDropdown(string val)
    {
        foreach (Element e in elementUi.selectedElements)
        {
            e.elementSubType = val;
        }
        ElementToField();
    }

    public void RecievePerspective(List<Perspective> val)
    {
        foreach (Element e in elementUi.selectedElements)
        {
            e.perspectives = val;
        }
        //ElementToField();
    }
    public void RecieveColor(Color val)
    {
        foreach (Element e in elementUi.selectedElements)
        {
            e.Color = val;
        }
    }
}

public enum ElementFieldType 
{
    None,
    Content,
    Description,
    SubType, //dropdown
    Perspectives, // need special editor for this
    Start,
    End,
    Age,
    Color, // need special editor for this
    CustomFields // need special editor for this
}