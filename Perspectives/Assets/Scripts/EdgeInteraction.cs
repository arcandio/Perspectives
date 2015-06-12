using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EdgeInteraction : MonoBehaviour {
    Edge edge;
    Canvas canvas;
    RectTransform canvasTransform;
    Camera cam;
    bool selected = false;
    public Image background;

    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        edge = GetComponent<Edge>();
        canvas = GetComponentInParent<Canvas>();
        canvasTransform = canvas.GetComponent<RectTransform>();
        cam = Camera.main;
        ElementPaneUI.elementUI.AddElement(edge);
    }

    public void ClickedElement()
    {
        ElementPaneUI.elementUI.SelectElement(edge);
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
        }
    }
}
