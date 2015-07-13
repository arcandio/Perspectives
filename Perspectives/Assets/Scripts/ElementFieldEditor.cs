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

    public RectTransform dateTimePicker;
    public InputField yearField;
    public InputField monthField;
    public InputField dayField;
    public InputField hourField;
    public InputField minuteField;
    public InputField secondField;

    public Text textField;

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
                textField.gameObject.SetActive(true);
                break;
            case ElementFieldType.Color:
                elementUi.colorSelector.transform.SetParent(fieldsTransform, false);
                elementUi.colorSelector.gameObject.SetActive(true);
                elementUi.colorSelector.efe = this;
                elementUi.colorSelector.elementUi = ElementPaneUI.elementUI;
                elementUi.colorSelector.SetupColorButtons();
                break;
            case ElementFieldType.CustomFields:
                elementUi.customFieldList.gameObject.SetActive(true);
                elementUi.customFieldList.transform.SetParent(fieldsTransform, false);
                elementUi.customFieldList.gameObject.SetActive(true);
                elementUi.customFieldList.efe = this;
                elementUi.customFieldList.elementUi = ElementPaneUI.elementUI;
                elementUi.customFieldList.SetupList();
                break;
        }

        ElementToField();
    }

    public void ElementToField ()
    {
        // abort if there's no selecttion
        if (FileData.currentFile.selectedElements.Count == 0)
        {
            return;
        }

        Element e = FileData.currentFile.selectedElements[0];
        // set the field from the element's data
        switch (fieldType)
        {
            case ElementFieldType.Content:
                inputField.text = e.Content;
                break;
            case ElementFieldType.Description:
                inputField.text = e.description;
                break;
            case ElementFieldType.SubType:
                dropdownText.text = e.elementSubType;
                break;
            case ElementFieldType.Perspectives:
                perspectiveList.ResetList();
                break;
            case ElementFieldType.Start:
                TimelineDateToField(e.startDate);
                UpdateAge();
                break;
            case ElementFieldType.End:
                TimelineDateToField(e.endDate);
                UpdateAge();
                break;
            case ElementFieldType.Age:
                break;
            case ElementFieldType.Color:
                elementUi.colorSelector.HighlightCurrentColor(e.Color);
                break;
            case ElementFieldType.CustomFields:
                elementUi.customFieldList.Populate(e.customFields);
                break;
        }
    }

    public void FieldToElement()
    {
        foreach (Element e in FileData.currentFile.selectedElements)
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
                    DisplayDropdown(dropdownField.isOn, FileData.currentFile.selectedElements[0].elementType.ToString());
                    break;
                case ElementFieldType.Perspectives:
                    break;
                case ElementFieldType.Start:
                    e.startDate = FieldToTimelineDate();
                    UpdateAge();
                    break;
                case ElementFieldType.End:
                    e.endDate = FieldToTimelineDate();
                    UpdateAge();
                    break;
                case ElementFieldType.Age:
                    break;
                case ElementFieldType.Color:
                    break;
                case ElementFieldType.CustomFields:
                    break;
            }
            ElementPaneUI.elementUI.GenerateElementList();
        }
    }

    TimelineDate FieldToTimelineDate()
    {
        int year = int.Parse(yearField.text);
        int month = int.Parse(monthField.text);
        int day = int.Parse(dayField.text);

        int hour = int.Parse(hourField.text);
        int minute = int.Parse(minuteField.text);
        int second = int.Parse(secondField.text);

        return new TimelineDate(year, month, day, hour, minute, second);
    }
    void TimelineDateToField(TimelineDate tld)
    {
        yearField.text = tld.year.ToString();
        monthField.text = tld.month.ToString();
        dayField.text = tld.day.ToString();

        hourField.text = tld.hour.ToString();
        minuteField.text = tld.minute.ToString();
        secondField.text = tld.second.ToString();
    }

    void UpdateAge()
    {
        Element e = FileData.currentFile.selectedElements[0];
        string a = (e.endDate - e.endDate).ToString();
        textField.text = a;
        //Debug.Log("updated age: " + textField.text);
    }

    public void DisplayDropdown (bool show, string selected)
    {
        if (show)
        {
            dropdown.gameObject.SetActive(true);
            List<string> combinedList = new List<string>();
            if (FileData.currentFile.selectedElements[0].elementType == ElementType.Node)
            {
                combinedList.AddRange(FileData.nodeTypesDefault);
                combinedList.AddRange(FileData.currentFile.nodeTypesCustom);
            }
            else
            {
                combinedList.AddRange(FileData.edgeTypesDefault);
                combinedList.AddRange(FileData.currentFile.edgeTypesCustom);
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
        foreach (Element e in FileData.currentFile.selectedElements)
        {
            e.elementSubType = val;
        }
        ElementToField();
    }

    public void RecievePerspective(List<Perspective> val)
    {
        foreach (Element e in FileData.currentFile.selectedElements)
        {
            e.perspectives = val;
        }
        //ElementToField();
    }
    public void RecieveColor(Color val)
    {
        foreach (Element e in FileData.currentFile.selectedElements)
        {
            e.Color = val;
            elementUi.colorSelector.HighlightCurrentColor(FileData.currentFile.selectedElements[0].Color);
        }
    }

    public void RecieveCustomFields(Dictionary<string, string> d)
    {
        foreach (Element e in FileData.currentFile.selectedElements)
        {
            e.customFields = d;
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