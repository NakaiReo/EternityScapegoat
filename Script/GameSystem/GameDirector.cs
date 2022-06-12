using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour {

	DiscordController discordController;

	Text WaveText;
	Text EnemyLeftText;

	bool waveStart;
	bool GameStart = false;

	int id = 0;

	static int wave;
	static int leftSpawnEnemy;
	static int leftEnemy;
	static int fieldMaxEnemy = 30;
	static float enemyHP;
	static float enemySpeed;
	static int wavePerAmountCount;
	static int WaveEnemyAmount;
	static float spawnDealy;

	public int[] EnemyType;

	const int MaxEnemyField = 30;

	/* ウェーブの初期設定 */
	int StartWave = 1; //初期Wave数
	int StartEnemyAmount = 6; //敵の初期数
	float StartEnemyHP = 30.0f; //敵初期体力
	float StartEnemySpeed = 1.5f; //敵初期スピード(2.0f)
	float StartSpawnDealy = 5.0f; //敵のスポーン間隔初期値

	/*--------------------*/


	/*  ウェーブの設定  */
	int wavePerAmount = 4; //ウェーブ毎の敵の増加量
	int plusPerWave = 10; // 値のWave毎に敵の増加量を増やす
	float enemyHPPerWave = 1.10f; //ウェーブ毎の敵の体力の増加倍率
	float enemySpeedWave = 0.05f; //ウェーブ毎の敵のスピード増加量
	float SpawnDealyWave = 0.15f; //ウェーブ毎の敵のスポーン間隔減少量
	/*------------------*/

	int startMoney;

	public static int shotCount = 0;
	public static int hitCount = 0;
	public static int killCount = 0;

	void Start () {
		waveStart = false;
		GameStart = false;

		discordController = GameObject.Find("DiscordDirector").GetComponent<DiscordController>();

		WaveText = GameObject.Find("PlayerStatus").transform.Find("Wave").transform.Find("WaveText").gameObject.GetComponent<Text>();
		EnemyLeftText = GameObject.Find("PlayerStatus").transform.Find("Wave").transform.Find("EnemyLeft").gameObject.GetComponent<Text>();

		GameMode.GamemodeChange(GameMode.GameModeSelect);
		setWaveData();
		GameObject.Find("Player").GetComponent<PlayerStatus>().SetMoney(startMoney);

		wave = 1;
		wavePerAmountCount = 0;
		WaveEnemyAmount = StartEnemyAmount;
		leftEnemy = WaveEnemyAmount;
		leftSpawnEnemy = leftEnemy;
		enemySpeed = StartEnemySpeed;
		enemyHP = StartEnemyHP;
		spawnDealy = StartSpawnDealy;
		SpawnArea.SetSpawnDelay(spawnDealy);
		EnemyStatus.SetDropPercent(5);

		shotCount = 0;
		hitCount = 0;
		killCount = 0;

		for (int i = 0; wave < StartWave; i++)
		{
			WaveNext();
		}

		discordController.View(wave);
		EnemyTypeAllocation();

		WaveTextQuickFade(false);

		//WaveText.color = new Color(1, 0, 0, 0);
		//WaveTextFade(true);
		EnemyLeftTextChange();

		Invoke("WaveStart", 5);
	}

	void WaveStart()
	{
		waveStart = true;
		GameStart = true;
		SoundDirector.PlaySE("WaveChange");
		WaveTextFade(true);
		//WaveTextQuickFade(true);
	}

	void WaveNext()
	{
		wave += 1;
		wavePerAmountCount += 1;
		if(wavePerAmountCount >= plusPerWave)
		{
			wavePerAmountCount = 0;
			wavePerAmount += 1;
		}
		WaveEnemyAmount += wavePerAmount;
		leftEnemy = WaveEnemyAmount;
		leftSpawnEnemy = leftEnemy;

		enemyHP *= enemyHPPerWave;
		enemyHP = (enemyHP * 100) / 100;

		enemySpeed += enemySpeedWave;
		spawnDealy -= SpawnDealyWave;
		SpawnArea.SetSpawnDelay(spawnDealy);
		EnemyStatus.RemoveDropPercent(0.15f);

		WaveText.text = "Wave " + wave;
		WaveTextFade(true);

		if (GameStart == true)
		{
			waveStart = true;
			SoundDirector.PlaySE("WaveChange");
		}

		discordController.View(wave);
		EnemyLeftTextChange();
		EnemyTypeAllocation();

		//Debug.Log(enemyHP + "HP : " + leftEnemy + "Enemy");
	}

	public void killEnemy()
	{
		leftEnemy -= 1;
		killCount += 1;
		EnemyLeftTextChange();

		if(leftEnemy <= 0 && waveStart == true)
		{
			waveStart = false;
			Debug.Log("Fin");
			WaveTextFade(false);
			Invoke("WaveNext", 7);
		}
	}

	bool inOut;
	float colorAlpha;
	void WaveTextFade(bool vs=true)
	{
		inOut = vs;
		colorAlpha = inOut == true ? 0 : 1;
		InvokeRepeating("WaveTextFadeMethod", 0,0.05f);
	}
	void WaveTextFadeMethod()
	{
		if (inOut == true)
		{
			colorAlpha += 0.025f;
			WaveText.color = new Color(1, 0, 0, colorAlpha);
			EnemyLeftText.color = new Color(1, 0, 0, colorAlpha);
			if (colorAlpha >= 1)
			{
				CancelInvoke("WaveTextFadeMethod");
			}
		}
		else
		{
			colorAlpha -= 0.025f;
			WaveText.color = new Color(1, 0, 0, colorAlpha);
			EnemyLeftText.color = new Color(1, 0, 0, colorAlpha);
			if (colorAlpha <= 0)
			{
				CancelInvoke("WaveTextFadeMethod");
			}
		}
	}

	void WaveTextQuickFade(bool fade)
	{
		if (fade == true)
		{
			WaveText.color = new Color(1, 0, 0, 1);
			EnemyLeftText.color = new Color(1, 0, 0, 1);
		}
		else
		{
			WaveText.color = new Color(1, 0, 0, 0);
			EnemyLeftText.color = new Color(1, 0, 0, 0);
		}
	}

	void EnemyLeftTextChange()
	{
		EnemyLeftText.text = leftEnemy + " Left";
	}

	void DestroyEnemy()
	{
		Transform EnemySpawnPosition = GameObject.Find("Enemy").transform;
		for (int i = 0; i < EnemySpawnPosition.childCount; i++)
		{
			Destroy(EnemySpawnPosition.transform.GetChild(i));
		}
	}

	void EnemyTypeAllocation()
	{
		EnemyType = new int[WaveEnemyAmount];

		int dogAmount = wave > 5 ? (int)(WaveEnemyAmount * 0.2) : 0;
		int ghostAmount = wave > 10 ? (int)(WaveEnemyAmount * 0.2) : 0;
		int slimeAmount = wave > 15 ? (int)(WaveEnemyAmount * 0.2) : 0;

		for (int i = 0; i < EnemyType.Length; i++)
		{
			EnemyType[i] = 0;
			if (i <= dogAmount - 1) EnemyType[i] = 1;
			else if (i <= dogAmount + ghostAmount - 1) EnemyType[i] = 2;
			else if (i <= dogAmount + ghostAmount + slimeAmount - 1) EnemyType[i] = 3;
		}

		var array = EnemyType.OrderBy(i => Guid.NewGuid()).ToArray();
		for(int i=0; i<EnemyType.Length; i++)
		{
			EnemyType[i] = array[i];
		}
	}

	/*------------------------------------------------*/
	//変数を返すメソッド
	public bool GetWaveStart()
	{
		return waveStart;
	}
	public float ReturnSpeedValue()
	{
		return enemySpeed;
	}
	public int GetLeftSpawnEnemy()
	{
		return leftSpawnEnemy;
	}
	public void LeftEnemySpawnChange(int amount)
	{
		leftSpawnEnemy += amount;
	}
	public float GetEnemyHP()
	{
		return enemyHP;
	}
	public int GetID()
	{
		return id;
	}
	public void AddID()
	{
		id++;
		if (id >= 100000)
		{
			id = 0;
		}
	}
	public static int GetWaveNow()
	{
		return wave;
	}
	public int GetMaxEnemyField()
	{
		return MaxEnemyField;
	}

	public void setWaveData()
	{
		StartWave = GameMode.StartWave; //初期Wave数
		StartEnemyAmount = GameMode.StartEnemyAmount; //敵の初期数
		StartEnemyHP = GameMode.StartEnemyHP; //敵初期体力
		StartEnemySpeed = GameMode.StartEnemySpeed; //敵初期スピード(2.0f)
		StartSpawnDealy = GameMode.StartSpawnDealy; //敵のスポーン間隔初期値

		/*--------------------*/


		/*  ウェーブの設定  */
		wavePerAmount = GameMode.wavePerAmount; //ウェーブ毎の敵の増加量
		plusPerWave = GameMode.plusPerWave; // 値のWave毎に敵の増加量を増やす
		enemyHPPerWave = GameMode.enemyHPPerWave; //ウェーブ毎の敵の体力の増加倍率
		enemySpeedWave = GameMode.enemySpeedWave; //ウェーブ毎の敵のスピード増加量
		SpawnDealyWave = GameMode.SpawnDealyWave; //ウェーブ毎の敵のスポーン間隔減少量
									  /*------------------*/

		startMoney = GameMode.startMoney;
	}
}
