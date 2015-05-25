using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Edge : Element {
	public EdgeType edgeType = EdgeType.None;
	public Element head; // Where the edge is going
	public Element tail; // where the edge is coming from
	public Directionality directionality = Directionality.Directional;
    public float width = 1f;
    public Image lineImage;
    public RectTransform lineTransform;

	void Update() {
		UpdatePosition ();
	}
	void UpdatePosition () {
		if (head != null && tail != null) {
            rectTransform.position = (head.rectTransform.position + tail.rectTransform.position) / 2;

            // do it with an Image instead
            // set line length
            Vector2 lineSize = new Vector2((head.transform.position-tail.transform.position).magnitude, width);
            lineTransform.sizeDelta = lineSize;
            Vector3 euler = new Vector3(0, 0, CalcualateAngle(tail.transform.position, head.transform.position));
            lineTransform.rotation = Quaternion.Euler(euler);
		}
	}

    float CalcualateAngle(Vector3 point1, Vector3 point2)
    {
        float xDiff = point2.x - point1.x;
        float yDiff = point2.y - point1.y;
        return Mathf.Atan2(yDiff, xDiff) * (180 / Mathf.PI);
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