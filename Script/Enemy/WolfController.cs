using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfController : MonoBehaviour {

	GameObject Player;
	Rigidbody2D rb;
	Animator animator;

	bool moveWay = false;
	float speed = 2;

	void Start () {
		Player = GameObject.Find("Player");
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}
	
	void Update () {
		if (Mathf.Approximately(Time.timeScale, 0f)) return;

		Vector2 wolfPos = transform.position;
		Vector2 playerPos = Player.transform.position;
		Vector2 clacPos = playerPos - wolfPos;
		float d = Mathf.Sqrt(Mathf.Pow(clacPos.x, 2) + Mathf.Pow(clacPos.y, 2));
		Debug.Log(d);
		if (d > 3) { ChangeMoveWay(true); return; }
		ChangeMoveWay(false);

		Vector2 Axis = playerPos - wolfPos;
		Axis = Axis.normalized;

		rb.velocity = Axis * speed;

		//animator.SetFloat("SeeX", Axis.x);
		//animator.SetFloat("SeeY", Axis.y);
	}

	public void ChangeSpeed(float value)
	{
		speed = value;
	}

	public void ChangeMoveWay(bool b)
	{
		if (moveWay == b) return;
		moveWay = b;
		GetComponent<AstarAI>().enabled = moveWay;
	}
}
