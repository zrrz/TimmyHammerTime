using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour {

	public bool isPlayer = false;
	public GameObject[] hearts;

	public int maxHealth;
	int currentHealth;

	void Start () {
		currentHealth = maxHealth;
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Q)) {
			ApplyDamage(1);
		}
	}

	void ApplyDamage(int damage) {
		if(isPlayer)
			UpdateHealthUI(damage);
		
		currentHealth -= damage;

		if(currentHealth <= 0) {
			Die();
		}
	}

	void Die() {
		if(isPlayer) {
			//Game over shit
		} else {
			GetComponent<Animator>().Play("Death");
//			SendMessage("Death");
			Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length); //This might be looking at wrong state b4 animator changes
		}
	}

	void UpdateHealthUI(int damage) {
		for(int i = currentHealth; i > 0 && i > currentHealth - damage; i--) {
			hearts[i-1].GetComponent<Animator>().Play("HeartBreak");
		}
	}
}
