using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class PlayerShotBullet : MonoBehaviour {
	
	static EnchantSheet enchantSheet;
	KnifeSheet knifeSheet;
	GunsDataSheet gunsDataSheet;
	Sprite[] GunsSprite;
	Sprite[] KnifeSprite;

	GameObject KnifeAnimation;
	float knifeTime;
	float knifeCoolTime = 1;
	
	float damagePlusTime = 0;

	[SerializeField]
	private GameObject bulletAmmoUI;
	[SerializeField]
	GameObject PlayerStatusCanvas;
	GameObject AmmoUI;
	Slider ReloadUI;
	Slider StatusBar;
	Image StatusBarColor;
	Text AmmoText;
	GameObject OutOfAmmoText;
	Image[] GunImage = new Image[2];
	GameObject[] Frame = new GameObject[2];
	Text[] GunName = new Text[2];
	Text[] AmmoLeft = new Text[2];

	Transform KnifePanel;
	Image CoolTimeImage;
	Image KnifeImage;
	Text KnifeName;

	Transform EnchantTransform;
	GameObject EnchantText;

	bool ExplosionBool = false;
	bool FireAspect = false;

	[SerializeField]
	private GameObject Bullet_0;

	Vector3 mousePosition;

	struct KnifeData
	{
		public string Name;
		public float Damage;
		public float ShotRate;
		public float Range;
	}
	struct BulletData
	{
		public string Name;
		public string Type;
		public float speed;
		public float damage;
		public int maxAmmo;
		public int magazineAmmo;
		public float shotRate;
		public float reloadSpeed;
		public int amount;
		public float bure;
		public int penetration;
		public float critical;
		public float explosionPar;
	}
	KnifeData knifeData = new KnifeData();
	BulletData bulletData = new BulletData();

	static bool reloadNow = false;
	static bool changeWeaponNow = false;
	int[] magazineAmmo = new int[2];
	int[] maxAmmo = new int[2];
	int[] ammo = new int[2];
	static int[,] enchant = new int[2,ManaInjector.EnchantAmount];
	float shotRateCooltime;
	float reloadTime;
	float changeWeapon;
	public static int haveGunID;

	public static int knifeID;
	public static int[] gunsID;
	bool AutoReload = true;

	void Awake () {
		haveGunID = 0;
		knifeID = 0;
		gunsID = new int[2] { 0, 1 };

		enchantSheet = Resources.Load("Enchant") as EnchantSheet;
		knifeSheet = Resources.Load("KnifeData") as KnifeSheet;
		gunsDataSheet = Resources.Load("GunsData") as GunsDataSheet;
		KnifeAnimation = Resources.Load("KnifeAnimation") as GameObject;
		GunsSprite = Resources.LoadAll<Sprite>("Guns");
		KnifeSprite = Resources.LoadAll<Sprite>("KnifeImage");

		AmmoUI = PlayerStatusCanvas.transform.Find("ReloadUI").transform.Find("Ammo").transform.Find("AmmoUI").gameObject;
		ReloadUI = PlayerStatusCanvas.transform.Find("ReloadUI").gameObject.GetComponent<Slider>();
		ReloadUI.value = 1;
		StatusBar = GameObject.Find("Player").transform.Find("PlayerUI").transform.Find("StatusBar").gameObject.GetComponent<Slider>();
		StatusBar.value = 0;
		StatusBarColor = GameObject.Find("Player").transform.Find("PlayerUI").transform.Find("StatusBar").transform.Find("Fill Area").transform.Find("Fill").gameObject.GetComponent<Image>();
		AmmoText = PlayerStatusCanvas.transform.Find("ReloadUI").transform.Find("Ammo").transform.Find("AmmoText").gameObject.GetComponent<Text>();
		OutOfAmmoText = PlayerStatusCanvas.transform.Find("ReloadUI").transform.Find("Ammo").transform.Find("OutOfAmmoText").gameObject;
		for(int i = 0; i < 2; i++)
		{
			GetGunsData(gunsID[i]);
			GunImage[i] = GameObject.Find("Guns").transform.GetChild(i).transform.Find("GunImage").gameObject.GetComponent<Image>();
			GunImage[i].sprite = GunsSprite[gunsID[i]];
			Frame[i] = GameObject.Find("Guns").transform.GetChild(i).transform.Find("Frame").gameObject;
			GunName[i] = GameObject.Find("Guns").transform.GetChild(i).transform.Find("GunName").gameObject.GetComponent<Text>();
			GunName[i].text = bulletData.Name;

			AmmoLeft[i] = GameObject.Find("Guns").transform.GetChild(i).transform.Find("AmmoLeft").gameObject.GetComponent<Text>();

			magazineAmmo[i] = bulletData.magazineAmmo;
			maxAmmo[i] = bulletData.maxAmmo;
			ammo[i] = maxAmmo[i];
			EnchantReset(i);
		}
		EnchantTransform = PlayerStatusCanvas.transform.Find("Enchant").transform.Find("Panel");
		EnchantText = Resources.Load("EnchantText") as GameObject;
		EnchantChange();

		GetGunsData(gunsID[haveGunID]);

		KnifePanel = PlayerStatusCanvas.transform.Find("Knife").transform.Find("Panel");
		CoolTimeImage = KnifePanel.transform.Find("CoolTimeImage").GetComponent<Image>();
		KnifeImage = KnifePanel.transform.Find("KnifeImage").GetComponent<Image>();
		KnifeName = KnifePanel.transform.Find("Name").GetComponent<Text>();
		GetKnifeData(knifeID);

		AmmoUISet(ammo[haveGunID]);
		ChangeAmmoText(ammo[haveGunID], maxAmmo[haveGunID], magazineAmmo[haveGunID]);
		shotRateCooltime = 0;
		reloadTime = 0;
		changeWeapon = 0;
		ExplosionBool = false;
	}
	private void Start()
	{
		Awake();
	}

	void Update () {
		if (Mathf.Approximately(Time.timeScale, 0f)) return;
		shotRateCooltime = shotRateCooltime > 0 ? shotRateCooltime - Time.deltaTime : 0;
		changeWeapon = changeWeapon > 0 ? changeWeapon - Time.deltaTime : 0;

		ChangeAmmoLeftText();

		if (PlayerStatus.Die == true) return;

		if (reloadNow == true)
		{
			reloadTime = reloadTime > 0 ? reloadTime - Time.deltaTime : 0;
			ReloadUI.value = reloadTime / (bulletData.reloadSpeed - bulletData.reloadSpeed * PlayerShotBullet.GetEffectAmount(4) / 100f);
			StatusBar.value = reloadTime / (bulletData.reloadSpeed - bulletData.reloadSpeed * PlayerShotBullet.GetEffectAmount(4) / 100f);
			if (reloadTime <= 0)
			{
				reloadNow = false;
				ReloadUI.value = 1;
				StatusBar.value = 0;
				magazineAmmo[haveGunID] += ammo[haveGunID];
				magazineAmmo[haveGunID] -= maxAmmo[haveGunID];
				ammo[haveGunID] = maxAmmo[haveGunID] + (magazineAmmo[haveGunID] < 0 ? magazineAmmo[haveGunID] : 0);
				magazineAmmo[haveGunID] = magazineAmmo[haveGunID] < 0 ? 0 : magazineAmmo[haveGunID];
				AmmoUISet(ammo[haveGunID]);
				ChangeAmmoText(ammo[haveGunID], maxAmmo[haveGunID], magazineAmmo[haveGunID]);
				OutOfAmmoText.SetActive(false);
				SoundDirector.PlaySE("ReloadFinish");
			}
		}

		if (Input.GetMouseButton(0) == true || Input.GetButton("C_Fire"))
		{
			if (shotRateCooltime <= 0 && reloadNow == false && changeWeaponNow == false)
			{
				if (ammo[haveGunID] > 0)
					ShotBullet();
				else
				{
					if(Input.GetMouseButtonDown(0) == true)
						SoundDirector.PlaySE("OutOfAmmo");
				}
			}
		}

		knifeTime -= knifeTime > 0 ? Time.deltaTime : 0;
		CoolTimeImage.fillAmount = knifeTime / knifeData.ShotRate;
		if ((Input.GetMouseButtonDown(1) == true || Input.GetButtonDown("C_Knife")) && knifeTime <= 0)
		{
			reloadNow = false;
			ReloadUI.value = 1;
			StatusBar.value = 0;
			
			knifeTime = knifeData.ShotRate;

			Vector2 Axis = AxisReturn() * knifeData.Range;
			GameObject ins  = Instantiate(KnifeAnimation, transform.position + (Vector3)Axis/2, Quaternion.identity, transform);
			ins.GetComponent<KnifeScript>().SetStatus(knifeData.Damage, knifeData.Range);
			ins.transform.localScale = new Vector3(1, 1, 1) * knifeData.Range;
		}

		if ((Input.GetKeyDown(KeyConfig.Reload) || Input.GetButtonDown("C_Reload") || (AutoReload == true && ammo[haveGunID]==0)) && ammo[haveGunID] != maxAmmo[haveGunID] && reloadNow == false && changeWeaponNow == false && magazineAmmo[haveGunID] > 0)
		{
			reloadNow = true;
			ReloadUI.value = 1;
			StatusBarColor.color = new Color32(0, 255, 0, 255);
			reloadTime = bulletData.reloadSpeed - bulletData.reloadSpeed * PlayerShotBullet.GetEffectAmount(4) / 100f;
			SoundDirector.PlaySE("ReloadStart");
		}

		if (Input.GetKeyDown(KeyConfig.WeaponChange) || Input.GetButtonDown("C_WeaponChange") || Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			haveGunID = haveGunID == 0 ? 1 : 0;
			changeWeapon = 1.0f;
			changeWeaponNow = true;
			StatusBarColor.color = new Color32(255, 165, 0, 255);

			Frame[0].SetActive(haveGunID == 0 ? true : false);
			Frame[1].SetActive(haveGunID == 0 ? false : true);

			SoundDirector.PlaySE("WeaponChange");

			GetGunsData(gunsID[haveGunID]);

			AmmoUISet(ammo[haveGunID]);

			EnchantChange();

			reloadNow = false;
			ReloadUI.value = 1;
		}
		if(changeWeaponNow == true)
		{
			StatusBar.value = changeWeapon / 1.0f;
			if (changeWeapon <= 0)
			{
				changeWeaponNow = false;
				StatusBar.value = 0;
			}
		}

		if(ammo[haveGunID] <= 0 && OutOfAmmoText)
		{
			OutOfAmmoText.SetActive(true);
		}
		else
		{
			OutOfAmmoText.SetActive(false);
		}
	}

	void ShotBullet()
	{
		shotRateCooltime = bulletData.shotRate - bulletData.shotRate * PlayerShotBullet.GetEffectAmount(2) / 100f;
		ammo[haveGunID] -= 1;
		AmmoUIRemove(1);
		ChangeAmmoText(ammo[haveGunID], maxAmmo[haveGunID], magazineAmmo[haveGunID]);
		Vector2 playerPos = transform.position;

		MousePositionGet();

		SoundDirector.PlaySE("BulletSound");
		GameDirector.shotCount += 1 * bulletData.amount;

		for (int i = 0; i < bulletData.amount; i++)
		{
			float bure = bulletData.bure;
			bure = PlayerShotBullet.GetEffectAmount(7) > 0 ? 0 : bure;

			Vector2 Axis2;
			float r = Mathf.Atan2(mousePosition.y - playerPos.y, mousePosition.x - playerPos.x) * Mathf.Rad2Deg;
			r += UnityEngine.Random.Range(-bure, bure);
			Axis2.x = Mathf.Cos(r * Mathf.Deg2Rad);
			Axis2.y = Mathf.Sin(r * Mathf.Deg2Rad);

			//Vector2 Axis = mousePosition - (Vector3)playerPos;
			//Axis = Axis.normalized;
			Vector2 bulletPos = playerPos + Axis2 * 0.5f ;

			GameObject InstantiateBullet = Instantiate(Bullet_0, bulletPos, Quaternion.identity) as GameObject;
			BulletController bulletController = InstantiateBullet.GetComponent<BulletController>();
			bulletController.ReturnBulletStatus(Axis2, bulletData.speed, bulletData.damage,bulletData.penetration, bulletData.critical, bulletData.explosionPar);
		}
	}

	public void GetGunsData(int id)
	{
		if (id < 0)
		{
			int id2 = (int)Math.Abs(id) - 1;
			GetKnifeData(id2);
			return;
		}
		bulletData.Name = gunsDataSheet.sheets[0].list[id].Name;
		bulletData.Type = gunsDataSheet.sheets[0].list[id].Type;
		bulletData.speed = gunsDataSheet.sheets[0].list[id].Speed;
		bulletData.damage = gunsDataSheet.sheets[0].list[id].Damage;
		bulletData.maxAmmo = gunsDataSheet.sheets[0].list[id].MaxAmmo;
		bulletData.maxAmmo += (int)(bulletData.maxAmmo * PlayerShotBullet.GetEffectAmount(5) / 100f);
		bulletData.magazineAmmo = gunsDataSheet.sheets[0].list[id].MagazineAmmo;
		bulletData.magazineAmmo += (int)(bulletData.magazineAmmo * PlayerShotBullet.GetEffectAmount(6) / 100f);
		bulletData.shotRate = gunsDataSheet.sheets[0].list[id].ShotRate;
		bulletData.reloadSpeed = gunsDataSheet.sheets[0].list[id].ReloadSpeed;
		bulletData.amount = gunsDataSheet.sheets[0].list[id].Amount;
		bulletData.bure = gunsDataSheet.sheets[0].list[id].Bure;
		bulletData.penetration = gunsDataSheet.sheets[0].list[id].Penetration;
		bulletData.critical = gunsDataSheet.sheets[0].list[id].Critical;
		bulletData.explosionPar = ExplosionBool == true ? 60.0f / bulletData.maxAmmo / bulletData.amount : 0;
	}
	public void GetKnifeData(int id)
	{
		knifeData.Name = knifeSheet.sheets[0].list[id].Name;
		knifeData.Damage = knifeSheet.sheets[0].list[id].Damage;
		knifeData.ShotRate = knifeSheet.sheets[0].list[id].ShotRate;
		knifeData.Range = knifeSheet.sheets[0].list[id].Range;

		KnifeImage.sprite = KnifeSprite[id];
		KnifeName.text = knifeData.Name;
	}

	void MousePositionGet()
	{
		//Vector3 ScreenWorldMousePosition = Input.mousePosition;
		//ScreenWorldMousePosition.z = 10f;
		//mousePosition = Camera.main.ScreenToWorldPoint(ScreenWorldMousePosition);
		mousePosition = TragetCursor.MousePosion;
	}

	Vector2 AxisReturn()
	{
		MousePositionGet();
		Vector2 Axis = mousePosition - transform.position;
		Axis = Axis.normalized;
		return Axis;
	}

	void AmmoUISet(int amount)
	{
		for(int i = 0; i < AmmoUI.transform.childCount; i++)
		{
			Destroy(AmmoUI.transform.GetChild(i).gameObject);
		}
		for(int i = 0; i < amount; i++)
		{
			Instantiate(bulletAmmoUI, transform.position, Quaternion.identity, AmmoUI.transform);
		}
		ChangeAmmoText(ammo[haveGunID], maxAmmo[haveGunID], magazineAmmo[haveGunID]);
	}

	void AmmoUIRemove(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			Destroy(AmmoUI.transform.GetChild(i).gameObject);
		}
	}

	void ChangeAmmoText(int ammo, int maxAmmo, int magazineAmmo)
	{
		AmmoText.text =  "[" + magazineAmmo + "]  " + ammo + " / " + maxAmmo;
	}

	void ChangeAmmoLeftText()
	{
		for(int i = 0; i < 2; i++)
		{
			AmmoLeft[i].text = ammo[i] + " / " + maxAmmo[i] + "  [" + magazineAmmo[i] + "]";
		}
	}

	public void TradeWeapon(int gunID)
	{
		if (gunID < 0)
		{
			int id2 = (int)Mathf.Abs(gunID) - 1;
			GetKnifeData(id2);
			return;
		}
		reloadNow = false;
		reloadTime = 0;
		ReloadUI.value = 1;
		StatusBar.value = 0;

		gunsID[haveGunID] = gunID;
		GetGunsData(gunsID[haveGunID]);

		Frame[0].SetActive(haveGunID == 0 ? true : false);
		Frame[1].SetActive(haveGunID == 0 ? false : true);

		magazineAmmo[haveGunID] = bulletData.magazineAmmo;
		maxAmmo[haveGunID] = bulletData.maxAmmo;
		ammo[haveGunID] = maxAmmo[haveGunID];
		AmmoUISet(ammo[haveGunID]);
		ChangeAmmoText(ammo[haveGunID], maxAmmo[haveGunID], magazineAmmo[haveGunID]);
		GunImage[haveGunID].sprite = GunsSprite[gunsID[haveGunID]];
		GunName[haveGunID].text = bulletData.Name;
		EnchantReset(haveGunID);
		EnchantChange();
		SoundDirector.PlaySE("WeaponChange");
	}

	public string ReturnGunName(int gunID)
	{
		if (gunID < 0)
		{
			int id2 = (int)Math.Abs(gunID) - 1;
			return knifeSheet.sheets[0].list[id2].Name;	
		}
		return gunsDataSheet.sheets[0].list[gunID].Name;
	}

	public void DamagePlus(float time)
	{
		damagePlusTime = time;
	}

	public void ExplosionBullet()
	{
		ExplosionBool = true;
		GetGunsData(gunsID[haveGunID]);
	}

	public void SetFireAspect()
	{
		FireAspect = true;
	}
	public bool GetFireAspect()
	{
		return FireAspect;
	}

	public int GetGunID()
	{
		Debug.Log(gunsID[haveGunID]);
		return gunsID[haveGunID];
	}

	public void MaxAmmo()
	{
		for(int i = 0; i < 2; i++)
		{
			ammo[i] = gunsDataSheet.sheets[0].list[gunsID[i]].MaxAmmo;
			ammo[i] += (int)(ammo[i] *PlayerShotBullet.GetEffectAmount(5) / 100f);
			maxAmmo[i] = ammo[i];
			magazineAmmo[i] = gunsDataSheet.sheets[0].list[gunsID[i]].MagazineAmmo;
			magazineAmmo[i] += (int)(magazineAmmo[i] * PlayerShotBullet.GetEffectAmount(6) / 100f);
		}
		AmmoUISet(ammo[haveGunID]);
		ChangeAmmoText(ammo[haveGunID], maxAmmo[haveGunID], magazineAmmo[haveGunID]);
		ChangeAmmoLeftText();

		reloadNow = false;
		reloadTime = 0;
		ReloadUI.value = 1;
		StatusBar.value = 0;
	}
	public void AmmoRefill()
	{
		int i = haveGunID;
		ammo[i] = gunsDataSheet.sheets[0].list[gunsID[i]].MaxAmmo;
		ammo[i] += (int)(ammo[i] * PlayerShotBullet.GetEffectAmount(5) / 100f);
		maxAmmo[i] = ammo[i];
		magazineAmmo[i] = gunsDataSheet.sheets[0].list[gunsID[i]].MagazineAmmo;
		magazineAmmo[i] += (int)(magazineAmmo[i] * PlayerShotBullet.GetEffectAmount(6) / 100f);
		AmmoUISet(ammo[haveGunID]);
		ChangeAmmoText(ammo[haveGunID], maxAmmo[haveGunID], magazineAmmo[haveGunID]);
		ChangeAmmoLeftText();

		reloadNow = false;
		reloadTime = 0;
		ReloadUI.value = 1;
		StatusBar.value = 0;
	}

	public void Enchant()
	{
		int[] enchantTemp = ManaInjector.EnchantEffect();
		for(int i = 0; i < enchantTemp.Length; i++)
		{
			enchant[haveGunID, i] = enchantTemp[i];
		}
		DebugEX.LogArray(enchantTemp);
		EnchantChange();
		GetGunsData(gunsID[haveGunID]);
		maxAmmo[haveGunID] = bulletData.maxAmmo;

		AmmoUISet(ammo[haveGunID]);
		ChangeAmmoText(ammo[haveGunID], maxAmmo[haveGunID], magazineAmmo[haveGunID]);
		ChangeAmmoLeftText();
	}

	void EnchantReset(int haveGunID)
	{
		for(int i = 0; i < ManaInjector.EnchantAmount; i++)
		{
			enchant[haveGunID, i] = 0;
		}
	}

	void EnchantChange()
	{
		for(int i = 0; i < EnchantTransform.childCount; i++)
		{
			Destroy(EnchantTransform.GetChild(i).gameObject);
		}
		for(int id=0;id< ManaInjector.EnchantAmount; id++)
		{
			if(enchant[haveGunID,id] != 0)
			{
				GameObject ins = Instantiate(EnchantText,EnchantTransform);
				ins.GetComponent<Text>().text = enchantSheet.sheets[0].list[id].Name + " " + NumberRome(enchant[haveGunID, id]) + " : " + enchantSheet.sheets[0].list[id].Lore;
			}
		}
	}

	/// <summary>
	/// 0 : BulletImpact
	/// 1 : Acceleration
	/// 2 : QuickFire
	/// 3 : Luck
	/// 4 : FastReload
	/// 5 : ExtendMagazine
	/// 6 : AmmoBag
	/// 7 : Precision
	/// 8 : Shiny
	/// 9 : Discovery
	/// </summary>
	public static float GetEffectAmount(int id)
	{
		return enchantSheet.sheets[0].list[id].EffectAmount * enchant[haveGunID, id];
	}

	string NumberRome(int n)
	{
		switch (n)
		{
			case 1:
				return "I";
			case 2:
				return "II";
			case 3:
				return "III";
			case 4:
				return "IV";
			case 5:
				return "V";
			case 6:
				return "VI";
			case 7:
				return "VII";
			case 8:
				return "VIII";
			case 9:
				return "IX";
			case 10:
				return "X";
			default:
				return "Null";
		}
	}
}
