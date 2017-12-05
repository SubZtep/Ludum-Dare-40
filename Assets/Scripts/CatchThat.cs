using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoLand.PersistData;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CatchThat : MonoBehaviour {

	public GameObject target;
	private Rigidbody tBody;
	private Renderer tRenderer;
	private PlayerEat tPlayerEat;
	private PlayerControl tPlayerControl;
	private FoodCounter foodCounter;

	private bool blinking = false;
	private bool dead = false;

	private void Awake() {
		if (!target) target = GameObject.FindWithTag("Player");
		tRenderer = target.transform.GetComponentInChildren<Renderer>();
		tPlayerEat = target.GetComponent<PlayerEat>();
		tPlayerControl = target.GetComponent<PlayerControl>();
		tBody = target.GetComponent<Rigidbody>();
		foodCounter = GameObject.FindWithTag("HUDFoodText").GetComponent<FoodCounter>();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == target.tag) {
			Caught();
		}
	}

	public void Caught() {
		if (dead) return;
		if (PersistData.Instance.Has(DataKeys.CurrFoodCount) && (int)PersistData.Instance.Get(DataKeys.CurrFoodCount) > 0) {
			// Blinking object, fly away a bit, loose food
			if (!blinking) {
				blinking = true;
				InvokeRepeating("Blink", 0, 0.1f);
				Invoke("StopBlink", 1.5f);
				tBody.velocity = Vector3.zero;
				tBody.AddForce(Vector3.up * 8f, ForceMode.Impulse); //Fixme: use better direction (?)
				PersistData.Instance.Set(DataKeys.CurrFoodCount, 0);
				foodCounter.Eaten();
				tPlayerEat.ScalePlayer();
				tPlayerControl.PlayCaught();
				//TODO: lost food effect
			}
		} else if (!blinking) {
			// Die
			string name = "";
			if (PersistData.Instance.Has(DataKeys.NumOfHatchlings)) {
				int hatchlingsCount = (int)PersistData.Instance.Get(DataKeys.NumOfHatchlings);
				if (hatchlingsCount > 0) name = hatchlingsCount == 1 ? "hatchling" : "hatchlings";
			}
			string msg = "Don’t collect more food. You are the food.";
			if (name != "") msg += " Just like your "+name+". Shortest family tradition ever.";
			PersistData.Instance.Set(DataKeys.ReasonOfDeath, msg);
			SceneManager.LoadScene("Dead");
		}
	}

	private void Blink() {
		if (blinking) {
			tRenderer.enabled = !tRenderer.enabled;
		}
	}

	private void StopBlink() {
		blinking = false;
		CancelInvoke("Blink");
		tRenderer.enabled = true;
	}
}
