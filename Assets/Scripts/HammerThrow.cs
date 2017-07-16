using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerThrow : MonoBehaviour {

	Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
		transform.up = Vector3.Lerp(transform.up, rb.velocity, Time.deltaTime);
	}

	void OnCollisionEnter2D(Collision2D col) {

	}

	void OnTriggerEnter2D(Collider2D col) {
		if(col.GetComponent<CrystalDevice>() != null) {
			col.GetComponent<CrystalDevice>().Smash();
		}
	}
}
