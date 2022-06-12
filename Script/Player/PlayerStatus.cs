using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour {

	int hp;
	int maxHp;

	public static int crystalPowerCount;

	float invincibleTime = 0;
	float regenerationStartCoolTime = 0;
	float regenerationHealCoolTime = 0;
	float regenerationStartSetTime = 5;
	float regenerationHealSetTime = 3;

	int money;

	public static bool Die = false;
	public static bool LastStand = false;
	public static bool SoulEater = false;

	[SerializeField]
	bool Tutorial = false;
	[SerializeField]
	GameObject PlayerStatusCanvas;
	[SerializeField]
	GameObject GameOverPanel;
	[SerializeField]
	ParticleSystem Blood;

	public static bool useKeyCanBool = false;
	public static bool useKeyViewBool = false;
	[SerializeField] GameObject useKeyViewObject;
	Text useKeyViewText;

	//HPバー用
	Slider HPUI;
	Text HPText;

	//Money用
	Text MoneyText;

	//Stamina用
	Slider StaminaUI;

	private void Start()
	{
		HPUI = PlayerStatusCanvas.transform.Find("HP").transform.Find("HPUI").gameObject.GetComponent<Slider>();
		HPUI.value = 1f;
		HPText = PlayerStatusCanvas.transform.Find("HP").transform.Find("HPText").gameObject.GetComponent<Text>();

		StaminaUI = PlayerStatusCanvas.transform.Find("Stamina").transform.Find("StaminaUI").gameObject.GetComponent<Slider>();
		StaminaUI.value = 1f;

		MoneyText = PlayerStatusCanvas.transform.Find("Money").transform.Find("MoneyUI").transform.Find("MoneyText").gameObject.GetComponent<Text>();
		money = 500;
		ChangeMoneyText(money);

		PlayerStatusCanvas.SetActive(true);
		GameOverPanel.SetActive(false);

		invincibleTime = 0;
		regenerationStartCoolTime = 0;
		regenerationHealCoolTime = 0;
		regenerationStartSetTime = 5;
		regenerationHealSetTime = 3;
		LastStand = false;
		SoulEater = false;

		crystalPowerCount = 0;

		hp = 50;
		maxHp = hp;
		Die = false;
		ChangeHPText(hp, maxHp);

		useKeyViewText = useKeyViewObject.GetComponent<Text>();
	}

	private void Update()
	{
		if (Mathf.Approximately(Time.timeScale, 0f)) return;
		if (Input.GetKey(KeyCode.RightShift) && Input.GetKey(KeyCode.LeftAlt))
		{
			money = 9999999;
			ChangeMoneyText(money);
		}

		invincibleTime = invincibleTime > 0 ? invincibleTime - Time.deltaTime : 0;

		if (hp > 0 && hp < maxHp)
		{
			regenerationStartCoolTime = regenerationStartCoolTime > 0 ? regenerationStartCoolTime - Time.deltaTime : 0;
			if (regenerationStartCoolTime <= 0)
			{
				regenerationHealCoolTime = regenerationHealCoolTime > 0 ? regenerationHealCoolTime - Time.deltaTime : 0;
				if(regenerationHealCoolTime <= 0)
				{
					regenerationHealCoolTime = regenerationHealSetTime - Artifact.GetArtifactAmount(8);
					hp = hp < maxHp ? hp + 1 : maxHp;
					ChangeHPText(hp, maxHp);
				}
			}
		}

		useKeyViewObject.SetActive(useKeyViewBool);

		if(useKeyCanBool == false)
		{
			useKeyViewText.text = "Press the <E> Key";
			useKeyViewText.color = new Color32(119, 255, 51, 255);
		}
		else
		{
			useKeyViewText.text = "Not enough money";
			useKeyViewText.color = new Color32(255, 51, 51, 255);
		}

		if (useKeyViewBool == true)
		{
			useKeyViewBool = false;
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Enemy")
		{
			if (Tutorial == true) return;
			if (invincibleTime <= 0f && Die == false)
			{
				Damage(collision.gameObject.GetComponent<EnemyStatus>().GetDamage());
			}
		}
	}

	public void Damage(int damage)
	{
		if (invincibleTime <= 0f && Die == false)
		{
			invincibleTime = 1f + Artifact.GetArtifactAmount(9);
			regenerationStartCoolTime = regenerationStartSetTime;
			regenerationHealCoolTime = 0;
			hp -= damage;
			ChangeHPText(hp, maxHp);
			if (hp <= 0)
			{
				DieScript();
				return;
			}
			SoundDirector.PlaySE("PlayerDamage");
			//GetComponent<SpriteRenderer>().color = new Color32(170, 0, 0, 255);
			GetComponent<PlayerSkinChange>().SpriteColor(new Color32(170, 0, 0, 255));
			Invoke("Flashing", 0.1f);
		}
	}

	void DieScript()
	{
		if (LastStand == true)
		{
			LastStand = false;
			hp = 10;
			invincibleTime = 5;
			ChangeHPText(hp,maxHp);
			Transform crystalPowerUI = PlayerStatusCanvas.transform.Find("CrystalPower").transform.Find("Panel");
			for(int i = 0; i < crystalPowerUI.childCount; i++)
			{
				if(crystalPowerUI.transform.GetChild(i).GetComponent<Image>().sprite.name == "CrystalPower_5")
				{
					crystalPowerUI.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 100);
				}
			}
			return;
		}
		Blood.Play();
		//GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<PlayerSkinChange>().SpriteRendererEnabled(false);
		GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
		GameOver.Gameover();
		SoundDirector.PlaySE("PlayerDown");
		Die = true;
	}

	void Flashing()
	{
		//GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
		GetComponent<PlayerSkinChange>().SpriteColor(new Color32(255, 255, 255, 255));
	}

	void ChangeHPText(int hp,int maxHp)
	{
		HPText.text = hp + " / " + maxHp;
		HPUI.value = hp / (float)maxHp;
	}

	void ChangeMoneyText(int money)
	{
		MoneyText.text = money + " G";
	}

	public void StaminaBarChange(float Stamina)
	{
		StaminaUI.value = Stamina / 1.0f;
	}

	public void QuickCureSet()
	{
		regenerationHealSetTime = regenerationHealSetTime * 4.0f / 10.0f;
	}

	public void HealthAlpha()
	{
		maxHp = (int)(maxHp * 3.0f / 2.0f);
		hp = maxHp;
		ChangeHPText(hp, maxHp);
	}

	public void GetMoney(int amount)
	{
		money += amount;
		ChangeMoneyText(money);
		if(amount > 0) SoundDirector.PlaySE("GetMoney");
	}

	public void SetMoney(int amount)
	{
		money = amount;
		ChangeMoneyText(money);
	}

	public int GetMoneyValue()
	{
		return money;
	}

	public void SetLastStand()
	{
		LastStand = true;
	}

	public void SetSoulEater()
	{
		SoulEater = true;
	}

	public void Heal(int amount)
	{
		hp += amount;
		if (hp > maxHp) hp = maxHp;
		ChangeHPText(hp, maxHp);
	}
}
