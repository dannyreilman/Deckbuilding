using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoCompress : MonoBehaviour {
	HorizontalLayoutGroup hgroup;
	Canvas canvas;
	public int elementWidth;

	void Awake()
	{
		hgroup = GetComponent<HorizontalLayoutGroup>();
		canvas = GetComponentInParent<Canvas>();
	}

	// Update is called once per frame
	void Update () {
		//This ensures widthNoSpacing + the negative spacing is never greater than total width,
		//Without affecting appearance of a small number of elements
		float widthNoSpacing = elementWidth * transform.childCount;
		float rectWidth = ((RectTransform)transform).rect.width * canvas.scaleFactor;
		if(rectWidth < widthNoSpacing)
		{
			hgroup.spacing = (rectWidth - widthNoSpacing) / (transform.childCount-1);
		}
		else
		{
			hgroup.spacing = 0;
		}
		
	}
}
