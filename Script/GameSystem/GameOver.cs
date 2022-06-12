using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
	[SerializeField]GameObject PlayerStatusC;
	[SerializeField]GameObject GameOverC;
	static GameObject PlayerStatusCanvas;
	static GameObject GameOverCanvas;

	//GameOverComponents
	static Animator GameOverTextAnimator;
	static GameObject GameOverPanel;
	static Text LastWaveText;
	static Text KillCountText;
	static Text AccuracyRateText;
	static Text GeneralScoreText;

	static bool CanOpenMenu = true;
    void Start()
    {
		PlayerStatusCanvas = PlayerStatusC;
		GameOverCanvas = GameOverC;
		GameOverPanel = GameOverCanvas.transform.Find("Panel").gameObject;
		GameOverTextAnimator = GameOverPanel.GetComponent<Animator>();

		LastWaveText = GameOverPanel.transform.Find("LastWaveText").GetComponent<Text>();
		KillCountText = GameOverPanel.transform.Find("KillCountText").GetComponent<Text>();
		AccuracyRateText = GameOverPanel.transform.Find("AccuracyRateText").GetComponent<Text>();
		GeneralScoreText = GameOverPanel.transform.Find("GeneralScoreText").GetComponent<Text>();
	}

	public static void Gameover()
	{
		PlayerStatusCanvas.SetActive(false);
		GameOverCanvas.SetActive(true);
		GameOverTextAnimator.SetTrigger("FadeIn");

		float AccuracyRate = AccuracyRateClac();
		int GeneralScore = GeneralScoreClac();
		if(GeneralScore > TopScore.GetScore((int)GameMode.GameModeSelect, false))
		{
			TopScore.SaveScore(GeneralScore, (int)GameMode.GameModeSelect, false);
		}

		LastWaveText.text = "最終Wave : " + GameDirector.GetWaveNow();
		KillCountText.text = "倒した敵 : " + GameDirector.killCount;
		AccuracyRateText.text = "銃の命中率 : " + AccuracyRate.ToString("F1") + "%";
		GeneralScoreText.text = "総合スコア : " + GeneralScore + "pt";
	}

	public void Restart()
	{
		SceneManager.LoadScene("Game");
	}

	public void ReturnToTitle()
	{
		SceneManager.LoadScene("Title");
	}

	static float AccuracyRateClac()
	{
		if (GameDirector.shotCount <= 0) return 100;
		return (float)GameDirector.hitCount / GameDirector.shotCount * 100;
	}

	static int GeneralScoreClac()
	{
		float waveScore = 5 * Mathf.Pow(1.07f, GameDirector.GetWaveNow());
		float enemyScore = GameDirector.killCount;
		float accuracyRatScore = (AccuracyRateClac() - 50.0f) * 0.025f;
		DebugEX.LogMultiple(waveScore, enemyScore, accuracyRatScore, (int)(waveScore * enemyScore * accuracyRatScore));
		return (int)(waveScore * enemyScore * accuracyRatScore);
	}
}
