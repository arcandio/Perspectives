using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Element : MonoBehaviour {
	public ElementType elementType = ElementType.None;
    public string elementSubType = "no type";
	public Node node;
	public Edge edge;
	//string guid;
	public TimelineDate startDate;
	public TimelineDate endDate;
	public string description;
	public Dictionary<string, string> customFields;
	//public Texture2D icon;
	public List<Element> children;
	public List<Perspective> perspectives;
	public List<Edge> edgesOut;
	public List<Edge> edgesIn;
	public RectTransform rectTransform;
    public Text buttonText;
    public ElementInteraction interaction;
    [SerializeField]
    Color color = Color.gray;
    public FileData fileData;

	void Start() {
		Guid = System.Guid.NewGuid().ToString ();
        //Content = gameObject.name;
        Color = color;
	}

    public string Guid
    {
        get
        {
            return name;
        }
        set
        {
            gameObject.name = value;
            //guid = value;
        }
    }
    public string Content
    {
        get
        {
            return buttonText.text;
        }
        set
        {
            //gameObject.name = value;
            buttonText.text = value;
        }
    }

    public Color Color
    {
        get
        {
            return color;
        }
        set
        {
            color = value;
            interaction.background.color = color;
            if (color.Luminance() > 0.5f)
            {
                buttonText.color = Color.black;
                buttonText.GetComponent<Shadow>().effectColor = Color.white;
            }
            else
            {
                buttonText.color = Color.white;
                buttonText.GetComponent<Shadow>().effectColor = Color.black;
            }
        }
    }
    public Perspective GetPerspective(string perspective)
    {
        Perspective temp = null;
        foreach (Perspective p in perspectives)
        {
            if (p.perspective == perspective)
            {
                temp = p;
            }
        }
        return temp;
    }
}
public enum ElementType {
	None,
	Node,
	Edge
}