using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : Element {
	public NodeType nodeType = NodeType.None;
	public Dictionary<string, Vector2> perspectivePositions;

	// do drag and drop
    //http://answers.unity3d.com/questions/847941/unity-46-drag-and-drop.html
}
public enum NodeType {
	None,
	Person,
	Place,
	Thing,
	Event,
	Group,
	Note,
	User
}