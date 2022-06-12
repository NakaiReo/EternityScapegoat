using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameMode
{
	public enum Gamemode
	{
		Normal,
		Hard,
		Mayhem
	}
	public static Gamemode GameModeSelect = Gamemode.Normal;

	/* ウェーブの初期設定 */
	public static int StartWave = 1; //初期Wave数
	public static int StartEnemyAmount = 6; //敵の初期数
	public static float StartEnemyHP = 30.0f; //敵初期体力
	public static float StartEnemySpeed = 1.5f; //敵初期スピード(2.0f)
	public static float StartSpawnDealy = 5.0f; //敵のスポーン間隔初期値

	/*--------------------*/


	/*  ウェーブの設定  */
	public static int wavePerAmount = 4; //ウェーブ毎の敵の増加量
	public static int plusPerWave = 10; // 値のWave毎に敵の増加量を増やす
	public static float enemyHPPerWave = 1.10f; //ウェーブ毎の敵の体力の増加倍率
	public static float enemySpeedWave = 0.05f; //ウェーブ毎の敵のスピード増加量
	public static float SpawnDealyWave = 0.15f; //ウェーブ毎の敵のスポーン間隔減少量
												/*------------------*/

	public static int startMoney = 500;

	public static void GamemodeChange(Gamemode gm)
	{
		switch (gm)
		{
			case Gamemode.Normal:
				/* ウェーブの初期設定 */
				StartWave = 1; //初期Wave数
				StartEnemyAmount = 6; //敵の初期数
				StartEnemyHP = 30.0f; //敵初期体力
				StartEnemySpeed = 1.5f; //敵初期スピード(2.0f)
				StartSpawnDealy = 5.0f; //敵のスポーン間隔初期値

				/*  ウェーブの設定  */
				wavePerAmount = 4; //ウェーブ毎の敵の増加量
				plusPerWave = 10; // 値のWave毎に敵の増加量を増やす
				enemyHPPerWave = 1.10f; //ウェーブ毎の敵の体力の増加倍率
				enemySpeedWave = 0.04f; //ウェーブ毎の敵のスピード増加量
				SpawnDealyWave = 0.15f; //ウェーブ毎の敵のスポーン間隔減少量

				startMoney = 500;
				break;

			case Gamemode.Hard:
				/* ウェーブの初期設定 */
				StartWave = 1; //初期Wave数
				StartEnemyAmount = 6; //敵の初期数
				StartEnemyHP = 40.0f; //敵初期体力
				StartEnemySpeed = 1.8f; //敵初期スピード(2.0f)
				StartSpawnDealy = 4.0f; //敵のスポーン間隔初期値

				/*  ウェーブの設定  */
				wavePerAmount = 4; //ウェーブ毎の敵の増加量
				plusPerWave = 5; // 値のWave毎に敵の増加量を増やす
				enemyHPPerWave = 1.20f; //ウェーブ毎の敵の体力の増加倍率
				enemySpeedWave = 0.06f; //ウェーブ毎の敵のスピード増加量
				SpawnDealyWave = 0.15f; //ウェーブ毎の敵のスポーン間隔減少量

				startMoney = 500;
				break;

			case Gamemode.Mayhem:
				/* ウェーブの初期設定 */
				StartWave = 15; //初期Wave数
				StartEnemyAmount = 6; //敵の初期数
				StartEnemyHP = 30.0f; //敵初期体力
				StartEnemySpeed = 1.5f; //敵初期スピード(2.0f)
				StartSpawnDealy = 5.0f; //敵のスポーン間隔初期値

				/*  ウェーブの設定  */
				wavePerAmount = 4; //ウェーブ毎の敵の増加量
				plusPerWave = 10; // 値のWave毎に敵の増加量を増やす
				enemyHPPerWave = 1.10f; //ウェーブ毎の敵の体力の増加倍率
				enemySpeedWave = 0.04f; //ウェーブ毎の敵のスピード増加量
				SpawnDealyWave = 0.15f; //ウェーブ毎の敵のスポーン間隔減少量

				startMoney = 10000;
				break;
		}
	}
}
