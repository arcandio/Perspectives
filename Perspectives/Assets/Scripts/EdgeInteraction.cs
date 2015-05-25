using UnityEngine;
using System.Collections;

public class EdgeInteraction : MonoBehaviour {
    Edge edge;
    Canvas canvas;
    RectTransform canvasTransform;
    Camera cam;

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

    public void SelectNode()
    {
        ElementPaneUI.elementUI.SelectElement(edge);
    }
}
