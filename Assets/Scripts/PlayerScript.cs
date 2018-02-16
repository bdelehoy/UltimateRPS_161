using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {	
	public int move = 0;

	//public IEnumerator GetMove() {
	public int GetMove() {
		if(Input.GetButtonDown("Rock1")) {
			return 1;
		}
		if(Input.GetButtonDown("Paper1")) {
			return 2;
		}
		if(Input.GetButtonDown("Scissors1")) {
			return 3;
		}
		return 0;	// returns by default: problem.
	}
}
