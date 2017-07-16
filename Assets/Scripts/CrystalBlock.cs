using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBlock : MonoBehaviour {

	public enum CrystalColor {
		Red, Orange, Yellow, Blue, Green, Purple
	}

	public CrystalColor color;

	public AudioClip breakSound;
	AudioSource audioSource;

	void Start() {
		audioSource = gameObject.AddComponent<AudioSource>();
	}

	public void Break(float delay = 0f) {
		StartCoroutine(DelayBreak(delay));
	}

	IEnumerator DelayBreak(float delay) {
		yield return new WaitForSeconds(delay);

		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<Collider2D>().enabled = false;
		transform.GetChild(0).gameObject.SetActive(true);

		audioSource.clip = breakSound;
		audioSource.Play();

		yield return new WaitForSeconds(0.5f); //I'm just going to hardcode the anim length fuck it
		Destroy(gameObject);
	}
}
