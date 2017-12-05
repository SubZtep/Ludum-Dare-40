using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxAI : MonoBehaviour {

	public GameObject target; // Fox want to eat >:]
	public float speed = 30f;
	public float jumpSpeed = 10f;
	public float freezeTime = 5f;  // got pooed :D
	private Transform targetTrans;
	private Rigidbody body;
	private Transform foxObj;
	private Animator anim;
	private CatchThat catchThat;
	private bool blinking = false; // For frozen state (by poo)
	private Renderer rend;

	private void Awake() {
		if (!target) target = GameObject.FindWithTag("Player");
		targetTrans = target.transform;
		body = GetComponent<Rigidbody>();
		foxObj = transform.GetChild(0);
		anim = foxObj.GetComponent<Animator>();
		catchThat = GetComponent<CatchThat>();
		rend = transform.GetComponentInChildren<Renderer>();
	}
	
	private void FixedUpdate() {
		if (blinking) return; // Fox is frozen, do nothing

		Vector3 tPos = targetTrans.position;
		Vector3 pos = transform.position;
		float tDir = 0;
		float distance = Vector3.Distance(tPos, pos);

		// Ignorance
		float yDistance = Mathf.Abs(tPos.y - pos.y);
		if (yDistance > 3f || distance > 20f) {
			// Pigeon is too far
			return;
		}

		// Heading
		Vector3 rot = foxObj.rotation.eulerAngles;
		if (tPos.x > pos.x) {
			rot.y = 0;
			tDir = 1f;
		} else if (tPos.x < pos.x) {
			rot.y = 180f;
			tDir = -1f;
		}
		foxObj.rotation = Quaternion.Euler(rot);

		// Follow
		float xDistance = Mathf.Abs(tPos.x - pos.x);
		if (xDistance > 1.5f) {
			Vector3 force = (tDir < 0 ? Vector3.left : Vector3.right) * speed;

			if (Mathf.Abs(force.x) > 0f && body.velocity.magnitude < 0.001f) {
				// Fox stuck for some reason, let's jump!
				transform.Translate(Vector3.up * jumpSpeed * Time.deltaTime);
				anim.SetTrigger("doJump");
			}
			body.AddForce(force);
			anim.SetFloat("hVelocity", Mathf.Abs(force.x));
		} else {
			anim.SetFloat("hVelocity", 0);
		}

		// Catch
		if (distance <= 1.5f) {
			catchThat.Caught();
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Poo") {
			Destroy(other.gameObject);
			blinking = true;
			Vector3 pos = transform.position;
			pos.z = 1.3f;
			transform.position = pos;
			anim.enabled = false;
			InvokeRepeating("Blink", 0, 0.1f);
			Invoke("StopBlink", freezeTime);
		}
	}

	private void Blink() {
		if (blinking) {
			rend.enabled = !rend.enabled;
		}
	}

	private void StopBlink() {
		blinking = false;
		CancelInvoke("Blink");
		rend.enabled = true;
		anim.enabled = true;
		Vector3 pos = transform.position;
		pos.z = 0f;
		transform.position = pos;
	}
}
