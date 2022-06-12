using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactVending : MonoBehaviour
{
	Artifact artifact;

	GameObject Player;
	Vector2 StartPos;

	PlayerStatus playerStatus;

	Sprite[] ArtifactSprites;

	GameObject ArtifactVendingrUI;
	GameObject ArtifactImageUI;
	Text ArtifactNameText;
	Text ArtifactLoreText;
	GameObject ArtifactSprite;
	ParticleSystem Particle1;
	ParticleSystem Particle2;

	int price = 5000;

	bool inPlayer = false;
	bool canBuy = false;
	bool colorChange = false;

	bool alreadyBuy = false;

	void Start()
    {
		Player = GameObject.Find("Player");
		artifact = Player.GetComponent<Artifact>();

		playerStatus = Player.GetComponent<PlayerStatus>();

		ArtifactSprites = Resources.LoadAll<Sprite>("Artifact");

		ArtifactVendingrUI = transform.Find("ArtifactVendingUI").gameObject;
		ArtifactVendingrUI.SetActive(false);
		ArtifactVendingrUI.transform.Find("Price").gameObject.GetComponent<Text>().text = "Price : " + price;

		ArtifactSprite = transform.Find("ArtifactImage").gameObject;
		ArtifactSprite.SetActive(false);

		ArtifactImageUI = transform.Find("ArtifactImageUI").gameObject;
		ArtifactNameText = ArtifactImageUI.transform.Find("ArtifactName").gameObject.GetComponent<Text>();
		ArtifactLoreText = ArtifactImageUI.transform.Find("ArtifactLore").gameObject.GetComponent<Text>();
		ArtifactImageUI.SetActive(false);

		StartPos = ArtifactSprite.transform.position;

		Particle1 = transform.Find("Particle1").gameObject.GetComponent<ParticleSystem>();
		Particle2 = transform.Find("Particle2").gameObject.GetComponent<ParticleSystem>();
	}

    void Update()
    {
		if (Mathf.Approximately(Time.timeScale, 0f)) return;

		deleteTime = deleteTime > 0 ? deleteTime - Time.deltaTime : 0;

		Vector2 distancePos = Player.transform.position - transform.position;
		float distace = Mathf.Sqrt(Mathf.Pow(distancePos.x, 2) + Mathf.Pow(distancePos.y, 2));

		inPlayer = distace <= 2.5f ? true : false;

		if (alreadyBuy == false)
		{
			if (inPlayer == true)
			{
				PlayerStatus.useKeyViewBool = true;
				canBuy = playerStatus.GetMoneyValue() >= Artifact.ClacMoney(price) ? true : false;
				ArtifactVendingrUI.transform.Find("Price").gameObject.GetComponent<Text>().text = "Price : " + Artifact.ClacMoney(price);

				ArtifactVendingrUI.SetActive(true);

				if (canBuy == true && colorChange == true)
				{
					PlayerStatus.useKeyCanBool = false;
					colorChange = false;
					ArtifactVendingrUI.transform.Find("BackGround").gameObject.GetComponent<Image>().color = new Color32(119, 255, 51, 155);
				}
				else if (canBuy == false && colorChange == false)
				{
					PlayerStatus.useKeyCanBool = true;
					colorChange = true;
					ArtifactVendingrUI.transform.Find("BackGround").gameObject.GetComponent<Image>().color = new Color32(255, 51, 51, 155);
				}

				if (canBuy == true)
				{
					if (Input.GetKeyDown(KeyConfig.Use) || Input.GetButtonDown("C_Use"))
					{
						BuyArtifactVending();
					}
				}
			}
			else
			{
				ArtifactVendingrUI.SetActive(false);
			}
		}
		else if (CanPick == true)
		{
			if (inPlayer == true)
			{
				ArtifactImageUI.SetActive(true);

				if (Input.GetKeyDown(KeyConfig.Use) || Input.GetButtonDown("C_Use"))
				{
					Artifact.SetArtifactData(artifactID);
					ResetArtifactVending();
				}
			}
			else
			{
				ArtifactImageUI.SetActive(false);
			}
		}
	}

	bool CanPick = false;
	int artifactID;
	float deleteTime;
	string ArtifactName;
	string ArtifactLore;

	void BuyArtifactVending()
	{
		alreadyBuy = true;
		playerStatus.GetMoney(-price);
		ArtifactSprite.transform.position = StartPos;
		ArtifactVendingrUI.SetActive(false);
		ArtifactSprite.SetActive(true);
		InvokeRepeating("ChangeArtifactImage", 0, 0.15f);
		Particle1.Play();
		Invoke("FinArtifactVending", 7.5f);
	}

	void FinArtifactVending()
	{
		artifactID = Random.Range(1, 12);
		ArtifactSprite.GetComponent<SpriteRenderer>().sprite = SpriteChange(artifactID);
		ArtifactImageUI.SetActive(true);
		ArtifactName = Artifact.GetArtifactName(artifactID);
		ArtifactLore = Artifact.GetArtifactLore(artifactID);
		CancelInvoke("ChangeArtifactImage");
		Particle1.Stop();
		Particle1.Clear();
		Particle2.Play();
		deleteTime = 15;
		CanPick = true;
		SoundDirector.PlaySE("GunsGenerator2");
		InvokeRepeating("PickArtifactVending", 0, 0.1f);
	}

	void PickArtifactVending()
	{
		ArtifactNameText.text = ArtifactName + " (" + (int)deleteTime + ")";
		ArtifactLoreText.text = ArtifactLore;

		if (deleteTime <= 0)
			ResetArtifactVending();

		if (ArtifactSprite.transform.position.y - StartPos.y > -2.25f)
			ArtifactSprite.transform.Translate(0, -8.5f * Time.deltaTime, 0);
		else if (ArtifactSprite.transform.position.y - StartPos.y < -2.25f)
			ArtifactSprite.transform.position = new Vector3(StartPos.x, -2.25f + StartPos.y, 0);
	}

	void ResetArtifactVending()
	{
		alreadyBuy = false;
		ArtifactSprite.SetActive(false);
		ArtifactVendingrUI.SetActive(true);
		ArtifactImageUI.SetActive(false);
		CancelInvoke("PickArtifactVending");
		CanPick = false;
	}

	void ChangeArtifactImage()
	{
		SoundDirector.PlaySE("GunsGenerator");
		ArtifactSprite.GetComponent<SpriteRenderer>().sprite = SpriteChange(Random.Range(1, 12));
	}

	Sprite SpriteChange(int id)
	{
		return ArtifactSprites[id];
	}
}
