using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickBehaviour : MonoBehaviour, IPointerClickHandler
{
	public delegate void ClickEvent();
	public ClickEvent onClick = null;
	public ClickEvent onRightClick = null;

	public bool clickable = true;

	public void OnPointerClick(PointerEventData ped)
	{
		if(ped.button == PointerEventData.InputButton.Left)
		{
			if (onClick != null && clickable)
			{
				onClick();
			}
		}
		else if(ped.button == PointerEventData.InputButton.Right)
		{
			if (onRightClick != null && clickable)
			{
				onRightClick();
			}
		}
	}
}
