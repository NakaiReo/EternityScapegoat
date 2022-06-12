using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallBuyScript : MonoBehaviour {

	GameObject Player;
	PlayerStatus playerStatus;
	PlayerShotBullet playerShotBullet;

	Sprite[] KnifeSprite;
	Sprite[] GunsSprite;
	Image GunImage;
	GameObject PriseUI;
	GameObject ImageUI;
	bool inPlayer = false;
	bool canBuy = false;
	bool colorChange = false;

	[SerializeField]
	int gunID;
	[SerializeField]
	int price;

	void Start()
	{
		Player = GameObject.Find("Player");
		playerStatus = Player.GetComponent<PlayerStatus>();
		playerShotBullet = Player.GetComponent<PlayerShotBullet>();

		KnifeSprite = Resources.LoadAll<Sprite>("KnifeImage");
		GunsSprite = Resources.LoadAll<Sprite>("Guns");

		PriseUI = transform.Find("WallBuyUI").gameObject;
		PriseUI.SetActive(false);
		PriseUI.transform.Find("Price").gameObject.GetComponent<Text>().text = "Price : " + price;
		PriseUI.transform.Find("GunName").gameObject.GetComponent<Text>().text = playerShotBullet.ReturnGunName(gunID);

		ImageUI = transform.Find("WallBuyImage").gameObject;
		GunImage = ImageUI.transform.Find("GunImage").gameObject.GetComponent<Image>();
		GunImage.sprite = SpriteChange(gunID);
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
					playerStatus.GetMoney(-price);
					if (PlayerShotBullet.gunsID[PlayerShotBullet.haveGunID] == gunID)
					{
						playerShotBullet.AmmoRefill();
					}
					else
					{
						playerShotBullet.TradeWeapon(gunID);
					}
				}
			}
		}
		else
		{
			PriseUI.SetActive(false);
		}
	}

	Sprite SpriteChange(int id)
	{
		if (id < 0)
		{
			int id2 = (int)Mathf.Abs(id) - 1;
			return KnifeSprite[id2];
		}
		return GunsSprite[id];
	}
}
