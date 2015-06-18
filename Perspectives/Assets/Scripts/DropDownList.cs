using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DropDownList : MonoBehaviour {
    public ElementFieldEditor efe;
    public List<string> items;
    public List<Button> buttons;
    public string current;
    public RectTransform rectTransform;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ResetList(RectTransform fosterParent, List<string> replacements)
    {
        transform.SetParent(fosterParent.parent, false);
        rectTransform.anchoredPosition = fosterParent.anchoredPosition;
        items = replacements;
        // check there are enough toggles
        CheckMinimumButtons();

        // setup correct toggles and turn off unused toggles
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i < items.Count)
            {
                buttons[i].gameObject.SetActive(true);
                buttons[i].gameObject.name = items[i];
                buttons[i].transform.GetChild(0).GetComponent<Text>().text = items[i];
                if (items[i] == current)
                {
                    buttons[i].interactable = false;
                }
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }
    void CheckMinimumButtons()
    {
        if (buttons.Count < items.Count)
        {
            GameObject clone = Instantiate(buttons[0].gameObject);
            clone.transform.SetParent(buttons[0].transform.parent, false);
            buttons.Add(clone.GetComponent<Button>());
            CheckMinimumButtons();
        }
    }
    public void Clicked(Button b)
    {
        efe.RecieveDropdown(b.name);
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            efe.DisplayDropdown(false, null);
        }
    }
}
