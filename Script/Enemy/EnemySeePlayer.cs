using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeePlayer : MonoBehaviour {

	GameObject Player;
	Animator animator;

	void Start () {
		Player = GameObject.Find("Player");
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Approximately(Time.timeScale, 0f)) return;

		Vector2 wolfPos = transform.position;
		Vector2 playerPos = Player.transform.position;
		Vector2 Axis = playerPos - wolfPos;
		Axis = Axis.normalized;

		animator.SetFloat("SeeX", Axis.x);
		animator.SetFloat("SeeY", Axis.y);
	}
}
