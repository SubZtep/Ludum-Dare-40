using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DemoLand.PersistData;

public class DrawHomeLevel : MonoBehaviour {

	[Header("Attach These Prefabs")]
	[Tooltip("Main canvas for bird images.")]
	public GameObject canvas;
	[Tooltip("Canvas draw starts here.")]
	public GameObject referencePoint;
	public GameObject missionText;
	public GameObject pigeonImage;
	public GameObject hatchlingImage;
	public GameObject startButton;

	public Sprite[] pigeons;

	void Start () {

		int hatchlingsNum = (int)PersistData.Instance.Get(DataKeys.NumOfHatchlings);

		string missionTxt;
		switch (hatchlingsNum) {
			case 1: missionTxt = "One little birdie, looks so hungry, bring some food or it’s not funky."; break;
			case 2: missionTxt = "Two little birdies, double dynamite, double trouble, work as a couple."; break;
			case 3: missionTxt = "Three little birdies, like in a tale, find extra seeds in the fox-tail."; break;
			case 4: missionTxt = "Four little birdies, never enough, watch just their fight for some cream puff."; break;
			case 5: missionTxt = "Five little birdies, a small swarm of bee, I think the best if they just eat slowly me."; break;
			default: missionTxt = "Go and enjoy the good weather, don’t sit all day front of that stupid computer!"; break;
		}
		missionText.GetComponent<Text>().text = missionTxt;

		/*Vector3 basePos = referencePoint.transform.position + (Vector3.up * 40);
		Transform birdCage = referencePoint.transform.parent.transform;
		// Draw hatchlings
		hatchlingsNum = 4;
		for (int i = 0; i < hatchlingsNum; i++) {
			Vector3 hatchlingPos = basePos;
			hatchlingPos += Vector3.right * Random.Range(30, 40) * i;
			hatchlingPos += Vector3.right * 110;
			hatchlingPos += Vector3.up * Random.Range(0, 20);
			Instantiate(hatchlingImage, hatchlingPos, Quaternion.identity, birdCage);
			//Instantiate(hatchlingImage, basePos + Vector3.right*Random.Range(30, 40)*i+Vector3.right*110 + Vector3.up*Random.Range(0, 20), Quaternion.identity, birdCage);
		}
		// Draw mother pigeon
		Instantiate(pigeonImage, basePos + (Vector3.left * 120) , Quaternion.identity, birdCage);*/

		referencePoint.GetComponent<Image>().sprite = pigeons[hatchlingsNum];
	}

}
