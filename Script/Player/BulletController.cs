using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

	Rigidbody2D rb;

	int id;
	float explosionPar;

	public struct BulletStatus
	{
		public Vector2 vector;
		public float speed;
		public float damage;
		public int penetration;
		public float critical;
		public float explosionPar;
	}
	public BulletStatus bulletStatus = new BulletStatus();

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		id = -1;
	}

	void Update()
	{
		if (Mathf.Approximately(Time.timeScale, 0f)) return;

		rb.velocity = bulletStatus.vector * bulletStatus.speed;
	}

	public void ReturnBulletStatus(Vector2 vector, float speed, float damage, int penetration, float critical, float explosionPar)
	{
		bulletStatus.vector = vector;
		bulletStatus.speed = speed + PlayerShotBullet.GetEffectAmount(1);
		bulletStatus.damage = damage + damage * PlayerShotBullet.GetEffectAmount(0) / 100f;
		bulletStatus.penetration = penetration;
		bulletStatus.critical = critical + critical * PlayerShotBullet.GetEffectAmount(3) / 100f;
		bulletStatus.explosionPar = explosionPar;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Block")
		{
			Destroy(gameObject);
		}
		if(collision.gameObject.tag == "Enemy")
		{
			if (id != collision.GetComponent<EnemyStatus>().GetID())
			{
				if (id == -1) GameDirector.hitCount +=1;
				id = collision.GetComponent<EnemyStatus>().GetID();
				collision.GetComponent<EnemyStatus>().EnemyDamage(gameObject);
				bulletStatus.penetration -= 1;
				if (bulletStatus.penetration < 0)
				{
					Destroy(gameObject);
				}
			}
		}
	}
}