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

		if(col.gameObject.layer == LayerMask.NameToLayer("Ground") || col.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
			if(rb.velocity.magnitude > 2.4f)
				Camera.main.GetComponent<CamShakeSimple>().StartShake(0.2f, 0.2f);
		}
//		Debug.LogError("Col: " + col.gameObject);
//		if(col.gameObject.layer == LayerMask.GetMask("Enemy")) {
//			col.gameObject.GetComponent<HealthHandler>().ApplyDamage(1);
//		}
	}

	void OnTriggerEnter2D(Collider2D col) {
//		Debug.LogError("trigger: " + col.gameObject);
//		if(col.gameObject.layer == LayerMask.GetMask("Enemy")) {
//			col.gameObject.GetComponent<HealthHandler>().ApplyDamage(1);
//		}
		if(col.GetComponent<CrystalDevice>() != null) {
			col.GetComponent<CrystalDevice>().Smash();
		}
	}
}
