using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooSplash : MonoBehaviour {

	private void Start() {
		Invoke("Suicide", 5);
	}

	private void OnTriggerEnter(Collider other) {
		GetComponent<Rigidbody>().isKinematic = true;
		transform.localScale = new Vector3(0.2f, 0.05f, 0.15f);
		Collider[] cols = GetComponents<Collider>();
		foreach(Collider col in cols) {
			if (!col.isTrigger) {
				col.enabled = false;
			}
		}
		Vector3 pos = transform.position;
		pos.z = 0.5f;
		transform.position = pos;
	}

	private void Suicide() {
		Destroy(this.gameObject);
	}
}
