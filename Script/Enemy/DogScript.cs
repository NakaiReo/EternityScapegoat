using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogScript : MonoBehaviour
{
	GameObject Player;
	Rigidbody2D rb;
	static float dashSpantime = 5;
	float dashCooltime = 0;

	private void Start()
	{
		Player = GameObject.Find("Player");
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
    {
		dashCooltime -= dashCooltime > 0 ? Time.deltaTime : 0;
		if (dashCooltime > 0) return;
		dashCooltime = dashSpantime;
		Vector2 playerPos = Player.transform.position;
		Vector2 wolfPos = transform.position;
		Vector2 Axis = playerPos - wolfPos;
		Axis = Axis.normalized;
		rb.AddForce(Axis * 0.5f);
	}
}
