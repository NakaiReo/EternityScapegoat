using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	GameObject Player;
	public float offsetX = 0;
	public float offsetY = 0;

	void Start () {
		Player = GameObject.Find("Player");
	}
	
	void Update () {
		//移動処理
		Vector3 playerPos = Player.transform.position;
		Vector3 setCameraPos = new Vector3(playerPos.x,playerPos.y,-10);
		Vector3 offset = new Vector3(offsetX, offsetY, 0);
		transform.position = setCameraPos + offset;
	}
}
