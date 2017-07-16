using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalDevice : MonoBehaviour {

	List<CrystalBlock> crystals;

	public CrystalBlock.CrystalColor color;

	public AudioClip breakSound;
	AudioSource audioSource;

	void Start () {
		audioSource = gameObject.AddComponent<AudioSource>();
		crystals = new List<CrystalBlock>();
		foreach(CrystalBlock block in FindObjectsOfType<CrystalBlock>()) {
			if(block.color == color) {
				crystals.Add(block);
			}
		}

//		crystals.Sort((c1, c2)=>Vector3.Distance(transform.position, c1.transform.position).CompareTo(Vector3.Distance(transform.position, c2.transform.position)));
	}

	public void Smash() {
		for(int i = 0; i < crystals.Count; i++) {
			float delay = Vector3.Distance(transform.position, crystals[i].transform.position) / 10f;
			crystals[i].Break(delay);
		}
		audioSource.clip = breakSound;
		audioSource.Play();
		Destroy(gameObject);
	}
}
