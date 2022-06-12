using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
public class AstarAI : AIPath {

	void Start()
	{
		base.Start();
	}

	void Update()
	{
		base.Update();
		//base.Start();
	}
}
