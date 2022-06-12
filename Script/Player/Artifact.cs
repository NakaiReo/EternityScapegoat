using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Artifact : MonoBehaviour
{
	static ArtifactSheet artifactSheet;

	static PlayerController playerController;
	static PlayerStatus playerStatus;
	static PlayerShotBullet playerShotBullet;

	public struct ArtfactData
	{
		public int ID;
		public string Name;
		public string Lore;
		public bool Use;
		public float Amount;
		public float Cooltime;
	}
	public static ArtfactData artfactData = new ArtfactData();

	static Sprite[] ArtifactSprites;
	static GameObject ArtifactCanvas;
	static Image CoolTimeImage;
	static Image ArtifactSpite;
	static Text ArtifactName;

	public static int ArtifactID;
	static float cooltime;
		
    void Start()
    {
		GameObject Player = GameObject.Find("Player");
		playerController = Player.GetComponent<PlayerController>();
		playerStatus = Player.GetComponent<PlayerStatus>();
		playerShotBullet = Player.GetComponent<PlayerShotBullet>();

		artifactSheet = Resources.Load("ArtifactData") as ArtifactSheet;
		ArtifactSprites = Resources.LoadAll<Sprite>("Artifact");
		ArtifactCanvas = GameObject.Find("PlayerStatus").transform.Find("Artifact").transform.Find("Panel").gameObject;
		CoolTimeImage = ArtifactCanvas.transform.Find("CoolTimeImage").GetComponent<Image>();
		ArtifactSpite = ArtifactCanvas.transform.Find("ArtifactImage").GetComponent<Image>();
		ArtifactName = ArtifactCanvas.transform.Find("Name").GetComponent<Text>();

		ArtifactID = 0;
		cooltime = 0;

		SetArtifactData(ArtifactID);
    }

	private void Update()
	{
		cooltime -= cooltime > 0 ? Time.deltaTime : 0;
		CoolTimeImage.color = new Color32(255, 0, 0, 127);
		CoolTimeImage.fillAmount = cooltime / artifactSheet.sheets[0].list[ArtifactID].Cooltime;
		if (cooltime <= 0 && artfactData.Use)
		{
			CoolTimeImage.fillAmount = 1;
			CoolTimeImage.color = new Color32(0, 255, 0, 75);
		}
		if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("C_ItemUse"))
		{
			ItemUse(ArtifactID);
		}
	}

	public static void SetArtifactData(int id)
	{
		artfactData.ID = artifactSheet.sheets[0].list[id].ID;
		artfactData.Name = artifactSheet.sheets[0].list[id].Name;
		artfactData.Lore = artifactSheet.sheets[0].list[id].Lore;
		artfactData.Use  = artifactSheet.sheets[0].list[id].Use;
		artfactData.Amount = artifactSheet.sheets[0].list[id].Amount;
		artfactData.Cooltime = artifactSheet.sheets[0].list[id].Cooltime;

		ArtifactSpite.sprite = ArtifactSprites[id];
		ArtifactName.text = artfactData.Name;

		ArtifactID = id;
		cooltime = 0;
	}

	public static void ItemUse(int id)
	{
		if (artfactData.Use == false) return;
		if (cooltime > 0) return;
		cooltime = artfactData.Cooltime;
		switch (id)
		{
			case 1:
			case 2:
				playerStatus.Heal((int)artfactData.Amount);
				return;
			case 3:
				playerShotBullet.MaxAmmo();
				return;
			case 4:
				playerController.SetStaminaInfinity(10);
				return;
			case 5:
				ItemUI.SetDamagePlusTime(25);
				ItemUI.SetPointPlusTime(25);
				return;
			default:
				Debug.LogAssertion("ID範囲外です!!");
				return;
		}
	}

	public static float GetArtifactAmount(int id)
	{
		return ArtifactID == id ? artifactSheet.sheets[0].list[id].Amount : 0;
	}

	public static string GetArtifactName(int id)
	{
		return artifactSheet.sheets[0].list[id].Name;
	}
	public static string GetArtifactLore(int id)
	{
		return artifactSheet.sheets[0].list[id].Lore;
	}

	public static int ClacMoney(int money)
	{
		return (int)(money - money * GetArtifactAmount(11) * 0.01f);
	}
}
