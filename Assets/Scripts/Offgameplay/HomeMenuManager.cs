using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DemoLand.PersistData;

public class HomeMenuManager : MonoBehaviour {

	[Header("Attach These Prefabs")]
	public GameReferee gameReferee;
	public GameObject closeButton;

	[Header("Sound On/Off")]
	public GameObject soundButton;
	public Sprite soundOnIcon;
	public Sprite soundOffIcon;

	[Header("Pages")]
	public GameObject helpPanel;
	public GameObject creditsPanel;


	private void Awake() {
		if (!gameReferee) {
			gameReferee = GetComponent<GameReferee>();
		}

		#if UNITY_WEBGL
		closeButton.SetActive(false);
		#endif

		if (!PersistData.Instance.Has(DataKeys.PlaySound)) {
			PersistData.Instance.Set(DataKeys.PlaySound, true);
		}

		helpPanel.SetActive(false);
		creditsPanel.SetActive(false);
	}

	public void StartGame() {
		gameReferee.SetGoals();
		SceneManager.LoadScene("Game");
	}

	public void Close() {
		#if !UNITY_WEBGL
		Application.Quit();
		#endif
	}

	void Update() {
		if (Input.GetKey("escape")) {
			Close();
		}
	}

	public void OpenHelp() {
		helpPanel.SetActive(true);
	}

	public void OpenCredits() {
		creditsPanel.SetActive(true);
	}

	public void SoundToggle() {
		Image img = soundButton.GetComponent<Image>();
		bool playSound = !(bool)PersistData.Instance.Get(DataKeys.PlaySound);
		PersistData.Instance.Set(DataKeys.PlaySound, playSound);
		img.sprite = playSound ? soundOnIcon : soundOffIcon;
		if (playSound) {
			GetComponent<AudioSource>().Play();
		}
	}
}
