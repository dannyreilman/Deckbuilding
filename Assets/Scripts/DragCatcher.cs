using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Something that can be dragged to 
public class DragCatcher: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static DragCatcher hovering = null;
    public delegate void AcceptDrag(DragBehaviour db);
    public AcceptDrag onDragTo;
    public void OnPointerEnter(PointerEventData ped)
    {
        hovering = this;
    }

    public void OnPointerExit(PointerEventData ped)
    {
        if(hovering == this)
        {
            hovering = null;
        }
    }

}