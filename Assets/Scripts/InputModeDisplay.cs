using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputModeDisplay : MonoBehaviour {

	Text attachedText;
	Text childText;
	GameObject autoPlay;
	
	void Awake()
	{
		attachedText = GetComponent<Text>();
		childText = transform.GetChild(0).GetComponentInChildren<Text>();
		autoPlay = transform.GetChild(1).gameObject;
	}

	// Update is called once per frame
	void Update () {
		switch(InputManager.instance.currentMode)
		{
			case InputManager.InputMode.Dragging:
				attachedText.text = "Drag cards";
				break;
			case InputManager.InputMode.Playing:
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
				attachedText.text = "Select " + InputManager.instance.stepsLeft + " cards.";
				break;
		}
	}

	public void Advance()
	{
		GameplayManager.instance.AdvancePhase();
	}
	public void Autoplay()
	{
		GameplayManager.instance.AutoplayResources();
	}
}
