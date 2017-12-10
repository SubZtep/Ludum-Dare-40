using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoLand.PersistData;

public class GameReferee : MonoBehaviour {

	private void Awake() {
		if (!PersistData.Instance.Has(DataKeys.NumOfHatchlings)) {
			PersistData.Instance.Set(DataKeys.NumOfHatchlings, 0);
		}
	}

	public void SetGoals() {
		int hatchlingsCount = (int)PersistData.Instance.Get(DataKeys.NumOfHatchlings);
		int requiredFood;
		int availableTime;
		switch (hatchlingsCount) {
			case 1:
				requiredFood = 5;
				availableTime = 60;
				break;
			case 2:
				requiredFood = 10;
				availableTime = 120;
				break;
			case 3:
				requiredFood = 15;
				availableTime = 180;
				break;
			case 4:
				requiredFood = 20;
				availableTime = 240;
				break;
			case 5:
				requiredFood = 25;
				availableTime = 300;
				break;
			default:
				requiredFood = 0;
				availableTime = 0;
				break;
		}

		PersistData.Instance.Set(DataKeys.RequiredFood, requiredFood);
		PersistData.Instance.Set(DataKeys.AvailableTime, availableTime);
	}
}
