using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinPicker : MonoBehaviour
{
	Transform InstanceFolder;

	[SerializeField] int viewCount;
	[Space(10)]
	[SerializeField] float distance;
	[SerializeField] float offsetAngle;
	[SerializeField] float between; 
	public GameObject[] buttonsObject;
	RectTransform[] buttonsRectTransform;
	Text[] buttonsText;
	float[] buttonsAngle;

	Vector3[] viewPosition;

	Vector3 originPointPos;

	int selectButton = 0;

    void Start()
    {
		ButtonsGetComponent();
		viewPosition = new Vector3[viewCount];
		originPointPos = GetComponent<RectTransform>().position;
		//between = 360.0f / buttons.Length;
		CircleArrangement();
		//InstallationButton();
    }

    void Update()
    {
	}

	void ButtonsGetComponent()
	{
		buttonsRectTransform = new RectTransform[buttonsObject.Length];
		buttonsText = new Text[buttonsObject.Length];
		buttonsAngle = new float[buttonsObject.Length];

		for(int i = 0; i < buttonsObject.Length; i++)
		{
			buttonsRectTransform[i] = buttonsObject[i].GetComponent<RectTransform>();
			buttonsText[i] = buttonsObject[i].transform.GetChild(0).GetComponent<Text>();
			buttonsAngle[i] = 0.0f;
		}
	}

	void ClacAngle()
	{
		int adjustment = (int)(offsetAngle - between * (int)(buttonsObject.Length / 2));
		for (int i = 0; i < buttonsObject.Length; i++)
		{
			buttonsAngle[i] = adjustment + between * i;
		}
		DebugEX.LogArray(buttonsAngle);
	}

	void CircleArrangement()
	{
		ClacAngle();
		for (int i = 0; i < buttonsObject.Length; i++)
		{
			Vector2 axis;
			axis.x = Mathf.Cos(buttonsAngle[i] * Mathf.Deg2Rad);
			axis.y = Mathf.Sin(buttonsAngle[i] * Mathf.Deg2Rad);
			buttonsRectTransform[i].position = originPointPos + (Vector3)axis * distance;
		}
	}

	void InstallationButton()
	{
		//for(int i = 0; i < InstanceFolder.childCount; i++)
		//{
		//	Destroy(InstanceFolder.GetChild(i).gameObject);
		//}
		for(int i = 0; i < viewCount; i++)
		{
			int temp = selectButton + i - (int)(viewCount / 2);
			temp = temp < buttonsObject.Length ? temp : 0;
			temp = temp > -1 ? temp : buttonsObject.Length - 1;

			buttonsRectTransform[temp].position = viewPosition[i];
		}
	}
}
