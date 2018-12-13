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
	Text text;

	void Awake()
	{
		attachedDrag = GetComponent<DragBehaviour>();
		attachedClick = GetComponent<ClickBehaviour>();
		image = transform.GetChild(0).GetComponent<Image>();
		text = transform.GetChild(2).GetComponent<Text>();
	}
	// Use this for initialization
	void Start ()	
	{
		card.p_card = this;
		interactableCards.Add(this);
		InputManager.instance.UpdatePCard(this);
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
	}

}
