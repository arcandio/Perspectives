using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomFieldList : MonoBehaviour {
    public ElementPaneUI elementUi;
    public ElementFieldEditor efe;
    public List<CustomFieldEntry> fieldEntries;

    public void SetupList()
    {
        CheckMinimumFields();
        for (int i = 0; i < fieldEntries.Count; i++ )
        {
            CustomFieldEntry cfe = fieldEntries[i];
            if (i < FileData.currentFile.customFields.Count)
            {
                cfe.gameObject.SetActive(true);
                cfe.key.text = FileData.currentFile.customFields[i];
            }
            else
            {
                cfe.gameObject.SetActive(false);
            }
        }
    }

    public void Populate (Dictionary<string, string> entries)
    {
        foreach (CustomFieldEntry cfe in fieldEntries)
        {
            if (entries != null && entries.ContainsKey(cfe.key.text) && !string.IsNullOrEmpty(entries[cfe.key.text]))
            {
                cfe.value.text = entries[cfe.key.text];
            }
            else
            {
                cfe.value.text = "";
            }
        }
    }

    public void Entered(string key, string value)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        dict.Add(key, value);
        efe.RecieveCustomFields(dict);
    }

    void CheckMinimumFields()
    {
        if (fieldEntries.Count < FileData.currentFile.customFields.Count)
        {
            GameObject clone = Instantiate(fieldEntries[0].gameObject);
            clone.transform.SetParent(fieldEntries[0].transform.parent, false);
            fieldEntries.Add(clone.GetComponent<CustomFieldEntry>());
            CheckMinimumFields();
        }
    }
}
