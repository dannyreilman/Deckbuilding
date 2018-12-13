using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputModeDisplay : MonoBehaviour {

	Text attachedText;
	
	void Awake()
	{
		attachedText = GetComponent<Text>();
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
						break;
					case GameplayManager.Phase.Resources:
						attachedText.text = "Play resources";
						break;
					case GameplayManager.Phase.Actions:
						attachedText.text = "Do actions";
						break;
				}
				break;
			case InputManager.InputMode.Selecting:
				attachedText.text = "Select " + InputManager.instance.stepsLeft + " cards.";
				break;
		}
	}
}
