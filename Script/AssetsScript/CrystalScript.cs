using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalScript : MonoBehaviour
{
	GameObject HaveCrystalPower;
	Sprite[] CrystalPowerImage;
	Transform CrystalPowerUI;

	GameObject Player;

	PlayerStatus playerStatus;
	PlayerController playerController;
	PlayerShotBullet playerShotBullet;

	GameObject CrystalUI;
	GameObject Particle;

	[SerializeField]
	int price;

	[SerializeField, TooltipAttribute("0: クイックキュア")]
	int id;

	bool inPlayer = false;
	bool canBuy = false;
	bool colorChange = false;

	bool alreadyBuy = false;

	void Start()
	{
		HaveCrystalPower = Resources.Load("HaveCrystalPower") as GameObject;
		CrystalPowerImage = Resources.LoadAll<Sprite>("CrystalPower");
		CrystalPowerUI = GameObject.Find("PlayerStatus").transform.Find("CrystalPower").transform.Find("Panel");
		Player = GameObject.Find("Player");

		playerStatus = Player.GetComponent<PlayerStatus>();
		playerController = Player.GetComponent<PlayerController>();
		playerShotBullet = Player.GetComponent<PlayerShotBullet>();

		CrystalUI = transform.Find("CrystalUI").gameObject;
		CrystalUI.SetActive(false);
		CrystalUI.transform.Find("Price").gameObject.GetComponent<Text>().text = "Price : " + price;

		Particle = transform.Find("Particle").gameObject;
	}

	void Update()
	{
		if (Mathf.Approximately(Time.timeScale, 0f)) return;

		if (PlayerStatus.crystalPowerCount >= 3 && alreadyBuy == false)
		{
			alreadyBuy = true;
			CrystalUI.SetActive(false);
			Particle.GetComponent<ParticleSystem>().Play();
			InvokeRepeating("AlphaChange", 0, 0.1f);
		}

		if (alreadyBuy == false)
		{
			Vector2 distancePos = Player.transform.position - transform.position;
			float distace = Mathf.Sqrt(Mathf.Pow(distancePos.x, 2) + Mathf.Pow(distancePos.y, 2));

			inPlayer = distace <= 2.0f ? true : false;

			if (inPlayer == true)
			{
				PlayerStatus.useKeyViewBool = true;
				canBuy = playerStatus.GetMoneyValue() >= Artifact.ClacMoney(price) ? true : false;
				CrystalUI.transform.Find("Price").gameObject.GetComponent<Text>().text = "Price : " + Artifact.ClacMoney(price);

				CrystalUI.SetActive(true);

				if (canBuy == true && colorChange == true)
				{
					PlayerStatus.useKeyCanBool = false;
					colorChange = false;
					CrystalUI.transform.Find("BackGround").gameObject.GetComponent<Image>().color = new Color32(119, 255, 51, 255);
				}
				else if (canBuy == false && colorChange == false)
				{
					PlayerStatus.useKeyCanBool = true;
					colorChange = true;
					CrystalUI.transform.Find("BackGround").gameObject.GetComponent<Image>().color = new Color32(255, 51, 51, 255);
				}

				if (canBuy == true)
				{
					if (Input.GetKeyDown(KeyConfig.Use) || Input.GetButtonDown("C_Use"))
					{
						CrystalPowerBuy();
					}
				}
			}
			else
			{
				CrystalUI.SetActive(false);
			}
		}
	}

	void CrystalPowerBuy()
	{
		PlayerStatus.crystalPowerCount += 1;

		switch (id)
		{
			case 0:
				playerStatus.QuickCureSet();
				break;
			case 1:
				playerStatus.HealthAlpha();
				break;
			case 2:
				playerController.BoostDash();
				break;
			case 3:
				playerShotBullet.ExplosionBullet();
				break;
			case 4:
				playerShotBullet.SetFireAspect();
				break;
			case 5:
				playerStatus.SetLastStand();
				break;
			case 6:
				playerStatus.SetSoulEater();
				break;
			default:
				Debug.LogError("idが存在しません!!");
				return;
		}

		SoundDirector.PlaySE("UseCrystal");

		GameObject ins = Instantiate(HaveCrystalPower, CrystalPowerUI);
		ins.GetComponent<Image>().sprite = CrystalPowerImage[id];

		alreadyBuy = true;
		CrystalUI.SetActive(false);
		playerStatus.GetMoney(-price);
		Particle.GetComponent<ParticleSystem>().Play();
		InvokeRepeating("AlphaChange", 0,0.1f);
	}

	float colorAlpha = 1;

	void AlphaChange()
	{
		colorAlpha -= 0.01f;
		GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, colorAlpha);
		if (colorAlpha <= 0.25f)
		{
			Particle.GetComponent<ParticleSystem>().Stop();
		}
		if (colorAlpha <= 0)
		{
			DestoryObj();
		}
	}
	void DestoryObj()
	{
		CancelInvoke("AlphaChange");
		Destroy(gameObject);
	}
}
