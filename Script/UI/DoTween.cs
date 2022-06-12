using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoTween : MonoBehaviour
{
	RectTransform rectTran;

	void Start()
    {
		rectTran = GetComponent<RectTransform>();
    }

    public void Punch()
	{
		rectTran.DOPunchScale(new Vector3(1.5f, 1.5f), 1.0f);
	}
	public void Shake()
	{
		rectTran.DOShakeScale(1.0f, 0.15f);
	}
}
