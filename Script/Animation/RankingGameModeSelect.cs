using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingGameModeSelect : MonoBehaviour
{
	[SerializeField] Text ModeText;
	[SerializeField] Text LocalTopScoreText;
	[SerializeField] Text OnlineTopScoreText;

	public void ChangeText(int id)
	{
		if (id >= 0)
		{
			ModeText.text = TopScore.GetMode(id) + " Mode";
			LocalTopScoreText.text = "Local : " + TopScore.GetScore(id, false) + "pt";
			int score = TopScore.GetScore(id, true);
			if (score != -1)
				OnlineTopScoreText.text = "Online : " + score + "pt";
			else
				OnlineTopScoreText.text = "Online : Disconnect";
		}
		else
		{
			ModeText.text = "";
			LocalTopScoreText.text = "";
			OnlineTopScoreText.text = "";
		}
	}
}
