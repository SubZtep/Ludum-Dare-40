using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoLand.PersistData;
using UnityEngine.Events;

public class PlayerEat : MonoBehaviour {

	[Header("Basic Prefabs")]
	[Tooltip("HUD Text")]
	public FoodCounter foodCounter;
	[Tooltip("Poo Prefab")]
	public GameObject poo;

	[Header("Settings")]
	[Tooltip("Start eaten food number. (for debug, should be 0)")]
	public int startFoodNum = 4;
	[Tooltip("Number of food when player start to overweight.")]
	public int startOverweight = 5;
	[Tooltip("Number of food when player is too full and have to release.")]
	public int maxFoodNum = 20;
	[Tooltip("Number of food what player release when too full.")]
	public int releaseFoodNum = 5;
	[Tooltip("Percentage of loose all control ability values per food.")]
	public float pcLoose = 0.5f;

	[Header("Sounds")]
	[Tooltip("Eat sounds.")]
	public AudioClip[] eatAudio;
	[Tooltip("Poo sounds.")]
	public AudioClip[] pooAudio;

	private Rigidbody body;
	private Renderer rend;
	private PlayerControl control;
	private int releaseLeft = 0; // after got sick the amout of poo still has to release

	private Vector3 origTransScale;
	private Vector3 origColSize;
	private Vector3 origColCenter;
	private float origSpeed;
	private float origJumpSpeed;
	private float origMaxUpPosition;
	private float origSlowDownPosY;
	private float origMinSpeed;

	private bool playSound = true;
	private AudioSource audioSource;

	private void Awake() {
		body = GetComponent<Rigidbody>();
		rend = transform.GetComponentInChildren<Renderer>();
		control = GetComponent<PlayerControl>();

		origTransScale = transform.localScale;
		origSpeed = control.speed;
		origJumpSpeed = control.jumpSpeed;
		origMaxUpPosition = control.maxUpPosition;
		origSlowDownPosY = control.slowDownPosY;
		origMinSpeed = control.minSpeed;

		audioSource = GetComponent<AudioSource>();
		if (PersistData.Instance.Has(DataKeys.PlaySound) && !(bool)PersistData.Instance.Get(DataKeys.PlaySound)) {
			playSound = false;
		}
	}

	private void Start() {
		PersistData.Instance.Set(DataKeys.CurrFoodCount, startFoodNum);
		foodCounter.Eaten();
		ScalePlayer();
	}

	public void ScalePlayer() {
		int foodCount = GetFoodCount();
		Vector3 newScale = origTransScale;

		if (foodCount >= startOverweight) {
			newScale.y += (foodCount-startOverweight) * 0.1f;
			newScale.z += (foodCount-startOverweight) * 0.05f;

			control.speed -= (control.speed/100*pcLoose)*(foodCount-startOverweight);
			control.jumpSpeed -= (control.jumpSpeed/100*pcLoose)*(foodCount-startOverweight);
			control.maxUpPosition -= (control.maxUpPosition/100*pcLoose)*(foodCount-startOverweight);
			control.slowDownPosY -= (control.slowDownPosY/100*pcLoose)*(foodCount-startOverweight);
			control.minSpeed -= (control.minSpeed/100*pcLoose)*(foodCount-startOverweight);
		} else {
			control.speed = origSpeed;
			control.jumpSpeed = origJumpSpeed;
			control.maxUpPosition = origMaxUpPosition;
			control.slowDownPosY = origSlowDownPosY;
			control.minSpeed = origMinSpeed;
		}
		transform.localScale = newScale;
	}

	private void ReleaseFood() {
		int foodCount = GetFoodCount();
		if (foodCount >= maxFoodNum) {
			releaseLeft = releaseFoodNum;
			body.velocity = Vector3.zero;
			body.AddForce(Vector3.up * 6f, ForceMode.Impulse);
			ReleasingFood();
			LookSick();
		}
	}

	private void LookSick() {
		rend.material.SetColor("_EmissionColor", Color.red);
		rend.material.EnableKeyword("_EMISSION");
	}

	private void UnlookSick() {
		rend.material.DisableKeyword("_EMISSION");
	}

	public void ReleasingFood() {
		Poo(true);
		if (--releaseLeft > 0) {
			Invoke("ReleasingFood", 0.1f);
		} else {
			UnlookSick();
			ScalePlayer();
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (releaseLeft > 0) return;
		if (other.tag == "Food") {
			Destroy(other.gameObject);
			int foodCount = GetFoodCount();
			foodCount++;
			PersistData.Instance.Set(DataKeys.CurrFoodCount, foodCount);
			foodCounter.Eaten();
			PlayEat();
			ReleaseFood();
			ScalePlayer();
		}
	}

	private int GetFoodCount() {
		if (PersistData.Instance.Has(DataKeys.CurrFoodCount)) {
			return (int)PersistData.Instance.Get(DataKeys.CurrFoodCount);
		}
		return 0;
	}

	public void Poo(bool release = false) {
		int foodCount = GetFoodCount();
		if (foodCount == 0) return;
		Vector3 pos = transform.position;
		Vector3 force = Vector3.down;
		if (release) {
			force = transform.GetChild(0).rotation.y == 0 ? Vector3.left : Vector3.right;
			force += Vector3.up / 2;
			force /= 2;
			pos += Vector3.up / 2;
		}
		GameObject pooObj = (GameObject)Instantiate(poo, pos, Quaternion.identity);
		pooObj.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
		PersistData.Instance.Set(DataKeys.CurrFoodCount, --foodCount);
		foodCounter.Eaten();
		ScalePlayer();
		PlayPoo();
	}

	private void PlayEat() {
		if (!playSound || eatAudio.Length == 0) return;
		audioSource.clip = eatAudio[Random.Range(0, eatAudio.Length)];
		audioSource.Play();
	}

	private void PlayPoo() {
		if (!playSound || pooAudio.Length == 0) return;
		audioSource.clip = pooAudio[Random.Range(0, pooAudio.Length)];
		audioSource.Play();
	}
}
