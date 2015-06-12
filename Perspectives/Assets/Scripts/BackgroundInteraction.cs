using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BackgroundInteraction : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerClickHandler {
    public GameObject nodePrototype;
    public Transform layout;
    public Camera cam;
    public RectTransform moveTransform;
    public RectTransform layoutTransform;
    public Vector3 clickOffset;

    public void OnDrag (PointerEventData eventData)
    {
        MoveLayoutTo(eventData.position);
    }

    public void OnBeginDrag (PointerEventData eventData)
    {
        clickOffset = (Vector3)cam.ScreenToViewportPoint(eventData.position).MangleByTransform(moveTransform) - (Vector3)layoutTransform.anchoredPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        clickOffset = Vector3.zero;
    }

    void MoveLayoutTo(Vector2 position)
    {        
        // For rendering in Screen Space - Camera canvas render mode
        Vector3 pos = cam.ScreenToViewportPoint(position).MangleByTransform(moveTransform);

        layoutTransform.anchoredPosition = pos - clickOffset;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.clickCount == 2)
        {
            GameObject clone = (GameObject)Instantiate(nodePrototype);
            ElementInteraction nodeInteraction = clone.GetComponent<ElementInteraction>();
            clone.transform.SetParent(layout, false);
            nodeInteraction.Setup();
            nodeInteraction.MoveTo(eventData.position);
        }
        if (eventData.clickCount == 1 && !eventData.dragging)
        {
            ElementPaneUI.elementUI.ClearSelection();
        }
    }
    
}
