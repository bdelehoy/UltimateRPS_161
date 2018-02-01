using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour {
	private string move;
	///private string[] availableMoves = {"Rock2", "Paper2", "Scissors2", "NoMove"};
	public int GetMove() {
		return Random.Range(0,4);	// basic AI
	}
}
