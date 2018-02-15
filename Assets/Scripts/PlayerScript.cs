using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {	
	public int GetMove() {
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
