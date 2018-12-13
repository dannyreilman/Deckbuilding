using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickBehaviour : MonoBehaviour, IPointerClickHandler
{
	public delegate void ClickEvent();
	public ClickEvent onClick = null;

	public bool clickable = true;

	public void OnPointerClick(PointerEventData ped)
	{
		if (onClick != null && clickable)
		{
			onClick();
		}
	}
}
