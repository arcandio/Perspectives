using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : Element {
	//public NodeType nodeType = NodeType.None;
	public Dictionary<string, Vector2> perspectivePositions;

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