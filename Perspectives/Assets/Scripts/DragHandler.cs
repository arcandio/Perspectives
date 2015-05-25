using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
	GameObject itemBeingDragged;
	Vector3 startPosition;
	Transform startParent;
	RectTransform rectTransform;

	public void OnBeginDrag (PointerEventData eventData)
	{
		itemBeingDragged = gameObject;
		startPosition = transform.position;
		startParent = transform.parent;
		GetComponent<CanvasGroup>().blocksRaycasts = false;
		rectTransform = GetComponent<RectTransform>();
	}

	public void OnDrag (PointerEventData eventData)
	{
		// this works on Screen Space, but needs a new line renderer implementation
		transform.position = eventData.position;

		// this doesn't get the right position, but at least shows the line renderer.
		//Perhaps use the second answer here: http://answers.unity3d.com/questions/799616/unity-46-beta-19-how-to-convert-from-world-space-t.html
		Vector2 pos = Camera.main.WorldToViewportPoint(eventData.position);
		rectTransform.anchoredPosition = pos;
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		itemBeingDragged = null;
		if(transform.parent == startParent)
		{
			//transform.position = startPosition;
		}
		GetComponent<CanvasGroup>().blocksRaycasts = true;
	}

	public void OnDrop (PointerEventData eventData)
	{
		// We received a drop
		Debug.Log("Dropped");
	}
}