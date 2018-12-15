using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CardZoom : MonoBehaviour {

	public static CardZoom instance = null;
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

	public void Show(Card c)
	{
		visibleObjects.SetActive(true);
		image.sprite = c.image;
		title.text = c.cardname;
		cardText.text = c.GetCardText();

		if(c.tooltip != "" || c.tooltipTitle != "")
		{
			tooltipBox.SetActive(true);
			tooltipBox.transform.GetChild(1).GetComponentInChildren<Text>().text = c.tooltipTitle;
			tooltipBox.transform.GetChild(2).GetComponentInChildren<Text>().text = c.tooltip;
		}
		else
		{	
			tooltipBox.SetActive(false);
		}

		try{
			int cost = ((Spell)c).energyCost;
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
		
		typeBanner.color = c.GetTypeColor();
		typeBanner.GetComponentInChildren<Text>().text = c.GetCardType();

	}
}
