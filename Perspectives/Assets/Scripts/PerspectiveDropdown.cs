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
        //toggles.Add(prototype);
        CheckMinimumToggles(perspectives.Count);
        for (int i = 0; i < toggles.Count; i++)
        {
            Toggle t = toggles[i];
            if (i < perspectives.Count)
            {
                t.gameObject.SetActive(true);
                t.transform.GetChild(1).GetComponent<Text>().text = perspectives[i];
                t.name = perspectives[i];
                t.isOn = false;
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
        string oldPerspective = perspective;
        perspective = t.name;
        dropdownText.text = perspective;
        index = GetIndex(t);
        RepositionElements(perspective, oldPerspective);
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
        string title = t.name;
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

    void RepositionElements(string newPerspective, string oldPerspective)
    {
        Debug.Log("new: "+newPerspective + " old: "+ oldPerspective);
        //string newPerspective = FileData.currentFile.currentPerspective;
        bool showAll = false;
        if (newPerspective == "Default")
        {
            showAll = true;
        }
        foreach (Element e in FileData.currentFile.allElements)
        {
            Perspective np = e.GetPerspective(newPerspective);
            Perspective op = e.GetPerspective(oldPerspective);
            
            // show what's in the perspective by position
            if (np != null && np.isDisplayed)
            {
                /*if (op == null)
                {
                    op = new Perspective();
                    op.perspective = oldPerspective;
                    op.position = e.transform.position;
                    op.isDisplayed = true;
                    e.perspectives.Add(op);
                }*/
                e.gameObject.SetActive(true);
                if (op != null)
                {
                    op.position = e.transform.position;
                }
                e.transform.position = np.position;
            }
            // Show everything, even if it doesn't have a position
            else if (showAll)
            {
                
                np = new Perspective();
                np.perspective = "Default";
                np.position = e.transform.position;
                np.isDisplayed = true;
                e.perspectives.Add(np);
                
                e.gameObject.SetActive(true);
            }
            // Not included, and not showing all
            else
            {
                e.gameObject.SetActive(false);
            }
        }
        FileData.currentFile.SetDirty();
    }
}
