using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {	

	void Update() {
		// put MakeMove here uhh soon
	}

	public int GetMove() {
		// ****TO DO: get the game to wait for a specific key to be pressed, THEN return the int
		if(Input.GetButtonDown("Rock1")) {
			return 0;
		}
		if(Input.GetButtonDown("Paper1")) {
			return 1;
		}
		if(Input.GetButtonDown("Scissors1")) {
			return 2;
		}
		return 3;
	}
}
