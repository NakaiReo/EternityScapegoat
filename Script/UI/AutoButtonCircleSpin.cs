using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoButtonCircleSpin : MonoBehaviour
{
	[SerializeField] float distance;
	[SerializeField] float spinSpeed;
	[SerializeField] float criterionAngle;
	[SerializeField] bool isBackGroundSpin;
	[SerializeField] RectTransform spinObject;
	public RectTransform[] buttons;

	Vector3 originPointPos;

	float r;
	float between;

    void Start()
    {
		InitInstantiate();
		Setup();
		r = 0;
		originPointPos = GetComponent<RectTransform>().position;
		between = 360.0f / buttons.Length;
		CircleArrangement(r);
		if (isBackGroundSpin && spinObject)
		{
			spinObject.position = originPointPos;
			spinObject.sizeDelta = new Vector2(1, 1) * distance  * 100 * 4;
		}
	}

    void Update()
    {
		r += spinSpeed * Time.deltaTime;
		CircleArrangement(r);
		if (isBackGroundSpin && spinObject) spinObject.Rotate(0, 0, spinSpeed * Time.deltaTime);
	}

	void CircleArrangement(float r)
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			Vector2 axis;
			float angle = between * i + r;
			axis.x = Mathf.Cos(angle * Mathf.Deg2Rad);
			axis.y = Mathf.Sin(angle * Mathf.Deg2Rad);
			Debug.Log(axis);
			buttons[i].position = originPointPos + (Vector3)axis * distance;
			buttons[i].eulerAngles = new Vector3(0, 0, 1) * (criterionAngle + angle);
		}
	}

	void InitInstantiate()
	{
		for(int i = 0; i < buttons.Length; i++)
		{
			Instantiate(transform.GetChild(i), transform);
		}
	}

	void Setup()
	{
		buttons = new RectTransform[buttons.Length * 2];
		for(int i=0;i < buttons.Length; i++)
		{
			buttons[i] = transform.GetChild(i).GetComponent<RectTransform>();
		}
	}
}
