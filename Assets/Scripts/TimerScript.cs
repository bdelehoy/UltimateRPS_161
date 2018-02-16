using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : MonoBehaviour {
	public float time = 0f;

	private IEnumerator StartCountdown() {
		while(time > 0) {
			time -= 0.1f;
			yield return new WaitForSecondsRealtime(0.1f);
		}
        gameObject.SetActive(false);
	}

	void OnEnable () {
		time = 5f;
		StartCoroutine(StartCountdown());
	}

	void OnDisable() {
		time = 0f;
		StopCoroutine("StartCountdown");
	}
	
}
