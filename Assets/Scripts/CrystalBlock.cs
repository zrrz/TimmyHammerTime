using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBlock : MonoBehaviour {

	public enum CrystalColor {
		Red, Orange, Yellow, Blue, Green, Purple
	}

	public CrystalColor color;

	public void Break(float delay = 0f) {
		StartCoroutine(DelayBreak(delay));
	}

	IEnumerator DelayBreak(float delay) {
		yield return new WaitForSeconds(delay);
		//Do animation
//		yield return new WaitForSeconds(GetComponent<Animation>().GetClip("Break").length;
		Destroy(gameObject);
	}
}
