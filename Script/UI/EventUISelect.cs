using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class EventUISelect : MonoBehaviour
{
	EventSystem eventSystem;
	public GameObject ResetSelectObject;
	[SerializeField] bool MouseMoveUnSelect;

	private void Start()
	{
		eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
	}

	void Update()
    {
		if (MouseMoveUnSelect == true)
		{
			Vector2 mouseMove;
			mouseMove.x = Input.GetAxis("Mouse X");
			mouseMove.y = Input.GetAxis("Mouse Y");
			if(mouseMove != new Vector2(0, 0))
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
		}
		Vector2 input;
		input.x = Input.GetAxis("Horizontal");
		input.y = Input.GetAxis("Vertical");
		if(input != new Vector2(0, 0))
		{
			if(eventSystem.currentSelectedGameObject == null && ResetSelectObject)
			{
				ResetSelectObject.GetComponent<Selectable>().Select();
			}
		}
    }

	public void SetNull()
	{
		ResetSelectObject = null;
		EventSystem.current.SetSelectedGameObject(null);
	}

	public void ResetSelect(GameObject obj)
	{
		obj.GetComponent<Selectable>().Select();
		ResetSelectObject = obj;
	}

	public void Select(GameObject obj)
	{
		obj.GetComponent<Selectable>().Select();
	}
}
