using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea : MonoBehaviour {

	Transform EnemySpawnPosition;

	GameObject GameDirector;
	GameDirector gameDirector;

	static GameObject Wolf;
	static GameObject Dog;
	static GameObject Ghost;
	static GameObject Slime;


	static float SpawnDelay = 7;

	bool playerIN = false;

	float time = 0;

	private void Start()
	{
		Wolf = Resources.Load("Enemy/Wolf") as GameObject;
		Dog = Resources.Load("Enemy/Dog") as GameObject;
		Ghost = Resources.Load("Enemy/Ghost") as GameObject;
		Slime = Resources.Load("Enemy/Slime") as GameObject;

		GameDirector = GameObject.Find("GameDirector");
		gameDirector = GameDirector.GetComponent<GameDirector>();
		EnemySpawnPosition = GameObject.Find("Enemy").transform;
	}

	void Update()
	{
		if (gameDirector.GetMaxEnemyField() <= EnemySpawnPosition.childCount) return;

		if (gameDirector.GetWaveStart() == false)
		{
			time = 0;
		}
		time += time <= SpawnDelay ? Time.deltaTime : 0;
		if (time >= SpawnDelay)
		{
			if (playerIN == true)
			{
				if (gameDirector.GetLeftSpawnEnemy() > 0)
				{
					time = 0;
					gameDirector.LeftEnemySpawnChange(-1);
					int enemyTypeID = gameDirector.EnemyType[gameDirector.GetLeftSpawnEnemy()];
					GameObject obj = null;
					switch (enemyTypeID)
					{
						case 0:
							obj = Instantiate(Wolf, transform.position, Quaternion.identity, EnemySpawnPosition);
							break;
						case 1:
							obj = Instantiate(Dog, transform.position, Quaternion.identity, EnemySpawnPosition);
							break;
						case 2:
							obj = Instantiate(Ghost, transform.position, Quaternion.identity, EnemySpawnPosition);
							break;
						case 3:
							obj = Instantiate(Slime, transform.position, Quaternion.identity, EnemySpawnPosition);
							break;
					}
					obj.GetComponent<EnemyStatus>().setType(enemyTypeID);
					obj.GetComponent<WolfController>().ChangeSpeed(gameDirector.ReturnSpeedValue() * (enemyTypeID == 1 ? 1.5f : 1f));
					obj.GetComponent<AstarAI>().maxSpeed = gameDirector.ReturnSpeedValue() * (enemyTypeID == 1 ? 1.5f : 1f);
					obj.GetComponent<EnemyStatus>().SetID(gameDirector.GetID());

					gameDirector.AddID();
				}
			}
		}
	}

	public static void SetSpawnDelay(float time)
	{
		SpawnDelay = time;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			playerIN = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			playerIN = false;
		}
	}
}
