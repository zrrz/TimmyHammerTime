using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhettoMainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.Find("DustTrail_0").GetComponent<SpriteRenderer>().enabled = false;
		GetComponentInChildren<Animator>().Play( Animator.StringToHash( "Idle" ) );
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
