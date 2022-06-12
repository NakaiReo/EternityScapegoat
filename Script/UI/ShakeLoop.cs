using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeLoop : MonoBehaviour
{
	RectTransform rectTran;

	float time;
	[SerializeField] float spanTime;
	[SerializeField] float runTime;
	[SerializeField,Range(0,100)] float power;

	void Start()
	{
		rectTran = GetComponent<RectTransform>();
		time = 0;
	}

	// Update is called once per frame
	void Update()
    {
		time += Time.deltaTime;
		if(time >= spanTime)
		{
			time = 0;
			rectTran.DOShakeRotation(runTime, power);
		}
    }
}
