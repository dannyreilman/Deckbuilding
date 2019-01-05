using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ZoomDisplay : MonoBehaviour {

	public static ZoomDisplay instance = null;
	GameObject visibleObjects;
	Image image;
	Text title;
	Text cardText;
	GameObject cardBanner;
	GameObject tooltipBox;
	Image typeBanner;


	// Use this for initialization
	void Awake () {
		if(instance == null || instance.Equals(null))
		{
			instance = this;
			GameObject.Find("Playmat").GetComponent<ClickBehaviour>().onRightClick = Hide;
			GameObject.Find("Playmat").GetComponent<ClickBehaviour>().onClick = Hide;
			visibleObjects = transform.GetChild(0).gameObject;
			tooltipBox = visibleObjects.transform.GetChild(1).gameObject;
			image = visibleObjects.transform.GetChild(0).GetChild(1).GetComponent<Image>();
			title = visibleObjects.transform.GetChild(0).GetChild(2).GetComponentInChildren<Text>();
			cardBanner = image.transform.GetChild(0).gameObject;
			typeBanner = visibleObjects.transform.GetChild(0).GetChild(3).GetComponent<Image>();
			cardText = visibleObjects.transform.GetChild(0).GetChild(4).GetComponentInChildren<Text>();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void Hide()
	{
		visibleObjects.SetActive(false);
	}

	public void Show(Buyable b)
	{
		visibleObjects.SetActive(true);
		image.sprite = b.GetDisplay();
		title.text = b.GetName();
		cardText.text = b.GetDescription();

		if(b.GetTooltip() != "" || b.GetTooltipTitle() != "")
		{
			tooltipBox.SetActive(true);
			tooltipBox.transform.GetChild(1).GetComponentInChildren<Text>().text = b.GetTooltipTitle();
			tooltipBox.transform.GetChild(2).GetComponentInChildren<Text>().text = b.GetTooltip();
		}
		else
		{	
			tooltipBox.SetActive(false);
		}

		try{
			int cost = ((Spell)b).energyCost;
			if(cost != 0)
			{
				cardBanner.SetActive(true);
				cardBanner.GetComponentInChildren<Text>().text = "Cost: " + cost.ToString() + " energy";
			}
			else
			{
				cardBanner.SetActive(false);
			}
		}
		catch(InvalidCastException e)
		{
			cardBanner.SetActive(false);
		}
		
		typeBanner.color = b.GetTypeColor();
		typeBanner.GetComponentInChildren<Text>().text = b.GetTypename();

	}
}
