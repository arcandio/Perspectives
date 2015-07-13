using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PerspectiveDropdown : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IScrollHandler {
    public Toggle prototype;
    public List<Toggle> toggles;
    public List<string> perspectives;
    public ToggleGroup toggleGroup;
    public RectTransform dropdownContents;
    public Toggle dropdownButton;
    public Text dropdownText;
    public string perspective;
    public int index = -1;

    void Start()
    {
        UpdateList();
        dropdownContents.gameObject.SetActive(false);
    }

    public void UpdateList()
    {
        perspectives = new List<string>();
        perspectives.AddRange(FileData.perspectivesDefault);
        perspectives.AddRange(FileData.currentFile.perspectivesCustom);
        toggles.Add(prototype);
        CheckMinimumToggles(perspectives.Count);
        for (int i = 0; i < toggles.Count; i++)
        {
            Toggle t = toggles[i];
            if (i < perspectives.Count)
            {
                t.gameObject.SetActive(true);
                t.GetComponentInChildren<Text>().text = perspectives[i];
                if (perspectives[i] == FileData.currentFile.currentPerspective)
                {
                    t.isOn = true;
                    dropdownText.text = perspectives[i];
                    index = i;
                }
            }
            else
            {
                t.gameObject.SetActive(false);
            }
        }
    }

    public void SetActivePerspective(Toggle t)
    {
        perspective = t.GetComponentInChildren<Text>().text;
        dropdownText.text = perspective;
        index = GetIndex(t);
    }

    void CheckMinimumToggles(int min)
    {
        if (toggles.Count < min)
        {
            Toggle clone = Instantiate(prototype);
            clone.transform.SetParent(prototype.transform.parent, false);
            toggles.Add(clone);
            CheckMinimumToggles(min);
        }
    }

    int GetIndex(Toggle t)
    {
        int match = -1;
        string title = t.GetComponentInChildren<Text>().text;
        for (int i = 0; i < perspectives.Count; i++)
        {
            if (perspectives[i] == title)
            {
                match = i;
            }
        }
            return match;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        dropdownContents.gameObject.SetActive(true);
        dropdownButton.isOn = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        dropdownContents.gameObject.SetActive(false);
        dropdownButton.isOn = false;
    }
    public void OnScroll(PointerEventData eventData)
    {
        
        int newIndex = (index + (int)eventData.scrollDelta.y);
        newIndex = newIndex < 0 ? (newIndex + perspectives.Count) : newIndex;
        newIndex %= perspectives.Count;
        toggles[newIndex].isOn = true;
    }
}
