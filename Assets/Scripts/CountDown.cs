using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DemoLand.PersistData;
using UnityEngine.SceneManagement;

public class CountDown : MonoBehaviour {

	[Tooltip("Seconds before babies starve.")]
	public int timeLeft = 3600;
	private Text timeText;

	private void Awake() {

		if (PersistData.Instance.Has(DataKeys.NumOfHatchlings)) {
			int num = (int)PersistData.Instance.Get(DataKeys.NumOfHatchlings);
			if (num == 0) {
				Destroy(this.gameObject);
			}
		}

		timeText = GetComponent<Text>();
		if (PersistData.Instance.Has(DataKeys.AvailableTime)) {
			timeLeft = (int)PersistData.Instance.Get(DataKeys.AvailableTime);
		}
		InvokeRepeating("TimeFlow", 0, 1);
	}

	private void TimeFlow() {
		timeLeft--;
		if (timeLeft <= 0) {
			string name = "";
			if (PersistData.Instance.Has(DataKeys.NumOfHatchlings)) {
				int hatchlingsCount = (int)PersistData.Instance.Get(DataKeys.NumOfHatchlings);
				if (hatchlingsCount > 0) {
					name = hatchlingsCount == 1 ? "hatchling" : "hatchlings";
				}
			}
			if (name == "") {
				name = "mates";
			}
			PersistData.Instance.Set(DataKeys.ReasonOfDeath, "Time is up, your "+name+" aren’t hungry anymore.");
			SceneManager.LoadScene("Dead");
		}
		timeText.text = "Longevity: "+timeLeft.ToString();
	}
}
