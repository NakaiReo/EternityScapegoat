using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour {

	KeyConfig KeyConfigScript;

	public enum MainMenuID:int
	{
		Close = -1,
		MainMenu,
		Option,
		GameSystem
	}
	public enum SubMenuID : int
	{
		Close = -1,
		GameSystem = 0,
		KeyConfig,
		SoundOption,

		ReturnToTitle = 100,
		RestartGame,
		ShutDown
	}
	[HideInInspector] public MainMenuID mainMenu = MainMenuID.Close;
	[HideInInspector] public SubMenuID subMenu = SubMenuID.Close;

	[SerializeField] bool HideMouse;
	[Space(10)]
	[SerializeField] GameObject MenuCanvas;
	[SerializeField] GameObject GameCanvas;
	[SerializeField] GameObject FadeOut;
	[SerializeField] GameObject TrailRendererObject;

	[Space(10)]
	[SerializeField] GameObject MainMenuFrame;
	[SerializeField] GameObject OptionPanel;
	[SerializeField] GameObject GameSystemPanel;

	[Space(10)]
	[SerializeField] GameObject SubMenuPanel;
	[SerializeField] GameObject GameSystem;
	[SerializeField] GameObject KeyConfig;
	[SerializeField] GameObject SoundOption;
	[SerializeField] GameObject Confirmation;

	[Space(10)]
	[SerializeField] GameObject MessageBox;

	bool Pause;

	void Awake () {
		SaveDataSystem.load();

		mainMenu = MainMenuID.MainMenu;
		Pause = false;
		ChangeUI();

		if (HideMouse == true) MouseCursorChange.ChangeMouseScope();
		else MouseCursorChange.ChangeDefaultMouse();
	}
	

	void Update () {
		if (Input.GetButtonDown("Menu"))
		{
			Pause = !Pause;
			ChangeUI();
		}
	}

	public void ChangeUI()
	{
		Time.timeScale = Pause == true ? 0.0f : 1.0f;

		if (TrailRendererObject)
			TrailRendererObject.GetComponent<TrailRenderer>().enabled = !Pause;

		if(GameCanvas)
			GameCanvas.SetActive(!Pause);
		MenuCanvas.SetActive(Pause);
		if (!Pause)
		{
			MouseCursorChange.ChangeDefaultMouse();
			if (HideMouse == true) MouseCursorChange.ChangeMouseScope();
			else MouseCursorChange.ChangeDefaultMouse();
		}
		else
		{
			MouseCursorChange.ChangeDefaultMouse();
			if (FadeOut) FadeOut.SetActive(false);
			MainMenuChange(0);
		}
	}

	public void MainMenuChange(int menuID)
	{
		mainMenu = (MainMenuID)menuID;
		SubMenuChange(-1);
		if (mainMenu == MainMenuID.Close)
		{
			mainMenu = MainMenuID.MainMenu;
			Pause = false;
			ChangeUI();
		}
		MainMenuFrame.SetActive(true);
		OptionPanel.SetActive(mainMenu == MainMenuID.Option ? true : false);
		GameSystemPanel.SetActive(mainMenu == MainMenuID.GameSystem ? true : false);
	}

	public void SubMenuChange(int menuID)
	{
		subMenu = (SubMenuID)menuID;
		SubMenuPanel.SetActive(subMenu != SubMenuID.Close ? true : false);
		GameSystem.SetActive(subMenu == SubMenuID.GameSystem ? true : false);
		KeyConfig.SetActive(subMenu == SubMenuID.KeyConfig ? true : false);
		SoundOption.SetActive(subMenu == SubMenuID.SoundOption ? true : false);
		Confirmation.SetActive((int)subMenu >= 100 ? true : false);
		ConfirmationText();
	}

	public void ConfirmationText()
	{
		string mes = "Null";
		switch (subMenu)
		{
			case SubMenuID.ReturnToTitle:
				mes = "本当にタイトルに戻りますか?";
				break;
			case SubMenuID.RestartGame:
				mes = "本当にゲームをリセットますか?";
				break;
			case SubMenuID.ShutDown:
				mes = "本当にゲームを終了よろしいですか?";
				break;
		}
		MessageBox.GetComponent<Text>().text = mes;
	}

	public void ConfirmationButton(bool Confirm)
	{
		if (Confirm == false)
		{
			SubMenuChange(-1);
			return;
		}
		switch (subMenu)
		{
			case SubMenuID.ReturnToTitle:
				SceneManager.LoadScene("Title");
				break;
			case SubMenuID.RestartGame:
				SceneManager.LoadScene("Game");
				break;
			case SubMenuID.ShutDown:
				ShutDown();
				break;
		}
	}

	public void SettingOpenOrClose()
	{
		Pause = !Pause;
		ChangeUI();
	}

	public void SceneLoad(string SceneName)
	{
		SceneManager.LoadScene(SceneName);
	}

	public void ShutDown()
	{
	#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
	#elif UNITY_STANDALONE
		UnityEngine.Application.Quit();
	#endif
	}
}
