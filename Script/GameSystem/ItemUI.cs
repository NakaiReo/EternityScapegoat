using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {

	static GameObject ItemCanvas;
	static GameObject DamagePlusObject;
	static Text DamagePlusText;
	static GameObject PointPlusObject;
	static Text PointPlusText;

	static float damagePlusTime = 0;
	static float pointPlusTime = 0;

	void Start () {
		DamagePlusObject = transform.Find("DamagePlus").gameObject;
		PointPlusObject = transform.Find("PointPlus").gameObject;
		DamagePlusText = DamagePlusObject.transform.Find("Text").GetComponent<Text>();
		PointPlusText = PointPlusObject.transform.Find("Text").GetComponent<Text>();

		damagePlusTime = 0;
		pointPlusTime = 0;

		DamagePlusObject.SetActive(false);
		PointPlusObject.SetActive(false);
	}
	
	void Update () {
		damagePlusTime = damagePlusTime > 0 ? damagePlusTime -= Time.deltaTime : 0;
		pointPlusTime = pointPlusTime > 0 ? pointPlusTime -= Time.deltaTime : 0;
		DamagePlusObject.SetActive(damagePlusTime > 0 ? true : false);
		PointPlusObject.SetActive(pointPlusTime > 0 ? true : false);
		if (damagePlusTime > 0)
		{
			DamagePlusText.text = damagePlusTime.ToString("F1");
		}
		if (pointPlusTime > 0)
		{
			PointPlusText.text = pointPlusTime.ToString("F1");
		}
	}

	public static void SetDamagePlusTime(float time)
	{
		damagePlusTime = time;
	}
	public static void SetPointPlusTime(float time)
	{
		pointPlusTime = time;
	}
	public static float GetDamagePlusTime()
	{
		return damagePlusTime;
	}
	public static float GetPointPlusTime()
	{
		return pointPlusTime;
	}
	public static int MagnificationDamagePlus()
	{
		return damagePlusTime > 0 ? 2 : 1;
	}
	public static int MagnificationPointPlus()
	{
		return pointPlusTime > 0 ? 2 : 1;
	}
}
