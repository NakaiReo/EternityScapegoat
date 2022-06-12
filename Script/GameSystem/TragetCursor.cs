using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TragetCursor : MonoBehaviour
{
	[SerializeField] RectTransform ScopeImage;
	public static bool mouse = true;

	public void Update()
	{
		Vector2 dir;
		dir.x = Input.GetAxis("Mouse X");
		dir.y = Input.GetAxis("Mouse Y");
		Vector2 dir2;
		dir2.x = Input.GetAxisRaw("C_Horizontal2");
		dir2.y = Input.GetAxisRaw("C_Vertical2");
		if(dir != new Vector2(0,0) && mouse == false)
		{
			mouse = true;
		}
		else if(dir2 != new Vector2(0,0) && mouse == true)
		{
			mouse = false;
		}

		Debug.Log(mouse);

		if (mouse)
			MousePostionGet();
		else
			ControllerPosition();
	}
	public static Vector3 MousePosion = Vector3.zero;
	public void MousePostionGet()
	{
		Vector3 dir = Input.mousePosition;

		ScopeImage.localPosition = new Vector3((dir.x - Screen.width / 2.0f) * 1600.0f / Screen.width, (dir.y - Screen.height / 2.0f) * 900.0f / Screen.height, 0);
		Vector3 ScreenWorldMousePosition = Input.mousePosition;
		ScreenWorldMousePosition.z = 10f;

		MousePosion = Camera.main.ScreenToWorldPoint(ScreenWorldMousePosition);
	}

	public static Vector2 ControllerAxis = Vector2.zero;

	public void ControllerPosition()
	{
		Vector2 dir;
		dir.x = Input.GetAxisRaw("C_Horizontal2");
		dir.y = Input.GetAxisRaw("C_Vertical2");
		if (dir != new Vector2(0, 0))
		{
			ControllerAxis = dir.normalized;
		}
		ScopeImage.localPosition = ControllerAxis * 150;
		Vector3 ScreenWorldMousePosition = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0) + (Vector3)ControllerAxis * 150;
		ScreenWorldMousePosition.z = 10f;

		MousePosion = Camera.main.ScreenToWorldPoint(ScreenWorldMousePosition);
	}
}
