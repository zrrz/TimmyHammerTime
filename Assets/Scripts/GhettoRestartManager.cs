using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhettoRestartManager : MonoBehaviour {

	void Start() {
		transform.Find("Canvas").parent = null;
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.R)) {
			RestartLevel();
		}

	}

	public void RestartLevel() {
//		FindObjectOfType<HumanCharacterController>().gameObject.SetActive(false);
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
	}
}
