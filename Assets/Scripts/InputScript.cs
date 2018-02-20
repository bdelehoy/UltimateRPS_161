using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour {

	public int move;
	public bool playerReady = false;
	public AudioSource feedbackManager;

	public AudioClip moveHit;
	public AudioClip moveMiss;

	void Awake() {
		feedbackManager = GetComponent<AudioSource>();
	}

	void Update () {
		if(Input.GetButtonDown("Rock1")) {
			Debug.Log("Just got ROCK");
			move = 1;
			PlaySounds();
		}
		if(Input.GetButtonDown("Paper1")) {
			Debug.Log("Just got PAPER");
			move = 2;
			PlaySounds();
		}
		if(Input.GetButtonDown("Scissors1")) {
			Debug.Log("Just got SCISSORS");
			move = 3;
			PlaySounds();
		}
	}

	private void PlaySounds() {
		if(!playerReady) {
			feedbackManager.PlayOneShot(moveMiss);
		}
		else {
			feedbackManager.PlayOneShot(moveHit);
		}
	}

	void OnEnable() {
		//Debug.Log("GO AHEAD AND GIVE INPUTS");
		move = 0;
	}

}
