using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Element : MonoBehaviour {
	public ElementType elementType = ElementType.None;
	public Node node;
	public Edge edge;
    public NodeInteraction nodeInteraction;
    public EdgeInteraction edgeInteraction;
	public string guid;
	public TimelineDate startDate;
	public TimelineDate endDate;
	public string description;
	public Dictionary<string, string> customFields;
	public Texture2D icon;
	public List<Element> children;
	public List<Perspective> perspectives;
	public List<Edge> edgesOut;
	public List<Edge> edgesIn;
	public RectTransform rectTransform;
    public Text buttonText;

	void Start() {
		guid = Guid.NewGuid().ToString ();
        Content = gameObject.name;
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
}
public enum ElementType {
	None,
	Node,
	Edge
}