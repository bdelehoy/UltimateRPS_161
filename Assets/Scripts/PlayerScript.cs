using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {	
	public void ShowMoveGraphic(int move) {
		transform.GetChild(move).gameObject.SetActive(true);
	}

	public void HideMoveGraphic() {
		transform.GetChild(0).gameObject.SetActive(false);
		transform.GetChild(1).gameObject.SetActive(false);
		transform.GetChild(2).gameObject.SetActive(false);
		transform.GetChild(3).gameObject.SetActive(false);
	}
}
