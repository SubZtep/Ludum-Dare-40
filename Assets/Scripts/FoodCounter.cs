using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DemoLand.PersistData;

public class FoodCounter : MonoBehaviour {

	[Tooltip("Required food for feed the babies.")]
	public int minFood = 0;
	private Text foodText;

	private void Awake() {
		foodText = GetComponent<Text>();
		if (PersistData.Instance.Has(DataKeys.RequiredFood)) {
			minFood = (int)PersistData.Instance.Get(DataKeys.RequiredFood);
		}
	}

	public void Start() {
		Eaten();
	}

	public void Eaten() {
		int foodCount = 0;
		if (PersistData.Instance.Has(DataKeys.CurrFoodCount)) {
			foodCount = (int)PersistData.Instance.Get(DataKeys.CurrFoodCount);
		}
		string txt = "Food: "+foodCount.ToString();
		if (minFood > 0) {
			txt += "/"+minFood.ToString();
		}
		foodText.text = txt;
	}

}
