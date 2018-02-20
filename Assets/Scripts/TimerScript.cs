using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : MonoBehaviour {
	public float time = 0f;
	private float timeResolution = 0.1f;

	private IEnumerator StartCountdown() {
		while(time > 0) {
			time -= timeResolution;
			yield return new WaitForSeconds(timeResolution);
		}
        gameObject.SetActive(false);
	}

	public void setTime(float newtime) {
		time = newtime;
	}

	public void OnEnable () {
		StartCoroutine(StartCountdown());
	}

	public void OnDisable() {
		time = 0f;
		//StopCoroutine("StartCountdown");	// idk if this is doing anything but it's not broken
	}
	
}
