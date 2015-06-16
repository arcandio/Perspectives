using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ElementFieldEditor : MonoBehaviour {
    public ElementFieldType fieldType = ElementFieldType.None;

    public Text fieldName;

    public RectTransform fieldsTransform;
    public InputField inputField;
    public Toggle toggleField;
    public Button buttonField;

    public RectTransform dateTimePicker;
    public InputField yearField;
    public InputField monthField;
    public InputField dayField;
    public InputField hourField;
    public InputField minuteField;
    public InputField secondField;

    /// <summary>
    /// Called by changing the selection in ElementPanelUI. 
    /// </summary>
    /// <param name="n">name of field</param>
    /// <param name="t">type of field</param>
    public void SetupField(ElementFieldType t)
    {
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
                break;
            case ElementFieldType.Description:
                inputField.gameObject.SetActive(true);
                break;
            case ElementFieldType.Type:
                buttonField.gameObject.SetActive(true);
                break;
            case ElementFieldType.Perspectives:
                break;
            case ElementFieldType.Start:
            case ElementFieldType.End:
                dateTimePicker.gameObject.SetActive(true);
                break;
            case ElementFieldType.Age:
                inputField.gameObject.SetActive(true);
                break;
            case ElementFieldType.Icon:
                break;
            case ElementFieldType.Color:
                break;
            case ElementFieldType.CustomFields:
                break;
        }


        ElementToField();
    }

    public void ElementToField ()
    {
        switch (fieldType)
        {
            case ElementFieldType.Content:
                break;
            case ElementFieldType.Description:
                break;
            case ElementFieldType.Type:
                break;
            case ElementFieldType.Perspectives:
                break;
            case ElementFieldType.Start:
                break;
            case ElementFieldType.End:
                break;
            case ElementFieldType.Age:
                break;
            case ElementFieldType.Icon:
                break;
            case ElementFieldType.Color:
                break;
            case ElementFieldType.CustomFields:
                break;
        }
    }

    public void FieldToElement()
    {
        switch (fieldType)
        {
            case ElementFieldType.Content:
                break;
            case ElementFieldType.Description:
                break;
            case ElementFieldType.Type:
                break;
            case ElementFieldType.Perspectives:
                break;
            case ElementFieldType.Start:
                break;
            case ElementFieldType.End:
                break;
            case ElementFieldType.Age:
                break;
            case ElementFieldType.Icon:
                break;
            case ElementFieldType.Color:
                break;
            case ElementFieldType.CustomFields:
                break;
        }
    }
}
public enum ElementFieldType 
{
    None,
    Content,
    Description,
    Type,
    Perspectives, // need special editor for this
    Start,
    End,
    Age,
    Icon, // need special editor for this
    Color, // need special editor for this
    CustomFields // need special editor for this
}