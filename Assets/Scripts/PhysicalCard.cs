using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicalCard : MonoBehaviour
{
	public static List<PhysicalCard> interactableCards = new List<PhysicalCard>();
	public Card card;
	public DragBehaviour attachedDrag;
	public ClickBehaviour attachedClick;
	Image image;
	Image highlight;
	Text text;
	GameObject costBanner;
	Image typeBanner;

	void Awake()
	{
		attachedDrag = GetComponent<DragBehaviour>();
		attachedClick = GetComponent<ClickBehaviour>();
		image = transform.GetChild(1).GetComponent<Image>();
		highlight = GetComponent<Image>();
		text = transform.GetChild(2).GetComponentInChildren<Text>();
		costBanner = image.transform.GetChild(0).gameObject;
		typeBanner = transform.GetChild(3).GetComponent<Image>();
	}
	// Use this for initialization
	void Start ()	
	{
		card.p_card = this;
		interactableCards.Add(this);
		InputManager.instance.UpdatePCard(this);
		attachedClick.onRightClick = () => CardZoom.instance.Show(card);
	}

	public void Select()
	{
		highlight.enabled = true;
	}
	public void Deselect()
	{
		highlight.enabled = false;
	}

	public void preventInteraction()
	{
		interactableCards.Remove(this);
		attachedDrag.enabled = false;
	}

	public void MakeInteractable()
	{
		interactableCards.Add(this);
		attachedDrag.enabled = true;
	}

	public void BreakPCard()
	{
		card.p_card= null;
		Destroy(gameObject);
	}

	public void Associate(Card c)
	{
		c.p_card= this;
		card = c;
		image.sprite = c.image;
		text.text = c.cardname;
		try{
			int cost = ((Spell)c).energyCost;
			if(cost != 0)
			{
				costBanner.SetActive(true);
				costBanner.GetComponentInChildren<Text>().text = "Cost: " + cost.ToString() + " energy";
			}
			else
			{
				costBanner.SetActive(false);
			}
		}
		catch(InvalidCastException e)
		{
			costBanner.SetActive(false);
		}

		typeBanner.color = c.GetTypeColor();
		typeBanner.GetComponentInChildren<Text>().text = c.GetCardType();
	}

}
