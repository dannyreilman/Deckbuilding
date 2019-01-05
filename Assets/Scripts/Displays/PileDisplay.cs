using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Displays the top card of a faceup pile
public class PileDisplay : MonoBehaviour, ZoneDisplay {

	Image display;
	public Text number;
	public Pile toDisplay = null;
	public void Display(Zone z)
	{
		toDisplay = (Pile)z;
	}

	// Use this for initialization
	void Awake () {
		display = transform.GetChild(1).GetComponent<Image>();
		number = transform.GetChild(2).GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if(toDisplay != null)
		{
			number.text = (toDisplay.cards.Count).ToString();
			if(toDisplay.cards.Count > 0)
			{
				display.sprite = toDisplay.cards[toDisplay.cards.Count - 1].image;
			}
			else
			{
				display.sprite = null;
			}
		}
	}
}
