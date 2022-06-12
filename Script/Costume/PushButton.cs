using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CostumeDirector : MonoBehaviour
{
	[SerializeField] Button SaveButton;
	[SerializeField] Button TitleButton;
	[SerializeField] StartFade startFade;

	public void InteractableButton(bool eneble)
	{
		SaveButton.interactable = eneble;
		TitleButton.interactable = eneble;
	}

	public void GoTitleScene()
	{
		InteractableButton(false);
		startFade.FadeIn(5);
		Invoke("GoTitleSceneMove", 5.5f);
	}

	void GoTitleSceneMove()
	{
		SceneManager.LoadScene("Title");
	}
}
