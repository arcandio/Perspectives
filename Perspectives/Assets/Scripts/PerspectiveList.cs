using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PerspectiveList : MonoBehaviour {
    //ElementPaneUI elementUi;
    public ElementFieldEditor efe;
    public List<string> items;
    public List<Toggle> toggles;
    public RectTransform rectTransform;
    public Element currentElement;

    public void ResetList()
    {
        //elementUi = ElementPaneUI.elementUI;
        currentElement = FileData.currentFile.selectedElements[0];
        // get the new items
        items = new List<string>();
        items.AddRange(FileData.perspectivesDefault);
        items.AddRange(FileData.currentFile.perspectivesCustom);

        // check there are enough toggles
        CheckMinimumToggles();

        // setup the toggles and turn off the unused ones
        for (int i = 0; i < toggles.Count; i++)
        {
            if (i < items.Count)
            {
                toggles[i].gameObject.SetActive(true);
                toggles[i].gameObject.name = items[i];
                toggles[i].transform.GetChild(1).GetComponent<Text>().text = items[i];
                
                // check if this is already active
                toggles[i].isOn = false;
                foreach (Perspective p in currentElement.perspectives.ToArray())
                {
                    if (p.perspective == items[i] && p.isDisplayed)
                    {
                        toggles[i].isOn = true;
                    }
                }
            }
            else
            {
                toggles[i].gameObject.SetActive(false);
            }
        }
    }
    void CheckMinimumToggles()
    {
        if (toggles.Count < items.Count)
        {
            GameObject clone = Instantiate(toggles[0].gameObject);
            clone.transform.SetParent(toggles[0].transform.parent, false);
            toggles.Add(clone.GetComponent<Toggle>());
            CheckMinimumToggles();
        }
    }
    public void Clicked(Toggle t)
    {
        // rebuild the list of perspectives
        List<Perspective> newlist = FileData.currentFile.selectedElements[0].perspectives;
        for (int i = 0; i < toggles.Count; i++ )
        {
            Toggle currentToggle = toggles[i];
            bool foundExistingPerspective = false;
            for (int p = 0; p < newlist.Count; p++ )
            {
                Perspective currentPerspective = newlist[p];
                if (currentToggle.name == currentPerspective.perspective)
                {
                    currentPerspective.isDisplayed = currentToggle.isOn;
                    foundExistingPerspective = true;
                    newlist[p] = currentPerspective;
                }
            }
            if (!foundExistingPerspective)
            {
                Perspective newPerspective = new Perspective();
                newPerspective.perspective = currentToggle.name;
                newPerspective.isDisplayed = currentToggle.isOn;
                newlist.Add(newPerspective);
            }
        }
        // Todo not collapse positions, bug #25
        efe.RecievePerspective(newlist);
    }
}
