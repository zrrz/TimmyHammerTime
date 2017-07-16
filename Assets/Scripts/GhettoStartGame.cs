using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhettoStartGame : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		if(Input.anyKeyDown) {
			UnityEngine.SceneManagement.SceneManager.LoadScene(1);
		}
	}
}
