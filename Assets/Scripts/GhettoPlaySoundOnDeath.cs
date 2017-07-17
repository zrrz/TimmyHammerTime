using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhettoPlaySoundOnDeath : MonoBehaviour {

	AudioSource myAudioSource;
	public AudioClip sound;

	public void PlaySound() {
		myAudioSource = gameObject.AddComponent<AudioSource>();
		myAudioSource.clip = sound;
		myAudioSource.Play();
	}
}
