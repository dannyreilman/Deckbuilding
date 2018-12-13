using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceMonitor : MonoBehaviour {
	Text energy;
	Text coin;
	Text sword;
	Text hammer;
	Text science;

	void Awake()
	{
		energy = GetComponentsInChildren<Text>()[0];
		coin = GetComponentsInChildren<Text>()[1];
		sword = GetComponentsInChildren<Text>()[2];
		hammer = GetComponentsInChildren<Text>()[3];
		science = GetComponentsInChildren<Text>()[4];
	}
	
	// Update is called once per frame
	void Update () {
		GameplayManager gm = GameplayManager.instance;
		energy.text = gm.energy.ToString();
		coin.text = gm.coin.ToString();
		sword.text = gm.attack.ToString();
		hammer.text = gm.hammers.ToString();
		science.text = gm.science.ToString();

	}
}
