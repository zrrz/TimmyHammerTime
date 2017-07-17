using UnityEngine;
using System.Collections;

public class CamShakeSimple : MonoBehaviour  {
//	Vector3 originalCameraPosition;

	float shakeAmt = 0;

	Camera mainCamera;

	void Start() {
		mainCamera = Camera.main;
	}

	public void StartShake(float amount, float duration) {
		shakeAmt = amount;
		InvokeRepeating("CameraShake", 0, .01f);
		Invoke("StopShaking", duration);
	}

	void CameraShake()
	{
		if(shakeAmt>0) 
		{
			float quakeAmt = Random.value*shakeAmt*2 - shakeAmt;
			Vector3 pp = mainCamera.transform.position;

//			pp.y+= quakeAmt; // can also add to x and/or z
			pp += (Vector3)( Random.insideUnitCircle*shakeAmt); // can also add to x and/or z
			mainCamera.transform.position = pp;
		}
	}

	void StopShaking()
	{
		CancelInvoke("CameraShake");
//		mainCamera.transform.position = originalCameraPosition;
	}

}