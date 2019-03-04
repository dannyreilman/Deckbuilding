using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputModeDisplay : MonoBehaviour {

	public static InputModeDisplay instance = null;
	Text attachedText;
	Text childText;
	GameObject autoPlay;
	GameObject nextTurn;
	GameObject finishSelecting;
	public GameObject skipConfirmation;
	
	void Awake()
	{
		if(instance == null || instance.Equals(null))
		{
			instance = this;
			attachedText = GetComponent<Text>();
			childText = transform.GetChild(0).GetComponentInChildren<Text>();
			nextTurn = transform.GetChild(0).gameObject;
			autoPlay = transform.GetChild(1).gameObject;
			finishSelecting = transform.GetChild(2).gameObject;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		finishSelecting.SetActive(false);
		switch(InputManager.instance.currentMode)
		{
			case InputManager.InputMode.Dragging:
				attachedText.text = "Drag cards";
				nextTurn.SetActive(false);
				autoPlay.SetActive(false);
				break;
			case InputManager.InputMode.Playing:
				nextTurn.SetActive(true);
				attachedText.text = "Play cards";
				childText.text = "Next Turn";
				autoPlay.SetActive(true);
				break;
			case InputManager.InputMode.Selecting:
				attachedText.text = "Select " + (InputManager.instance.currentUpTo?"up to ":"") + InputManager.instance.currentStepsLeft + " cards.";
				nextTurn.SetActive(false);
				finishSelecting.SetActive(true);
				autoPlay.SetActive(false);
				break;
		}

		if(!InputManager.instance.inputValid)
		{
			nextTurn.SetActive(false);
			finishSelecting.SetActive(false);
			autoPlay.SetActive(false);
		}
	}

	public void Advance()
	{
		GameplayManager gm = GameplayManager.instance;
		if((gm.coinsShop.AnyBuyable()) ||
		   (gm.attackShop.AnyBuyable()) ||
		   (gm.hammersShop.AnyBuyable()) ||
		   (gm.scienceShop.AnyBuyable()))
		{
			skipConfirmation.SetActive(true);
		}
		else
		{
			ForceAdvance();
		}
	}

	public void ForceAdvance()
	{
		if(InputManager.instance.inputValid)
		{
			GameplayManager.instance.EndTurn();
			skipConfirmation.SetActive(false);
		}
	}
	public void CloseConfirmation()
	{
		skipConfirmation.SetActive(false);
	}

	public void FinishSelecting()
	{
		if(InputManager.instance.inputValid)
		{
			InputManager.instance.FinishSelecting();
		}
	}

	public void Autoplay()
	{
		if(InputManager.instance.inputValid)
		{
			GameplayManager.instance.AutoplayResources();
		}
	}
}
