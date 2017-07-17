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
		if(invulnerableTimer > 0f) {
			invulnerableTimer -= Time.deltaTime;
		}
	}

	float invulnerableTimer = 0f;
	public float invulnerableTime = 1f;

	public void ApplyDamage(int damage) {
		if(invulnerableTimer > 0f) {
			return;
		}
		invulnerableTimer += invulnerableTime;
		if(isPlayer)
			UpdateHealthUI(damage);
		
		currentHealth -= damage;

		if(currentHealth <= 0) {
			Die();
		} else {
			GetComponent<HumanCharacterController>().KnockBack();
		}
	}

	void Die() {
		if(isPlayer) {
			//Die animation?
			FindObjectOfType<GhettoRestartManager>().RestartLevel();
			//Game over shit
		} else {
//			Debug.LogError("Death");
			GetComponent<Animator>().Play("Death");
			SendMessage("Death");
			foreach(Collider2D col in GetComponents<Collider2D>()) {
				col.enabled = false;
			}
			if(GetComponent<SimpleEnemy>() != null)
				GetComponent<SimpleEnemy>().enabled = false;
//			Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length); //This might be looking at wrong state b4 animator changes
		}
	}

	void UpdateHealthUI(int damage) {
		for(int i = currentHealth; i > 0 && i > currentHealth - damage; i--) {
			hearts[i-1].GetComponent<Animator>().Play("HeartBreak");
		}
	}
}
