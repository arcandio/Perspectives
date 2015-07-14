using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Edge : Element {
	//public EdgeType edgeType = EdgeType.None;
	public Element head; // Where the edge is going
	public Element tail; // where the edge is coming from
    public string headGuid;
    public string tailGuid;
    public Vector3 headPos;
    public Vector3 tailPos;
    public Vector3 dragPos;
    public EdgeDragMode dragMode = EdgeDragMode.None;
	public Directionality directionality = Directionality.Directional;
    public float width = 1f;
    public Image lineImage;
    public RectTransform lineTransform;

	void Update() {
		UpdatePosition ();
	}
	void UpdatePosition () {
        
        // We always need a Head to start with, and we need either a tail or a dragpos
		if (head != null 
            && (tail != null || dragMode != EdgeDragMode.None))
        {
            
            // skip if no changes
            if (head != null 
                && tail != null 
                && head.rectTransform.position == headPos 
                && tail.rectTransform.position == tailPos
                && dragMode == EdgeDragMode.None)
            {
                // Do Nothing
            }

            // either the positions were changed, or we're on dragmode.
            else
            {
                // get head and tail pos
                headPos = head != null ? head.rectTransform.position : Vector3.zero;
                tailPos = tail != null ? tail.rectTransform.position : Vector3.zero;
                
                // check for dragging an end
                headPos = dragMode == EdgeDragMode.Head ? dragPos : headPos;       
                tailPos = dragMode == EdgeDragMode.Tail ? dragPos : tailPos;
                
                // set label position
                rectTransform.position = (headPos + tailPos) / 2;

                // do it with an Image instead
                // set line length
                Vector2 lineSize = new Vector2((headPos - tailPos).magnitude, width);
                lineTransform.sizeDelta = lineSize;
                Vector3 euler = new Vector3(0, 0, CalcualateAngle(tailPos, headPos));
                lineTransform.rotation = Quaternion.Euler(euler);
            }
		}        
	}

    float CalcualateAngle(Vector3 point1, Vector3 point2)
    {
        float xDiff = point2.x - point1.x;
        float yDiff = point2.y - point1.y;
        return Mathf.Atan2(yDiff, xDiff) * (180 / Mathf.PI);
    }
    public void EndsFromGuids()
    {
        // Find appropriate Elements
        head = fileData.GetElementByName(headGuid);
        tail = fileData.GetElementByName(tailGuid);
        // set the position
        UpdatePosition();
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
public enum EdgeDragMode
{
    None,
    Head,
    Tail
}