using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSelector : MonoBehaviour
{
	[SerializeField] StartFade startFade;

	[Space(10)]
	[SerializeField] Button StartGameButton;
	[SerializeField] Button CostumeButton;
	[SerializeField] Button TutorialButton;
	[SerializeField] Button OptionButton;
	[SerializeField] Button ShutdownButton;

	public void InteractableButton(bool enable)
	{
		StartGameButton.interactable = enable;
		CostumeButton.interactable = enable;
		TutorialButton.interactable = enable;
		OptionButton.interactable = enable;
		ShutdownButton.interactable = enable;
	}

	public void GoCostumeScene()
	{
		InteractableButton(false);
		startFade.FadeIn(5);
		sceneID = 1;
		Invoke("GoSceneMove", 5.5f);
	}
	public void GoTutorialScene()
	{
		InteractableButton(false);
		startFade.FadeIn(5);
		sceneID = 2;
		Invoke("GoSceneMove", 5.5f);
	}
	public void GoTrainingScene()
	{
		InteractableButton(false);
		startFade.FadeIn(5);
		sceneID = 3;
		Invoke("GoSceneMove", 5.5f);
	}

	int sceneID;
	void GoSceneMove()
	{
		switch (sceneID)
		{
			case 1:
			SceneManager.LoadScene("Costume");
				break;
			case 2:
				SceneManager.LoadScene("Tutorial");
				break;
			case 3:
				SceneManager.LoadScene("Training");
				break;
		}
	}
}
