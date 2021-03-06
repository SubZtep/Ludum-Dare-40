﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DemoLand.PersistData;
using UnityEngine.SceneManagement;

public class Nest : MonoBehaviour {

	[Header("Attach These Prefabs")]
	public GameObject player;
	public GameObject foodText;
	public GameObject timeText;
	public GameObject winPanel;

	private Rigidbody pBody;
	private bool winnerMessageOn = false;

	private void Awake() {
		pBody = player.GetComponent<Rigidbody>();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {

			int minFood = 0;
			if (PersistData.Instance.Has(DataKeys.RequiredFood)) {
				minFood = (int)PersistData.Instance.Get(DataKeys.RequiredFood);
			}
			if (minFood == 0) GoHome();

			int foodCount = 0;
			if (PersistData.Instance.Has(DataKeys.CurrFoodCount)) {
 				foodCount = (int)PersistData.Instance.Get(DataKeys.CurrFoodCount);
			}

			if (foodCount >= minFood) {

				int hatchlingsCount = 0;
				if (PersistData.Instance.Has(DataKeys.NumOfHatchlings)) {
					hatchlingsCount = (int)PersistData.Instance.Get(DataKeys.NumOfHatchlings);
				}

				switch (hatchlingsCount) {
					case 0:
					case 1:
					case 2:
					case 3:
					case 4:
						hatchlingsCount++;
						PersistData.Instance.Set(DataKeys.NumOfHatchlings, hatchlingsCount);
						GoHome();
						break;
					case 5:
						Winner();
						break;
				}
			} else {
				pBody.velocity = Vector3.zero;
				pBody.AddForce(Vector3.down * 400f);
			}
		}
	}

	private void GoHome() {
		SceneManager.LoadScene("Home");
	}

	private void Winner() {
		foodText.SetActive(false);
		timeText.SetActive(false);
		winPanel.SetActive(true);
		Time.timeScale = 0f;
		StartCoroutine(SetWinnerMessageOn());
	}


	private IEnumerator SetWinnerMessageOn() {
		yield return new WaitForSecondsRealtime(3);
		winnerMessageOn = true;
	}

	private void Update() {
		if (winnerMessageOn) {
			if (Input.anyKey) {
				PersistData.Instance.Del(DataKeys.NumOfHatchlings);
				PersistData.Instance.Del(DataKeys.RequiredFood);
				PersistData.Instance.Del(DataKeys.AvailableTime);
				Time.timeScale = 1f;
				GoHome();
			}
		}
	}
}
