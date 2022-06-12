using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretWeapon : MonoBehaviour {

	GameObject Player;
	PlayerStatus playerStatus;
	PlayerShotBullet playerShotBullet;

	bool inPlayer;
	bool alredeyBuy = false;

	void Start () {
		Player = GameObject.Find("Player");
		playerStatus = Player.GetComponent<PlayerStatus>();
		playerShotBullet = Player.GetComponent<PlayerShotBullet>();
	}

	void Update()
	{
		if (Mathf.Approximately(Time.timeScale, 0f)) return;

		Vector2 distancePos = Player.transform.position - transform.position;
		float distace = Mathf.Sqrt(Mathf.Pow(distancePos.x, 2) + Mathf.Pow(distancePos.y, 2));

		inPlayer = distace <= 2.0f ? true : false;

		if (inPlayer && !alredeyBuy)
		{
			if (Input.GetKeyDown(KeyConfig.Use) || Input.GetButtonDown("C_Use"))
			{
				alredeyBuy = true;
				playerShotBullet.TradeWeapon(34);
			}
		}
	}
}
