using UnityEngine;
using System.Collections;

public class Edge : Element {
	public EdgeType edgeType = EdgeType.None;
	public Element head; // Where the edge is going
	public Element tail; // where the edge is coming from
	public Directionality directionality = Directionality.Directional;
	public LineRenderer lineRenderer;

	void Update() {
		UpdatePosition ();
	}
	void UpdatePosition () {
		if (head != null && tail != null) {
			rectTransform.position = (head.rectTransform.position+ tail.rectTransform.position) / 2;
			lineRenderer.SetPosition (0, tail.transform.position);
			lineRenderer.SetPosition (1, head.transform.position);
		}
	}
}
public enum EdgeType {
	None,
	Action,
	Relationship,
	Path,
	Dependency,
	User
}
public enum Directionality {
	Nondirectional,
	Directional,
	Bidirectional
}