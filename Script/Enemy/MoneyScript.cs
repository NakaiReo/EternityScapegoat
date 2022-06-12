using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyScript : MonoBehaviour {

	GameObject Player;

	float stayTime = 0.5f;

	float speed = 0;
	float acceleration = 0.05f;
	float maxSpeed = 1.5f;

	byte[,] color = new byte[7, 3] {
		{ 255, 255, 255},
		{ 255, 0, 0},
		{ 0, 255, 0},
		{ 0, 0, 255},
		{ 255, 255, 0},
		{ 0, 255, 255},
		{ 255, 0, 255}
	};

	PlayerStatus playerStatus;
	[SerializeField] int moneyAmount;

	private void Start()
	{
		Player = GameObject.Find("Player");

		int colorPick = Random.Range(0, 7);
		GetComponent<SpriteRenderer>().color = new Color32(color[colorPick, 0], color[colorPick, 1], color[colorPick, 2], 200); 
		
		playerStatus = GameObject.Find("Player").GetComponent<PlayerStatus>();
	}

	private void Update()
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
			playerStatus.GetMoney(moneyAmount * ItemUI.MagnificationPointPlus());
			Destroy(gameObject);
		}
	}
}
