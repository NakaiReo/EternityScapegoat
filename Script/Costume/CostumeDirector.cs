using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PushButton : MonoBehaviour
{
	[SerializeField] Button SaveButton;
	[SerializeField] Button TitleButton;
	[SerializeField] Fade fade;

	private void Start()
	{
		
	}

	public void GoTitle()
	{
		SceneManager.LoadScene("Title");
	}
	public void GoCostume()
	{
		SceneManager.LoadScene("Costume");
	}
}
