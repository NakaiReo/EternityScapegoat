using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorScript : MonoBehaviour {

	GameObject Player;
	PlayerStatus playerStatus;

	GameObject PriseUI;
	bool inPlayer = false;
	bool canBuy = false;
	bool colorChange = false;

	[SerializeField]
	int price;

	void Start () {
		Player = GameObject.Find("Player");
		playerStatus = Player.GetComponent<PlayerStatus>();

		PriseUI = transform.Find("DoorUI").gameObject;
		PriseUI.transform.Find("Price").gameObject.GetComponent<Text>().text = "Price : " + price;
		PriseUI.SetActive(false);
	}

	private void Update()
	{
		if (Mathf.Approximately(Time.timeScale, 0f)) return;

		Vector2 distancePos = Player.transform.position - transform.position;
		float distace = Mathf.Sqrt(Mathf.Pow(distancePos.x, 2) + Mathf.Pow(distancePos.y, 2));

		inPlayer = distace <= 2.0f ? true : false;

		if (inPlayer == true)
		{
			PlayerStatus.useKeyViewBool = true;
			canBuy = playerStatus.GetMoneyValue() >= Artifact.ClacMoney(price) ? true : false;
			PriseUI.transform.Find("Price").gameObject.GetComponent<Text>().text = "Price : " + Artifact.ClacMoney(price);

			PriseUI.SetActive(true);

			if (canBuy == true && colorChange == true)
			{
				PlayerStatus.useKeyCanBool = false;
				colorChange = false;
				PriseUI.transform.Find("BackGround").gameObject.GetComponent<Image>().color = new Color32(119, 255, 51, 255);
			}
			else if (canBuy == false && colorChange == false)
			{
				PlayerStatus.useKeyCanBool = true;
				colorChange = true;
				PriseUI.transform.Find("BackGround").gameObject.GetComponent<Image>().color = new Color32(255, 51, 51, 255);
			}

			if (canBuy == true)
			{
				if (Input.GetKeyDown(KeyConfig.Use) || Input.GetButtonDown("C_Use"))
				{
					SoundDirector.PlaySE("OpenDoor");
					playerStatus.GetMoney(-price);
					Destroy(gameObject);
				}
			}
		}
		else
		{
			PriseUI.SetActive(false);
		}
	}
}
