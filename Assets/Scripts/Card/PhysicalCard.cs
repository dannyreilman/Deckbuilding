using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhysicalCard : MonoBehaviour
{
	static float shrinkFactor = 0.9f;
	static float growFactor = 1.1f;
	public static List<PhysicalCard> interactableCards = new List<PhysicalCard>();
	public Card card;
	public DragBehaviour attachedDrag;
	public ClickBehaviour attachedClick;
	Image image;
	Image highlight;
	Text text;
	GameObject costBanner;
	Image typeBanner;
	GameObject grayout;
	bool hovering = false;
	Vector2 size = new Vector2(100, 150);

	void Awake()
	{
		attachedDrag = GetComponent<DragBehaviour>();
		attachedClick = GetComponent<ClickBehaviour>();
		image = transform.GetChild(1).GetComponent<Image>();
		highlight = GetComponent<Image>();
		text = transform.GetChild(2).GetComponentInChildren<Text>();
		costBanner = image.transform.GetChild(0).gameObject;
		typeBanner = transform.GetChild(3).GetComponent<Image>();
		grayout = transform.GetChild(4).gameObject;
	}
	// Use this for initialization
	void Start ()	
	{
		card.p_card = this;
		interactableCards.Add(this);
		InputManager.instance.UpdatePCard(this);
		attachedClick.onRightClick = () => ZoomDisplay.instance.Show(card);
		UpdateSize();
	}

	bool shrink()
	{
		return (card.GetZone().GetName() == "hand") && !card.CanPlay() && InputManager.instance.currentMode == InputManager.InputMode.Playing;
	}

	bool grow()
	{
		return card.CanPlay() && hovering;
	}

	void UpdateSize()
	{
		if(shrink())
		{
			grayout.SetActive(true);
			((RectTransform)transform).sizeDelta = size * shrinkFactor;
		}
		else if(grow())
		{
			grayout.SetActive(false);
			((RectTransform)transform).sizeDelta = size * growFactor;
		}
		else
		{
			grayout.SetActive(false);
			((RectTransform)transform).sizeDelta = size;
		}
	}

	void Update()
	{
		UpdateSize();
	}

	public void OnMouseOver()
	{
		hovering = true;
	}

	public void OnMouseExit()
	{
		hovering = false;
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
		typeBanner.GetComponentInChildren<Text>().text = c.GetTypename();
	}

}
