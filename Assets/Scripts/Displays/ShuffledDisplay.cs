using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Displays the top card of a faceup pile
public class ShuffledDisplay : MonoBehaviour, ZoneDisplay {

	Text number;
	public ShuffledPile toDisplay = null;
	public void Display(Zone z)
	{
		toDisplay = (ShuffledPile)z;
	}

	// Use this for initialization
	void Awake () {
		number = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if(toDisplay != null)
		{
			number.text = (toDisplay.cards.Count).ToString();
		}
	}
}
