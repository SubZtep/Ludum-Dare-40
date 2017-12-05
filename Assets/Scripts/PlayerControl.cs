//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoLand.PersistData;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class PlayerControl : MonoBehaviour
{
	[Header("Settings")]
	[Tooltip("Horizontal movement speed.")]
	public float speed = 10f;
	[Tooltip("Fly up force speed.")]
	public float jumpSpeed = 300f;
	[Tooltip("Can't fly higher from this altitude.")]
	public float maxUpPosition = 9f;
	[Tooltip("Start to be slower close to the ground below this position.")]
	public float slowDownPosY = 3f;
	[Tooltip("Absolute horizontal minimum speed (on the floor).")]
	public float minSpeed = 0.2f;

	[Header("Sounds")]
	[Tooltip("Jump sounds.")]
	public AudioClip[] jumpAudio;
	[Tooltip("Caught sounds.")]
	public AudioClip[] caughtAudio;
	[Tooltip("Idle sounds.")]
	public AudioClip[] idleAudio;

	private Transform pigeonObj;
	private Rigidbody body;
	private Transform trans;
	private Animator anim;
	private PlayerEat eat;
	private bool playSound = true;
	private AudioSource audioSource;

	private void Awake() {
		body = GetComponent<Rigidbody>();
		trans = body.transform;
		pigeonObj = trans.GetChild(0);
		anim = pigeonObj.GetComponent<Animator>();
		eat = GetComponent<PlayerEat>();

		audioSource = GetComponent<AudioSource>();
		if (PersistData.Instance.Has(DataKeys.PlaySound) && !(bool)PersistData.Instance.Get(DataKeys.PlaySound)) {
			playSound = false;
		}
	}

	private void FixedUpdate() {
		// Horizontal movement
		float hAxis = Input.GetAxis("Horizontal");
		if (hAxis != 0) {
			float posY = transform.position.y;
			float pc = 0;
			if (posY < slowDownPosY) {
				pc = posY / slowDownPosY;
				if (pc < minSpeed) pc = minSpeed;
				hAxis = pc * hAxis;
			}
			trans.Translate(hAxis * speed * Time.deltaTime, 0, 0);
		}

		// Vertical movement
		if (trans.position.y < maxUpPosition && Input.GetButtonDown("Jump")) {
			Vector3 currVelo = body.velocity;
			currVelo.y = 0;
			body.velocity = currVelo;
			body.AddForce(Vector3.up * jumpSpeed);
			PlayJump();
		}

		// Heading
		Vector3 rot = pigeonObj.rotation.eulerAngles;
		if (hAxis > 0) {
			rot.y = 0;
		} else if (hAxis < 0) {
			rot.y = 180f;
		}
		pigeonObj.rotation = Quaternion.Euler(rot);

		// Poo
		if (Input.GetButtonDown("Fire1")) {
			eat.Poo();
		}

		// Set anim FIXME!!! (speed, etc)
		anim.SetFloat("hVelocity", Mathf.Abs(hAxis));
		anim.SetFloat("vPos", trans.position.y < 0.1 ? 0 : trans.position.y);
		anim.SetFloat("velMagnitude", body.velocity.magnitude);

		if (body.velocity.magnitude == 0) {
			Invoke("MightPlayIdle", 5);
		} else {
			CancelInvoke("MightPlayIdle");
		}
	}

	private void MightPlayIdle() {
		if (body.velocity.magnitude == 0 && !audioSource.isPlaying) {
			CancelInvoke("MightPlayIdle");
			PlayIdle();
		}	
	}

	private void PlayJump() {
		if (!playSound || jumpAudio.Length == 0) return;
		audioSource.clip = jumpAudio[Random.Range(0, jumpAudio.Length)];
		audioSource.Play();
	}

	public void PlayCaught() {
		if (!playSound || caughtAudio.Length == 0) return;
		audioSource.clip = caughtAudio[Random.Range(0, caughtAudio.Length)];
		audioSource.Play();
	}

	public void PlayIdle() {
		if (!playSound || idleAudio.Length == 0) return;
		audioSource.clip = idleAudio[Random.Range(0, idleAudio.Length)];
		audioSource.Play();
	}
}
