using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Element : MonoBehaviour {
	public ElementType elementType = ElementType.None;
	public Node node;
	public Edge edge;
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

	public static GameObject nodePrototype;
	public static GameObject edgePrototype;

	static Element () {
		nodePrototype = (GameObject)Resources.Load ("Node", typeof(GameObject));
		edgePrototype = (GameObject)Resources.Load ("Edge", typeof(GameObject));
	}

	void Start() {
		guid = Guid.NewGuid().ToString ();
	}

	public JSONObject PackJson (){
		JSONObject json = new JSONObject (JSONObject.Type.OBJECT);

		return json;
	}

	public static Element UnpackJson(JSONObject json) {
		Element instance = new Element();

		return instance;
	}
}
public enum ElementType {
	None,
	Node,
	Edge
}