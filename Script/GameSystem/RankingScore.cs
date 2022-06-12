using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NCMB;

public static class TopScore
{
	public static class Local
	{
		public static int Normal;
		public static int Hard;
		public static int Mayhem;
	}
	public static class Online
	{
		public static int Normal;
		public static int Hard;
		public static int Mayhem;
	}

	public static void ResetScore()
	{
		PlayerPrefs.SetInt("LocalTopScoreNormal", 0);
		PlayerPrefs.SetInt("LocalTopScoreHard", 0);
		PlayerPrefs.SetInt("LocalTopScoreMayhem", 0);
		PlayerPrefs.SetInt("OnlineTopScoreNormal", 0);
		PlayerPrefs.SetInt("OnlineTopScoreHard", 0);
		PlayerPrefs.SetInt("OnlineTopScoreMayhem", 0);
		SaveScoreOnline(0, 0);
		SaveScoreOnline(0, 1);
		SaveScoreOnline(0, 2);
	}

	public static string GetMode(int id)
	{
		switch (id)
		{
			case 0:
				return "Normal";
			case 1:
				return "Hard";
			case 2:
				return "Mayhem";
			default:
				Debug.LogError("範囲外です");
				break;
		}
		return "Null";
	}

	public static int GetScore(int id, bool online)
	{
		switch(id)
		{
			case 0:
				return online == false ? PlayerPrefs.GetInt("LocalTopScoreNormal") : PlayerPrefs.GetInt("OnlineTopScoreNormal");
			case 1:
				return online == false ? PlayerPrefs.GetInt("LocalTopScoreHard") : PlayerPrefs.GetInt("OnlineTopScoreHard");
			case 2:
				return online == false ? PlayerPrefs.GetInt("LocalTopScoreMayhem") : PlayerPrefs.GetInt("OnlineTopScoreMayhem");
			default:
				Debug.LogError("範囲外です");
				break;
		}
		return -1;
	}
	public static void SetScore(int id, bool online)
	{
		switch (id)
		{
			case 0:
				if (online == false)
				{
					Local.Normal = PlayerPrefs.GetInt("LocalTopScoreNormal");
				}
				else
				{
					Online.Normal = PlayerPrefs.GetInt("OnlineTopScoreNormal");
				}
				break;
			case 1:
				if (online == false)
				{
					Local.Hard = PlayerPrefs.GetInt("LocalTopScoreHard");
				}
				else
				{
					Online.Hard = PlayerPrefs.GetInt("OnlineTopScoreHard");
				}
				break;
			case 2:
				if (online == false)
				{
					Local.Mayhem = PlayerPrefs.GetInt("LocalTopScoreMayhem");
				}
				else
				{
					Online.Mayhem = PlayerPrefs.GetInt("OnlineTopScoreMayhem");
				}
				break;
			default:
				Debug.LogError("範囲外です");
				break;
		}
	}
	public static void SaveScore(int value, int id, bool online)
	{
		switch (id)
		{
			case 0:
				if (online == false)
				{
					PlayerPrefs.SetInt("LocalTopScoreNormal", value);
				}
				else
				{
					PlayerPrefs.SetInt("OnlineTopScoreNormal", value);
				}
				break;
			case 1:
				if (online == false)
				{
					PlayerPrefs.SetInt("LocalTopScoreHard", value);
				}
				else
				{
					PlayerPrefs.SetInt("OnlineTopScoreHard", value);
				}
				break;
			case 2:
				if (online == false)
				{
					PlayerPrefs.SetInt("LocalTopScoreMayhem", value);
				}
				else
				{
					PlayerPrefs.SetInt("OnlineTopScoreMayhem", value);
				}
				break;
			default:
				Debug.LogError("範囲外です");
				break;
		}
	}
	public static int[] loadStat;
	public static void LoadScoreOnline()
	{
		loadStat = new int[3];

		for (int i = 0; i < 3; i++)
		{
			loadStat[i] = 0;
			LoadScoreNCMB(i);
		}
	}
	public static bool LoadCheck()
	{
		DebugEX.LogMultiple("LoadStat : ",loadStat[0], loadStat[1], loadStat[2]);
		if (loadStat[0] == 0) return false;
		if (loadStat[1] == 0) return false;
		if (loadStat[2] == 0) return false;
		return true;
	}
	public static void LoadScoreNCMB(int id)
	{
		NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>("Ranking");
		query.WhereEqualTo("Mode", ((GameMode.Gamemode)id).ToString());
		query.OrderByDescending("Score");
		query.Limit = 1;
		query.FindAsync((List<NCMBObject> objList, NCMBException e) =>
			{
				if (e != null)
				{
					Debug.LogWarning("オンラインのデータ取得に失敗しました。" + e.ErrorCode);
					SaveScore(-1, id, true);
					loadStat[id] = -1;
				}
				else
				{
					Debug.Log("オンラインのデータ取得に成功しました。");
					foreach (NCMBObject obj in objList)
					{
						int score = System.Convert.ToInt32(obj["Score"]);
						if (score > GetScore(id, false))
						{
							SaveScore(score, id, true);
						}
						else
						{
							SaveScore(GetScore(id, false), id, true);
							SaveScoreOnline(GetScore(id, false), id);
						}
					}
					loadStat[id] = 1;
				}
			});
	}
	public static void SaveScoreOnline(int score, int mode)
	{
		NCMBObject obj = new NCMBObject("Ranking");
		obj["Mode"] = ((GameMode.Gamemode)mode).ToString();
		obj["Score"] = score;
		obj.SaveAsync((NCMBException e) =>
		{
			if (e != null)
			{
				Debug.LogWarning("オンラインのデータ保存に失敗しました。" + e.ErrorCode);
			}
			else
			{
				Debug.Log("オンラインのデータ保存に成功しました。" + obj.ObjectId);
			}
		});
	}
}

public class RankingScore : MonoBehaviour
{
	[SerializeField] Text TitleText;
	[SerializeField] Text ModeText;
	[SerializeField] Text LocalTopScoreText;
	[SerializeField] Text OnlineTopScoreText;

	int id;

    void Start()
    {
		//TopScore.ResetScore();
		TopScore.LoadScoreOnline();
		id = 0;
	}
	public void changeTopScore()
	{
		TextView(id);
		id = id < 2 ? id + 1 : 0;
	}

	void TextView(int id)
	{
		ModeText.text = TopScore.GetMode(id) + " Mode";
		LocalTopScoreText.text = "Local : " + TopScore.GetScore(id, false) + "pt";
		int score = TopScore.GetScore(id, true);
		if (score != -1)
			OnlineTopScoreText.text = "Online : " + score + "pt";
		else
			OnlineTopScoreText.text = "Online : Disconnect";
	}
}
