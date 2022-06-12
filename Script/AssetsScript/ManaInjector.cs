using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaInjector : MonoBehaviour {

	public static EnchantSheet enchantSheet;
	public static int EnchantAmount = 10;

	/*--------------------------------------------------
	 * 0 : BulletImpact 1-5   (10 * n %)のダメージ増加
	 * 1 : Acceleration 1-3 (n)の弾速増加
	 * 2 : QuickFire 1-5 (5 * n %)の射撃速度増加 
	 * 3 : Luck 1-5 (2 * n %)のクリティカル増加
	 * 4 : FastReload 1-3 (10 * n %)のリロード時間短縮
	 * 5 : ExtendMagazine 1-2 (25 * n %)装填弾数増加
	 * 6 : AmmoBag 1-2 (15 * n %)所持弾薬増加
	 * 7 : Precision 1-1 ブレがなくなる
 	 * 8 : Shiny 1-1 宝石がたまに二つ落ちる
 	 * 9 : Discovery 1-1 アイテムのドロップ確率が増加
	 * ------------------------------------------------*/

	PlayerShotBullet playerShotBullet;

	GameObject Player;

	PlayerStatus playerStatus;

	Sprite[] GunsSprite;

	GameObject ManaInjectorUI;

	GameObject GunImage;
	ParticleSystem Particle1;
	ParticleSystem Particle2;

	int price = 3000;

	bool inPlayer = false;
	bool canBuy = false;
	bool colorChange = false;

	bool alreadyBuy = false;

	bool CanPick = false;
	int gunID;
	float deleteTime;
	string GunName;

	void Start () {
		enchantSheet = Resources.Load("Enchant") as EnchantSheet;

		Player = GameObject.Find("Player");
		playerShotBullet = Player.GetComponent<PlayerShotBullet>();

		playerStatus = Player.GetComponent<PlayerStatus>();

		GunsSprite = Resources.LoadAll<Sprite>("Guns");

		ManaInjectorUI = transform.Find("ManaInjectorUI").gameObject;
		ManaInjectorUI.SetActive(false);
		ManaInjectorUI.transform.Find("Price").gameObject.GetComponent<Text>().text = "Price : " + price;

		GunImage = transform.Find("GunImage").gameObject;
		GunImage.SetActive(false);

		Particle1 = transform.Find("Particle1").gameObject.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {

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
				ManaInjectorUI.transform.Find("Price").gameObject.GetComponent<Text>().text = "Price : " + Artifact.ClacMoney(price);

				ManaInjectorUI.SetActive(true);

				if (canBuy == true && colorChange == true)
				{
					PlayerStatus.useKeyCanBool = false;
					colorChange = false;
					ManaInjectorUI.transform.Find("BackGround").gameObject.GetComponent<Image>().color = new Color32(119, 255, 51, 255);
				}
				else if (canBuy == false && colorChange == false)
				{
					PlayerStatus.useKeyCanBool = true;
					colorChange = true;
					ManaInjectorUI.transform.Find("BackGround").gameObject.GetComponent<Image>().color = new Color32(255, 51, 51, 255);
				}

				if (canBuy == true)
				{
					if (Input.GetKeyDown(KeyConfig.Use) || Input.GetButtonDown("C_Use"))
					{
						BuyManaInjector();
					}
				}
			}
			else
			{
				ManaInjectorUI.SetActive(false);
			}
		}
		else if (CanPick == true)
		{
			if (inPlayer == true)
			{
				if (Input.GetKeyDown(KeyConfig.Use) || Input.GetButtonDown("C_Use"))
				{
					playerShotBullet.TradeWeapon(gunID);
				}
			}
		}
	}

	void BuyManaInjector()
	{
		playerStatus.GetMoney(-price);
		ManaInjectorUI.SetActive(false);
		GunImage.GetComponent<SpriteRenderer>().sprite = GunsSprite[playerShotBullet.GetGunID()];
		if (playerShotBullet.GetGunID() != 0)
			playerShotBullet.Enchant();
		else
			playerShotBullet.TradeWeapon(35);
		Particle1.Play();
		SoundDirector.PlaySE("GunsGenerator2");
	}


	public static int[] EnchantEffect()
	{
		int[] enchantEffect = new int[EnchantAmount];

		for (int id = 0; id < EnchantAmount; id++)
		{
			int enchantPercent = Random.Range(0, 100);
			int level;
			for (level = 1; level <= enchantSheet.sheets[0].list[id].EffectLength; level++)
			{
				if(enchantPercent < EnchantPercentGet(id, level))
				{
					break;
				}
			}
			enchantEffect[id] = level - 1;
		}

		return enchantEffect;
	}

	public static int EnchantPercentGet(int id, int level)
	{
		switch (level)
		{
			case 1:
				return enchantSheet.sheets[0].list[id].P1;
			case 2:
				return enchantSheet.sheets[0].list[id].P2;
			case 3:
				return enchantSheet.sheets[0].list[id].P3;
			case 4:
				return enchantSheet.sheets[0].list[id].P4;
			case 5:
				return enchantSheet.sheets[0].list[id].P5;
		}
		return -1;
	}

	/// <summary>
	/// 0 : BulletImpact 1-5   (5 * n %)のダメージ増加
	/// 1 : Acceleration 1-3 (n) の弾速増加
	/// 2 : QuickFire 1-5 (5 * n %)の射撃速度増加
	/// 3 : Luck 1-5 (2 * n %)のクリティカル増加
	/// 4 : FastReload 1-3 (10 * n %)のリロード時間短縮
	/// 5 : ExtendMagazine 1-2 (25 * n %)装填弾数増加
	/// 6 : AmmoBag 1-2 (15 * n %)所持弾薬増加
	/// 7 : Precision 1-1 ブレがなくなる
	/// 8 : Shiny 1-1 宝石がたまに二つ落ちる
	/// 9 : Discovery 1-1 アイテムのドロップ確率が増加
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public static float GetEffectAmount(int id)
	{
		return enchantSheet.sheets[0].list[id].EffectAmount;
	}
}
