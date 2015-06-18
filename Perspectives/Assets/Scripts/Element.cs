﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Element : MonoBehaviour {
	public ElementType elementType = ElementType.None;
    public string elementSubType = "no type";
	public Node node;
	public Edge edge;
	public string guid;
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

	void Start() {
		guid = Guid.NewGuid().ToString ();
        Content = gameObject.name;
        Color = color;
	}

	public JSONObject PackJson (){
		JSONObject json = new JSONObject (JSONObject.Type.OBJECT);

		return json;
	}

	public static Element UnpackJson(JSONObject json) {
		Element instance = new Element();

		return instance;
	}


    public string Content
    {
        get
        {
            return name;
        }
        set
        {
            gameObject.name = value;
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
}
public enum ElementType {
	None,
	Node,
	Edge
}