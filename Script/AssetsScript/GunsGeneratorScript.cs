using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunsGeneratorScript : MonoBehaviour {

	PlayerShotBullet playerShotBullet;

	GameObject Player;
	Vector2 StartPos;

	PlayerStatus playerStatus;

	Sprite[] KnifeSprite;
	Sprite[] GunsSprite;

	GameObject GunsGeneratorUI;
	GameObject GunsImageUI;
	Text GunsText;
	GameObject GunSprite;
	ParticleSystem Particle1;
	ParticleSystem Particle2;

	int price = 2000;

	bool inPlayer = false;
	bool canBuy = false;
	bool colorChange = false;

	bool alreadyBuy = false;

	void Start () {
		Player = GameObject.Find("Player");
		playerShotBullet = Player.GetComponent<PlayerShotBullet>();

		playerStatus = Player.GetComponent<PlayerStatus>();

		KnifeSprite = Resources.LoadAll<Sprite>("KnifeImage");
		GunsSprite = Resources.LoadAll<Sprite>("Guns");

		GunsGeneratorUI = transform.Find("GunsGeneratorUI").gameObject;
		GunsGeneratorUI.SetActive(false);
		GunsGeneratorUI.transform.Find("Price").gameObject.GetComponent<Text>().text = "Price : " + price;

		GunSprite = transform.Find("GunImage").gameObject;
		GunSprite.SetActive(false);

		GunsImageUI = transform.Find("GunsImageUI").gameObject;
		GunsText = GunsImageUI.transform.Find("GunName").gameObject.GetComponent<Text>();
		GunsImageUI.SetActive(false);

		StartPos = GunSprite.transform.position;

		Particle1 = transform.Find("Particle1").gameObject.GetComponent<ParticleSystem>();
		Particle2 = transform.Find("Particle2").gameObject.GetComponent<ParticleSystem>();
	}
	
	void Update () {
		if (Mathf.Approximately(Time.timeScale, 0f)) return;

		deleteTime = deleteTime > 0 ? deleteTime - Time.deltaTime : 0;

		Vector2 distancePos = Player.transform.position - transform.position;
		float distace = Mathf.Sqrt(Mathf.Pow(distancePos.x, 2) + Mathf.Pow(distancePos.y, 2));

		inPlayer = distace <= 2.0f ? true : false;

		if (alreadyBuy == false)
		{
			if (inPlayer == true)
			{
				PlayerStatus.useKeyViewBool = true;
				canBuy = playerStatus.GetMoneyValue() >= Artifact.ClacMoney(price) ? true : false;
				GunsGeneratorUI.transform.Find("Price").gameObject.GetComponent<Text>().text = "Price : " + Artifact.ClacMoney(price);

				GunsGeneratorUI.SetActive(true);

				if (canBuy == true && colorChange == true)
				{
					PlayerStatus.useKeyCanBool = false;
					colorChange = false;
					GunsGeneratorUI.transform.Find("BackGround").gameObject.GetComponent<Image>().color = new Color32(119, 255, 51, 255);
				}
				else if (canBuy == false && colorChange == false)
				{
					PlayerStatus.useKeyCanBool = true;
					colorChange = true;
					GunsGeneratorUI.transform.Find("BackGround").gameObject.GetComponent<Image>().color = new Color32(255, 51, 51, 255);
				}

				if (canBuy == true)
				{
					if (Input.GetKeyDown(KeyConfig.Use) || Input.GetButtonDown("C_Use"))
					{
						BuyGunsGenerator();
					}
				}
			}
			else
			{
				GunsGeneratorUI.SetActive(false);
			}
		}
		else if(CanPick == true)
		{
			if (inPlayer==true)
			{
				GunsImageUI.SetActive(true);

				if (Input.GetKeyDown(KeyConfig.Use) || Input.GetButtonDown("C_Use"))
				{
					playerShotBullet.TradeWeapon(gunID);
					ResetGunsGenerator();
				}
			}
			else
			{
				GunsImageUI.SetActive(false);
			}
		}
	}

	bool CanPick = false;
	int gunID;
	float deleteTime;
	string GunName;

	void BuyGunsGenerator()
	{
		alreadyBuy = true;
		playerStatus.GetMoney(-price);
		GunSprite.transform.position = StartPos;
		GunsGeneratorUI.SetActive(false);
		GunSprite.SetActive(true);
		InvokeRepeating("ChangeGunImage", 0, 0.15f);
		Particle1.Play();
		Invoke("FinGunsGenerator", 7.5f);
	}

	void FinGunsGenerator()
	{
		gunID = Random.Range(-6, 34);
		GunSprite.GetComponent<SpriteRenderer>().sprite = SpriteChange(gunID);
		GunsImageUI.SetActive(true);
		GunName =  playerShotBullet.ReturnGunName(gunID);
		CancelInvoke("ChangeGunImage");
		Particle1.Stop();
		Particle1.Clear();
		Particle2.Play();
		deleteTime = 10;
		CanPick = true;
		SoundDirector.PlaySE("GunsGenerator2");
		InvokeRepeating("PickGunsGenerator", 0, 0.1f);
	}

	void PickGunsGenerator()
	{
		GunsText.text = GunName + " (" + (int)deleteTime + ")";

		if (deleteTime <= 0)
			ResetGunsGenerator();

		if (GunSprite.transform.position.y - StartPos.y > -1.75f)
			GunSprite.transform.Translate(0, -5.0f * Time.deltaTime, 0);
		else if (GunSprite.transform.position.y - StartPos.y < -1.75f)
			GunSprite.transform.position = new Vector3(StartPos.x, - 1.75f + StartPos.y, 0);
	}

	void ResetGunsGenerator()
	{
		alreadyBuy = false;
		GunSprite.SetActive(false);
		GunsGeneratorUI.SetActive(true);
		GunsImageUI.SetActive(false);
		CancelInvoke("PickGunsGenerator");
		CanPick = false;
	}

	void ChangeGunImage()
	{
		SoundDirector.PlaySE("GunsGenerator");
		GunSprite.GetComponent<SpriteRenderer>().sprite = SpriteChange(Random.Range(-6, GunsSprite.Length - 1));
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
