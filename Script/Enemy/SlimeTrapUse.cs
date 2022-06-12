using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTrapUse : MonoBehaviour
{
	[SerializeField] float trapDelay;
	[SerializeField] float trapAddMin;
	[SerializeField] float trapAddMax;
	[SerializeField] GameObject trapObject;

	Transform EnemyTransform;
	float trapTime;

    void Start()
    {
		EnemyTransform = GameObject.Find("Enemy").transform;
		trapTime = Random.Range(trapAddMin, trapAddMax);
    }

    void Update()
    {
		if (trapTime > 0) { trapTime -= Time.deltaTime; return; }

		Instantiate(trapObject, transform.position, Quaternion.identity, EnemyTransform);
		trapTime = trapDelay + Random.Range(trapAddMin, trapAddMax);
    }
}
