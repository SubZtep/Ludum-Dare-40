using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxSelector : MonoBehaviour {

	public Light directionalLight;
	public Material[] skyboxes;

	private void Awake() {
		if (skyboxes == null || skyboxes.Length == 0) return;

		int boxIdx = Random.Range(0, skyboxes.Length);
		boxIdx = 2;
		RenderSettings.skybox = skyboxes[boxIdx];

		// Skybox specific graphics settings
		/*switch (boxIdx) {
			case 0:
				// 0_SkyBlue
				//RenderSettings.ambientIntensity
				break;
			case 1:
				// 1_SkyBlue2
				break;
			case 2:
				// 2_SkyDarkBlue
				break;
			case 3:
				// 3_SkyDarkGrey
				break;
			case 4:
				// 4_SkyDarkPurple
				break;
		}*/
	}
}
