using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : MonoBehaviour {

	GameObject Player;
	Transform EnemySpawnPosition;

	float damage;
	float range;

	void Start ()
	{
		Player = GameObject.Find("Player");
		EnemySpawnPosition = GameObject.Find("Enemy").transform;
		DamageArea();
	}

	void DamageArea()
	{
		for (int i = 0; i < EnemySpawnPosition.childCount; i++)
		{
			GameObject enemyObj = EnemySpawnPosition.transform.GetChild(i).gameObject;
			Vector2 distancePos = enemyObj.transform.position - Player.transform.position;
			float distace = Mathf.Sqrt(Mathf.Pow(distancePos.x, 2) + Mathf.Pow(distancePos.y, 2));
			if (distace <= range)
			{
				EnemyStatus enemyStatus = enemyObj.GetComponent<EnemyStatus>();
				enemyStatus.AddHP(-damage - damage * Artifact.GetArtifactAmount(6));
				enemyStatus.DieTest();
			}
		}
		Invoke("DestoyObj", 2);
	}

	public void SetStatus(float damage,float range)
	{
		this.damage = damage;
		this.range = range;
	}

	void DestoyObj()
	{
		Destroy(gameObject);
	}
}
