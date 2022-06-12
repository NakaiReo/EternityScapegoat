using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class SetTarget : MonoBehaviour {

	GameObject Player;
	GameDirector gameDirector;

	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");
		gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
		GetComponent<AIDestinationSetter>().target = Player.transform;
		GetComponent<AILerp>().speed = gameDirector.ReturnSpeedValue();
	}
}
