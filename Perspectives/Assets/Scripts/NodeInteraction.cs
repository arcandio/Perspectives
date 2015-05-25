using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class NodeInteraction : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    Node node;
    GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;
    RectTransform rectTransform;
    Canvas canvas;
    RectTransform canvasTransform;
    Camera cam;

    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        node = GetComponent<Node>();
        canvas = GetComponentInParent<Canvas>();
        canvasTransform = canvas.GetComponent<RectTransform>();
        cam = Camera.main;
        ElementPaneUI.elementUI.AddElement(node);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveTo(eventData.position);
    }

    public void MoveTo(Vector2 position)
    {
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            // For rendering in Screen Space - Overlay
            transform.position = position;
        }

        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            // For rendering in Screen Space - Camera canvas render mode
            Vector3 pos;
            pos = cam.ScreenToViewportPoint(position);
            pos.x = (pos.x * canvasTransform.sizeDelta.x) - (canvasTransform.sizeDelta.x * 0.5f);
            pos.y = (pos.y * canvasTransform.sizeDelta.y) - (canvasTransform.sizeDelta.y * 0.5f);
            pos.z = 0;
            rectTransform.anchoredPosition = pos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;
        if (transform.parent == startParent)
        {
            //transform.position = startPosition;
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    public void SelectNode()
    {
        ElementPaneUI.elementUI.SelectElement(node);
    }

    public void OnDrop(PointerEventData eventData)
    {
        // We received a drop
        Debug.Log("Dropped: " + eventData.selectedObject.name);
    }
}