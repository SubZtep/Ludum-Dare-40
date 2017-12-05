using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodArrived : MonoBehaviour {

	private Rigidbody body;
	private bool trySetKinematic = false;

	private void Awake() {
		body = GetComponent<Rigidbody>();
		Invoke("SetTrySetKinematic", 1);
	}

	private void SetTrySetKinematic() {
		trySetKinematic = true;
	}

	private void FixedUpdate() {
		if (trySetKinematic && body.velocity == Vector3.zero) {
			body.isKinematic = true;
			trySetKinematic = false;
		}
	}
}
