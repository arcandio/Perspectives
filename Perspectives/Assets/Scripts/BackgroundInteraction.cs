using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BackgroundInteraction : MonoBehaviour, IDragHandler, IPointerClickHandler {
    public GameObject nodePrototype;
    public Canvas canvasElements;

    public void OnDrag (PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.clickCount == 2)
        {
            GameObject clone = (GameObject)Instantiate(nodePrototype);
            NodeInteraction nodeInteraction = clone.GetComponent<NodeInteraction>();
            clone.transform.SetParent(canvasElements.transform, false);
            nodeInteraction.Setup();
            nodeInteraction.MoveTo(eventData.position);
        }
        else
        {
            ElementPaneUI.elementUI.ClearSelection();
        }
    }
    
}
