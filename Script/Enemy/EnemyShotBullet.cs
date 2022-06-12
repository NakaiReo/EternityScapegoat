using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotBullet : MonoBehaviour {

	[SerializeField]
	GameObject EnemyBullet;

	GameObject Player;

	float shotTime = 0.0f;
	float shotDelay = 2.5f;

	float rnd = 20f;

	void Start () {
		Player = GameObject.Find("Player");
	}
	
	void Update () {
		shotTime += Time.deltaTime;
		if (shotTime >= shotDelay)
		{
			shotTime = 0;
			Vector2 PlayerPos = Player.transform.position;
			Vector2 EnemyPos = transform.position;
			Vector2 Axis;
			for (int i = 0; i < 3; i++)
			{
				float r = Mathf.Atan2(PlayerPos.y - EnemyPos.y, PlayerPos.x - EnemyPos.x) * Mathf.Rad2Deg;
				r += UnityEngine.Random.Range(-rnd, rnd);
				Axis.x = Mathf.Cos(r * Mathf.Deg2Rad);
				Axis.y = Mathf.Sin(r * Mathf.Deg2Rad);
				GameObject ins = Instantiate(EnemyBullet, transform.position, Quaternion.identity);
				ins.GetComponent<EnemyBulletController>().SetVector(Axis);
			}
		}
	}
}
