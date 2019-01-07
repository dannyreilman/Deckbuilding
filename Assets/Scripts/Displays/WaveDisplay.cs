using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveDisplay : MonoBehaviour {

	Text attachedText;
	// Use this for initialization
	void Awake () {
		attachedText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		attachedText.text = "Turns until next wave: " + GameplayManager.instance.turnsUntilWave.ToString();
	}
}
