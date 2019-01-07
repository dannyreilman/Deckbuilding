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
				break;
			case InputManager.InputMode.Playing:
				nextTurn.SetActive(true);
				switch(GameplayManager.instance.currentPhase)
				{
					case GameplayManager.Phase.Spells:
						attachedText.text = "Play spells";
						childText.text = "Advance To Resource Phase";
						autoPlay.SetActive(false);
						break;
					case GameplayManager.Phase.Resources:
						attachedText.text = "Play resources";
						childText.text = "Advance To Spending Phase";
						autoPlay.SetActive(true);
						break;
					case GameplayManager.Phase.Spending:
						attachedText.text = "Spend resources";
						childText.text = "Next Turn";
						autoPlay.SetActive(false);
						break;
				}
				break;
			case InputManager.InputMode.Selecting:
				attachedText.text = "Select " + (InputManager.instance.upTo?"up to ":"") + InputManager.instance.stepsLeft + " cards.";
				nextTurn.SetActive(false);
				finishSelecting.SetActive(true);
				break;
			case InputManager.InputMode.Animating:
				nextTurn.SetActive(false);
				autoPlay.SetActive(false);
			break;
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
		GameplayManager.instance.AdvancePhase();
		skipConfirmation.SetActive(false);
	}
	public void CloseConfirmation()
	{
		skipConfirmation.SetActive(false);
	}

	public void FinishSelecting()
	{
		InputManager.instance.FinishSelecting();
	}

	public void Autoplay()
	{
		GameplayManager.instance.AutoplayResources();
	}
}
