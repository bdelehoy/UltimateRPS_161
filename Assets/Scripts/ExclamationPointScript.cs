using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclamationPointScript : MonoBehaviour {
	public AudioClip AttentionSound;
	private AudioSource SoundPlayer;
	
	private void Awake() {
		SoundPlayer = GetComponent<AudioSource>();
	}

	private void OnEnable() {
		SoundPlayer.PlayOneShot(AttentionSound);
	}
}
