using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePanel : MonoBehaviour
{
	public GameObject choicePrefab;
	Text blurb;
	Image face;
	Transform optionsParent;

	// Use this for initialization
	void Awake ()
	{
		blurb = GetComponentInChildren<Text>();
		face = transform.GetChild(0).GetComponent<Image>();
		optionsParent = transform.GetChild(2);
	}

	public void Initialize(Visitor v)
	{
		blurb.text = v.blurb;
		face.sprite = v.sprite;
		foreach(Buyable b in v.options)
		{
			if(b is Card)
				(b as Card).cloneBuy = true;
			GameObject instantiated = Instantiate(choicePrefab, optionsParent);
			instantiated.GetComponent<BuyableDisplayer>().Initialize(b);
		}
	}

	public void Destroy()
	{
		Destroy(gameObject);
	}

	public void Hide()
	{
		transform.localScale = Vector3.zero;
	}
}
