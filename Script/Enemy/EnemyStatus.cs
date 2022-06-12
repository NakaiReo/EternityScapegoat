using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour {

	PlayerShotBullet playerShotBullet;

	GameObject GameDirector;
	GameDirector gameDirector;

	GameObject CriticalMes;
	GameObject ExplosionSysytemObj;
	GameObject FireImage;

	Transform EnemySpawnPosition;

	[SerializeField] GameObject MoneyDrop;
	[SerializeField] GameObject SoulDrop;
	[SerializeField] GameObject Item;

	static float itemDropPercent = 5;

	float[] hpMagnification = { 1.0f, 0.5f, 0.7f , 0.7f};
	float hp=100;
	float maxHp=100;

	int[] damage = { 10, 5, 10 ,10};

	int fireAmount = 0;
	float fireCoolTime = 0;

	int id;
	int type;

	GameObject HPUI;
	Slider hpBar;

	void Start () {
		playerShotBullet = GameObject.Find("Player").GetComponent<PlayerShotBullet>();
		GameDirector = GameObject.Find("GameDirector");
		gameDirector = GameDirector.GetComponent<GameDirector>();

		CriticalMes = Resources.Load("CriticalMes") as GameObject;
		ExplosionSysytemObj = Resources.Load("ExplosionSystem") as GameObject;
		FireImage = Resources.Load("FireImage") as GameObject;
		EnemySpawnPosition = GameObject.Find("Enemy").transform;

		if (gameDirector) hp = gameDirector.GetEnemyHP() * hpMagnification[type];
		else hp = 100;
		maxHp = hp;
		HPUI = transform.Find("HPUI").gameObject;
		hpBar = HPUI.transform.Find("HPBar").gameObject.GetComponent<Slider>();
		hpBar.value = 1f;
	}

	private void Update()
	{
		fireCoolTime -= fireCoolTime > 0 ? Time.deltaTime : 0;
		if (fireAmount > 0 && fireCoolTime <= 0)
		{
			fireCoolTime = 0.5f;
			fireAmount -= 1;
			AddHP(maxHp * -0.075f);
			Instantiate(FireImage, transform.position, Quaternion.identity,transform);
		}
		DieTest();
	}

	public void EnemyDamage(GameObject obj)
	{
		BulletController bulletController = obj.GetComponent<BulletController>();

		if (playerShotBullet.GetFireAspect())
		{
			float r = Random.Range(0.0f, 100f);
			if (r <= 5)
			{
				fireAmount = 15;
			}
		}

		float crit = 1.0f;
		float critPar = Random.Range(0.0f, 100);
		float explosionPar = Random.Range(0.0f, 100);
		if (critPar > bulletController.bulletStatus.critical && explosionPar > bulletController.bulletStatus.explosionPar)
		{
			SoundDirector.PlaySE("BulletHit");
		}
		if (critPar <= bulletController.bulletStatus.critical && explosionPar > bulletController.bulletStatus.explosionPar)
		{
			crit = 2.5f;
			Instantiate(CriticalMes, obj.transform.position, Quaternion.identity);
			SoundDirector.PlaySE("Critical");
		}
		if (explosionPar <= bulletController.bulletStatus.explosionPar)
		{
			Instantiate(ExplosionSysytemObj, obj.transform.position, Quaternion.identity);
			for (int i = 0; i < EnemySpawnPosition.childCount; i++)
			{
				GameObject enemyObj = EnemySpawnPosition.transform.GetChild(i).gameObject;
				Vector2 distancePos = enemyObj.transform.position - obj.transform.position;
				float distace = Mathf.Sqrt(Mathf.Pow(distancePos.x, 2) + Mathf.Pow(distancePos.y, 2));
				if (distace <= 2.5f)
				{
					EnemyStatus enemyStatus = enemyObj.GetComponent<EnemyStatus>();
					float damage = gameDirector.GetEnemyHP() * 0.2f * ItemUI.MagnificationDamagePlus();
					enemyStatus.hp -= damage;
				}
			}
			SoundDirector.PlaySE("Explosion");
			Destroy(obj);
		}

		hp -= bulletController.bulletStatus.damage * crit * ItemUI.MagnificationDamagePlus();
		GetComponent<SpriteRenderer>().color = new Color32(170, 0, 0, 255);
		Invoke("Flashing", 0.05f);
	}

	public void DieTest()
	{
		hpBar.value = hp / maxHp;
		if (hp <= 0)
		{
			float rand = Random.Range(0f, 100f);
			if (rand < itemDropPercent + PlayerShotBullet.GetEffectAmount(9) + Artifact.GetArtifactAmount(10))
				Instantiate(Item, transform.position, Quaternion.identity);

			if (PlayerStatus.SoulEater == true)
			{
				float rand2 = Random.Range(0f, 100f);
				if (rand2 < 35f)
					Instantiate(SoulDrop, transform.position - Vector3.up, Quaternion.identity);
			}

			float rand3 = Random.Range(0f, 100f);
			Instantiate(MoneyDrop, transform.position, Quaternion.identity);
			if(rand3 < PlayerShotBullet.GetEffectAmount(8))
				Instantiate(MoneyDrop, transform.position + Vector3.up, Quaternion.identity);

			if(gameDirector) gameDirector.killEnemy();
			Destroy(gameObject);
		}
	}

	void Flashing()
	{
		GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
	}

	public void SetStatus(float hp)
	{
		this.hp = hp;
		maxHp = hp;
	}

	public void SetID(int n)
	{
		id = n;
	}
	public int GetID()
	{
		return id;
	}
	public void setType(int id)
	{
		type = id;
	}
	public int GetDamage()
	{
		return damage[type];
	}
	public void AddHP(float amount)
	{
		hp += amount * ItemUI.MagnificationDamagePlus();
		if (amount < 0)
		{
			GetComponent<SpriteRenderer>().color = new Color32(170, 0, 0, 255);
			Invoke("Flashing", 0.05f);
		}
	}
	public void FireAspect(int amount)
	{
		fireAmount = amount;
	}

	public static void SetDropPercent(float f)
	{
		itemDropPercent = f;
	}

	public static void RemoveDropPercent(float f)
	{
		itemDropPercent -= f;
		if (itemDropPercent < 0.5f) itemDropPercent = 0.5f;
	}
}
