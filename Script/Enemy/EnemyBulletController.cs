using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour {

	Rigidbody2D rb;

	Vector2 vector;
	float speed = 7f;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
		rb.velocity = vector * speed;
	}

	public void SetVector(Vector2 value)
	{
		vector = value;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Block")
		{
			Destroy(gameObject);
		}
		if (collision.gameObject.tag == "Player")
		{
			GameObject.Find("Player").GetComponent<PlayerStatus>().Damage(5);
			Destroy(gameObject);
		}
	}
}
