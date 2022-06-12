using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulScript : MonoBehaviour
{
	GameObject Player;

	float stayTime = 0.5f;

	float speed = 0;
	float acceleration = 0.05f;
	float maxSpeed = 1.5f;

	PlayerStatus playerStatus;

	void Start()
    {
		Player = GameObject.Find("Player");
		playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
	}

    // Update is called once per frame
    void Update()
    {
		if (Mathf.Approximately(Time.timeScale, 0f)) return;

		stayTime -= stayTime > 0 ? Time.deltaTime : 0;
		if (stayTime > 0) return;

		speed += acceleration * Time.deltaTime;
		speed = speed > maxSpeed ? maxSpeed : speed;

		Vector3 PlayerPos = Player.transform.position;
		Vector3 MoneyPos = transform.position;

		Vector3 Axis = PlayerPos - MoneyPos;
		Axis = Axis.normalized;

		transform.position += Axis * speed;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			playerStatus.Heal(1);
			Destroy(gameObject);
		}
	}
}
