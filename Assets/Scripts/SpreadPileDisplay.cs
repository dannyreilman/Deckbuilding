using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Displays the top card of a faceup pile
public class SpreadPileDisplay : MonoBehaviour, ZoneDisplay {

	Image display;
	Text number;

	public Pile toDisplay = null;
	public void Display(Zone z)
	{
		toDisplay = (Pile)z;
	}
	public GameObject spreadPrefab;

	// Use this for initialization
	void Awake () {
		display = GetComponent<Image>();
		number = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if(toDisplay != null)
		{
			number.text = (toDisplay.cards.Count).ToString();
			while(transform.childCount > toDisplay.cards.Count)
			{
				DestroyImmediate(transform.GetChild(transform.childCount - 1));
			}

			while(transform.childCount < toDisplay.cards.Count)
			{
				GameObject.Instantiate(spreadPrefab, Vector3.zero, Quaternion.identity, transform);
			}


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
