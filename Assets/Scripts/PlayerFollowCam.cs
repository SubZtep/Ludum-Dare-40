using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowCam : MonoBehaviour
{
	public float startMovePosY = 4f;

	[SerializeField] private GameObject player;
	//private Camera cam;

	// For camera rotation
	private Vector3 startPos;
	private Quaternion startRot;

	private void Awake() {
		startPos = transform.position;
		startRot = transform.rotation;
		//cam = GetComponent<Camera>();
	}

	private void LateUpdate() {
		// If the player is too high, move and rotate the camera for better perspective
		Vector3 pos = transform.position;
		pos.y = startPos.y;
		pos.z = startPos.z;
		if (player.transform.position.y > startMovePosY) {
			float moveUp = player.transform.position.y - startMovePosY;
			pos.y += moveUp * 1.1f;
			pos.z -= moveUp * 1.7f;
			transform.rotation = startRot * Quaternion.AngleAxis(moveUp * 1.8f, Vector3.right);
		} else {
			transform.rotation = startRot;
		}

		// Don't follow the player exactly in the middle of the screen
		//Vector3 screenPos = cam.WorldToScreenPoint(player.transform.position);
		//print("Screen: "+screenPos.x+ ", Width: "+Screen.width+", Cam pos: "+pos.x);

		//print(cam.pixelWidth);





		/*if (screenPos.x < Screen.width * 0.25 || screenPos.x > Screen.width * 0.75) {
			// Follow player
			//pos.x += (Screen.width / 2) - player.transform.position.x;
			//print(cam.ScreenToWorldPoint(player.transform.position).x);
			print(screenPos.x + " - " + cam.ScreenToWorldPoint(new Vector3(screenPos.x, 0, 0))); // 100
			print(Screen.width + " - " + cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0))); // 687
			//print(cam.ScreenToWorldPoint(screenPos).x); // -3.370001

			//print("Screen width: "+Screen.width+", ScreenPos.x: "+screenPos.x);
			//print(cam.ViewportToWorldPoint( new Vector2(1, 1)));
			

			//s.x = player.transform.position.x + cam.ScreenToWorldPoint(Screen.width - screenPos.x).x;
			//pos.x = player.transform.position.x + 2.5f;

			//print("transform.position.x: "+player.transform.position.x);
			//pos.x = player.transform.position.x + 2.5f;
			//cam.transform.position += transform.position;
		}*/


		//print(transform.position.x);


		//pos.x = player.transform.position.x - cam.ScreenToWorldPoint(new Vector3(-40, 0, 0)).x;
		//pos.x = cam.ScreenToWorldPoint(new Vector3(-40, 0, 0)).x;
		pos.x = player.transform.position.x; // old follow
		//pos.x = player.transform.position.x + 2;
		//print("pos.x: "+pos.x);

		//float offsetX = 2f;

		//print()
		//pos.x = player.transform.position.x + offsetX;



		transform.position = pos;
	}
}
