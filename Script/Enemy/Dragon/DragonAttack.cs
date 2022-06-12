using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttack : MonoBehaviour
{
	GameObject Player;
    void Start()
    {
		Player = GameObject.Find("Player");
    }

	void FireBall()
	{
		Vector2 dragonPos = transform.position;
		Vector2 playerPos = Player.transform.position;
	}
}
