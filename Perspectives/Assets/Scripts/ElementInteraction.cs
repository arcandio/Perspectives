using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ElementInteraction : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    public Element element;
    GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;
    RectTransform rectTransform;
    Canvas canvas;
    RectTransform canvasTransform;
    Camera cam;
    bool selected = false;
    public Image background;
    public UiButton uiButton;
    public Image highlight;
    static Edge tempEdge;
    public CanvasGroup lineGroup;

    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        //element = GetComponent<Element>();
        //element.node = GetComponent<Node>();
        canvas = GetComponentInParent<Canvas>();
        canvasTransform = canvas.GetComponent<RectTransform>();
        cam = Camera.main;
        ElementPaneUI.elementUI.AddElement(element);
        Selected = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (element.elementType == ElementType.Node)
            {
                Debug.Log("Drag");
                itemBeingDragged = gameObject;
                startPosition = transform.position;
                startParent = transform.parent;
                GetComponent<CanvasGroup>().blocksRaycasts = false;
                rectTransform = GetComponent<RectTransform>();
            }
            if (element.elementType == ElementType.Edge)
            {
                // Determine dragged end
                EdgeDragMode dm = EdgeDragMode.Head;
                float headDist = ((Vector3)element.edge.head.transform.position - (Vector3)eventData.position).magnitude;
                float tailDist = ((Vector3)element.edge.tail.transform.position - (Vector3)eventData.position).magnitude;
                if (tailDist < headDist)
                {
                    dm = EdgeDragMode.Tail;
                }
                element.edge.dragMode = dm;
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // create new edge
            GameObject prototype = (GameObject)Resources.Load("ElementEdge", typeof(GameObject));
            GameObject clone = Instantiate(prototype);
            tempEdge = clone.GetComponent<Edge>();
            clone.transform.SetParent(transform.parent, false);
            ElementInteraction nodeInteraction = clone.GetComponent<ElementInteraction>();
            nodeInteraction.Setup();
            //nodeInteraction.MoveTo(eventData.position);
            // set head end to this element
            tempEdge.head = element;
            tempEdge.headGuid = element.Guid;
            tempEdge.headPos = element.transform.position;
            tempEdge.interaction.lineGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (element.elementType == ElementType.Node)
            {
                MoveTo(eventData.position);
                FileData.currentFile.SetDirty();
            }
            if (element.elementType == ElementType.Edge)
            {
                // send drag information
                element.edge.dragPos = eventData.position;
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // move tail end
            tempEdge.dragMode = EdgeDragMode.Tail;
            tempEdge.dragPos = eventData.position;
        }
        else if (tempEdge != null)
        {
            // move tail end
            tempEdge.dragMode = EdgeDragMode.Tail;
            tempEdge.dragPos = eventData.position;
        }
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
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (element.elementType == ElementType.Node)
            {
                itemBeingDragged = null;
                if (transform.parent == startParent)
                {
                    //transform.position = startPosition;
                }
                GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            if (element.elementType == ElementType.Edge)
            {
                Element dropTarget = null;
                foreach (GameObject go in eventData.hovered)
                {
                    Element e = go.GetComponent<Element>();
                    if (e != null)
                    {
                        dropTarget = e;
                    }
                }
                Debug.Log("DropTarget: " + dropTarget);
                if (dropTarget != null && dropTarget != element)
                {
                    if (element.edge.dragMode == EdgeDragMode.Head)
                    {
                        element.edge.head = dropTarget;
                    }
                    else if (element.edge.dragMode == EdgeDragMode.Tail)
                    {
                        element.edge.tail = dropTarget;
                    }
                    FileData.currentFile.SetDirty();
                }
                element.edge.dragPos = Vector3.zero;
                element.edge.dragMode = EdgeDragMode.None;
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // decide whether to connect or destroy
            if (tempEdge.tail == null)
            {
                Debug.Log("Destroy");
                FileData.currentFile.allElements.Remove(tempEdge);
                Destroy(tempEdge.gameObject);
                ElementPaneUI.elementUI.GenerateElementList();
            }
            else
            {
                Debug.Log("Drag End hit");
                tempEdge.dragMode = EdgeDragMode.None;
                tempEdge.interaction.lineGroup.blocksRaycasts = true;
                tempEdge = null;
            }
        }
    }

    public void ClickedElement()
    {
        Element selected = element.node != null ? (Element)element.node : (Element)element.edge;
        ElementPaneUI.elementUI.SelectElement(selected);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped: " + eventData.selectedObject);
        if (eventData.button == PointerEventData.InputButton.Right && element != tempEdge)
        {
            tempEdge.tail = element;
            tempEdge.tailGuid = element.Guid;
            tempEdge.dragPos = element.transform.position;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 1)
        {
            Debug.Log("Select");
            ClickedElement();
        }
        if (eventData.clickCount == 2)
        {
            Debug.Log("Edit");
        }
    }


    public bool Selected
    {
        get
        {
            return selected;
        }
        set
        {
            selected = value;
            
            if (selected)
            {
                //background.color = ElementPaneUI.elementUI.colorSelected;
                highlight.gameObject.SetActive(true);
                highlight.color = ElementPaneUI.elementUI.colorSelected;
            }
            else
            {
                //background.color = ElementPaneUI.elementUI.colorUnselected;
                highlight.gameObject.SetActive(false);
            }
            
            uiButton.Selected = selected;
        }
    }

}