using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameModeSelecter : MonoBehaviour
{
	[Space(10)]
	[SerializeField] DiscordController discordController;

	[Space(10)]
	[SerializeField] Animator TitleAnimator;
	[SerializeField] GameObject fadeObject;

    [SerializeField] GameObject TitleCanvas;
	[SerializeField] GameObject GameModeSelectCanvas;
	[SerializeField] GameObject Bullet;

	[Space(10)]
	[SerializeField] Button NormalButton;
	[SerializeField] Button hardButton;
	[SerializeField] Button MayhemButton;
	[SerializeField] Button BackButton;

	Fade fade;
	bool GameStartBool;
	float waitTime = 0;

    void Start()
    {
		fade = fadeObject.GetComponent<Fade>();
		GameStartBool = false;
		waitTime = 0;
		fade.FadeOut(1);

		discordController.Title();

		TitleCanvas.SetActive(true);
		GameModeSelectCanvas.SetActive(false);
		Bullet.SetActive(false);

		ChangeActive(true);
	}

	private void Update()
	{
		if (Input.anyKeyDown && TitleAnimator.GetCurrentAnimatorStateInfo(0).IsName("TitleStart"))
		{
			TitleAnimator.Play(0, 0, 9.9f);
			TitleAnimator.transform.GetComponent<RankingStart>().StartRanking();
		}
		if (GameStartBool == false) return;
		waitTime += Time.deltaTime;
		if (waitTime > 6.0f)
			SceneManager.LoadScene("Game");
	}

	public void GameModeSelecterOpen(bool Open)
	{
		TitleCanvas.SetActive(!Open);
		GameModeSelectCanvas.SetActive(Open);
	}

	public void GameStart(int id)
	{
		Bullet.SetActive(true);
		Vector2 ClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Bullet.transform.position = ClickPos;
		GameMode.Gamemode gamemode = (GameMode.Gamemode)id;
		GameMode.GameModeSelect = gamemode;
		ChangeActive(false);
		fade.FadeIn(5);
		GameStartBool = true;
	}

	void ChangeActive(bool enable)
	{
		NormalButton.interactable = enable;
		hardButton.interactable = enable;
		MayhemButton.interactable = enable;
		BackButton.interactable = enable;
	}
}