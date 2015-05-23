using UnityEngine;
using System.Collections;
using System;

public class Element : MonoBehaviour {
	public ElementType elementType = ElementType.None;
	public Guid guid = new Guid();
	
}
public enum ElementType {
	None,
	Node,
	Edge
}