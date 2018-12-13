using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public static DragBehaviour dragging = null;
	public delegate void DragEvent();
	public DragEvent onRelease;

	[HideInInspector]
	public Image image;

	void Awake()
	{
		image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(dragging != null && Input.GetMouseButtonUp(0))
		{
			if(dragging.onRelease != null)
			{
				dragging.onRelease();
			}
			dragging = null;
		}

		if(dragging == this)
		{
			transform.position = Input.mousePosition;
		}
	}

	public void OnPointerDown(PointerEventData ped)
	{
		dragging = this;
		dragging.image.raycastTarget = false;
	}
	public void OnPointerUp(PointerEventData ped)
	{
		if(dragging == null || dragging.Equals(null))
		{
			Debug.Log("How did this get hit?(In DragBehaviour)");
		}
		else if(DragCatcher.hovering != null)
        {
            if(DragCatcher.hovering.onDragTo != null)
            {
            	Debug.Log("Hit");
                DragCatcher.hovering.onDragTo(dragging);
            }
			dragging.image.raycastTarget = true;
            dragging = null;
        }
		else
		{
			if(dragging.onRelease != null)
				dragging.onRelease();
			dragging.image.raycastTarget = true;
            dragging = null;
		}
	}
}
