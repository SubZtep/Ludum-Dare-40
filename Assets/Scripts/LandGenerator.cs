using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandGenerator : MonoBehaviour {

	[Header("Attach These Prefabs")]
	public GameObject platform;
	public GameObject player;
	public GameObject food;
	public GameObject fox;

	[Header("Obstacles")]
	public GameObject dangerousStone;
	public GameObject[] trees;

	private float platformWidth;
	private float dangerousStoneWidth;

	private int currPlatformNum;
	private Dictionary<int, GameObject> platforms;
	private int maxPlatformsAhead = 2; // Maximum number of platforms in the scene (delete rest);

	private void Awake() {
		platformWidth = platform.transform.GetComponentInChildren<Renderer>().bounds.size.x;
		dangerousStoneWidth = dangerousStone.transform.GetComponentInChildren<Renderer>().bounds.size.x;
		currPlatformNum = 0;
		platforms = new Dictionary<int, GameObject>();
		platforms.Add(0, transform.GetChild(0).gameObject);
	}

	private void AddPlatform(int platformNum) {
		// Add new platform
		if (!platforms.ContainsKey(platformNum)) {
			Transform obj = Instantiate(platform.transform, new Vector2(platformNum * platformWidth, 0), Quaternion.identity, transform);
			platforms.Add(platformNum, obj.gameObject);

			AddTree(obj);
			AddFood(obj);
			AddFox(obj);
			AddDangerousStone(obj);
		}

		// Remove unnecessary platforms
		int minNum = currPlatformNum - maxPlatformsAhead;
		int maxNum = currPlatformNum + maxPlatformsAhead;
		List<int> platformsToDestroy = new List<int>();
		foreach (KeyValuePair<int, GameObject> item in platforms) {
			if (item.Key < minNum || item.Key > maxNum) {
				platformsToDestroy.Add(item.Key);
				Destroy(item.Value);
			}
		}
		foreach (int idx in platformsToDestroy) {
			platforms.Remove(idx);
		}
	}

	private void AddTree(Transform parent) {
		int treeNum = Random.Range(0, 5);
		float posX = platformWidth / treeNum;
		for (int i = 0; i < treeNum; i++) {
			GameObject tree = trees[Random.Range(0, trees.Length)];
			// /2 because 0 is the middle point of the platform
			Vector2 pos = new Vector2(parent.position.x + (-platformWidth/2) + (posX*i) + (posX/2), 0);  
			Instantiate(tree, pos, Quaternion.identity, parent);
		}
	}

	private void AddFood(Transform parent) {
		int foodNum = Random.Range(0, 10);
		for (int i = 0; i < foodNum; i++) {
			float startX = parent.position.x - platformWidth / 2;
			Vector2 pos = new Vector2(startX + Random.Range(0f, platformWidth), 1f);
			Instantiate(food, pos, Quaternion.identity, parent);
		}
	}

	private void AddFox(Transform parent) {
		int foxNum = Random.Range(0, 3);
		for (int i = 0; i < foxNum; i++) {
			float startX = parent.position.x - platformWidth / 2;
			Vector2 pos = new Vector2(startX + Random.Range(0f, platformWidth), 1f);
			Instantiate(fox, pos, Quaternion.identity, parent);
		}
	}

	private void AddDangerousStone(Transform parent) {
		int stoneNum = Random.Range(0, 2);
		for (int i = 0; i < stoneNum; i++) {
			float startX = parent.position.x - platformWidth / 2;
			Vector2 pos = new Vector2(startX + Random.Range(0f, platformWidth) - (dangerousStoneWidth / 2), 0);
			Instantiate(dangerousStone, pos, Quaternion.identity, parent);
		}
	}

	void Update () {
		float playerPosX = player.transform.position.x;
		currPlatformNum = (int)Mathf.Round(playerPosX / platformWidth);
		float playerPlatformPos = playerPosX - (platformWidth * currPlatformNum) + (platformWidth / 2);
		if (playerPlatformPos > platformWidth / 2) {
			AddPlatform(currPlatformNum + 1); // Create platform forward
		} else {
			AddPlatform(currPlatformNum - 1); // Create platform backward
		}
	}
}
