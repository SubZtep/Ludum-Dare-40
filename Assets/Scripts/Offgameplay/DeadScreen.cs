using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DemoLand.PersistData;
using UnityEngine.UI;

public class DeadScreen : MonoBehaviour {

	[Header("Attach This Prefab")]
	public GameObject reasonText;

	public void Awake() {
		string reason;
		if (PersistData.Instance.Has(DataKeys.ReasonOfDeath)) {
			reason = (string)PersistData.Instance.Get(DataKeys.ReasonOfDeath);
		} else {
			reason = "Game Over";
		}

		reasonText.GetComponent<Text>().text = reason;
	}

	public void GoHome() {
		PersistData.Instance.Set(DataKeys.NumOfHatchlings, 0);
		SceneManager.LoadScene("Home");
	}

}
