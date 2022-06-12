using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCircleSpin : MonoBehaviour
{
	[SerializeField] float distance;
	[SerializeField] float criterionAngle;
	[SerializeField] bool isBackGroundSpin;
	[SerializeField] RectTransform spinObject;
	public RectTransform[] buttons;

	Vector3 originPointPos;

	float constant;
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
		MoveCircle();
		CircleArrangement(r);
	}


	Vector2 clickPos;
	Vector2 movePos;
	float mouseScrollValue = 0;
	void MoveCircle()
	{
		Vector2 inputKeyAxis;
		inputKeyAxis.x = Input.GetAxisRaw("Horizontal");
		inputKeyAxis.y = Input.GetAxisRaw("Vertical");
		float inputKeyClac = inputKeyAxis.x + inputKeyAxis.y;

		if (Input.GetMouseButton(0))
		{
			if(Input.GetMouseButtonDown(0)) clickPos = Input.mousePosition;
			movePos = Input.mousePosition;
			Vector2 amountValue = movePos - clickPos;
			inputKeyClac = (amountValue.x + amountValue.y) * 0.005f;
		}

		mouseScrollValue += Input.GetAxis("Mouse ScrollWheel") * 500;

		if(mouseScrollValue > 0)
		{
			mouseScrollValue -= 5;
			r += 1;
			if (mouseScrollValue < 0) mouseScrollValue = 0;
		}
		if (mouseScrollValue < 0)
		{
			mouseScrollValue += 5;
			r -= 1;
			if (mouseScrollValue > 0) mouseScrollValue = 0;
		}

		r -= inputKeyClac;
		spinObject.rotation = Quaternion.Euler(0, 0, r);
	}

	void CircleArrangement(float r)
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			Vector2 axis;
			float angle = between * i + r;
			axis.x = Mathf.Cos(angle * Mathf.Deg2Rad);
			axis.y = Mathf.Sin(angle * Mathf.Deg2Rad);
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
