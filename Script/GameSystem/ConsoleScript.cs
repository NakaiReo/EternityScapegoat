using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleScript : MonoBehaviour {

	[SerializeField]
	InputField CommandLine;
	[SerializeField]
	GameObject ConsoleCommandMes;

	RectTransform rectTransform;
	Transform CommandLog;
	Toggle Label;

	GameObject Player;
	PlayerStatus playerStatus;
	PlayerShotBullet playerShotBullet;

	bool Cheat = true;

	void Awake () {
		rectTransform = GetComponent<RectTransform>();
		CommandLog = transform.Find("CommandLog");
		Label = transform.Find("Toggle").GetComponent<Toggle>();
		Player = GameObject.Find("Player");
		playerStatus = Player.GetComponent<PlayerStatus>();
		playerShotBullet = Player.GetComponent<PlayerShotBullet>();
		Debug.Log(rectTransform.offsetMin);
		Debug.Log(rectTransform.offsetMax);
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Return))
		{
			ViewMes(">>" + CommandLine.text);
			ConsoleCommand(CommandLine.text);
			CommandLine.text = null;
		}
	}
	void ConsoleCommand(string command)
	{
		string[] commandSplit = command.Split(' ');
		switch (commandSplit[0])
		{
			case "Cheat":
				if (commandSplit.Length != 3) { ViewMes("引数の長さが違います"); break; }
				if (commandSplit[2] != "yasaiMasimasi") { ViewMes("パスワードが違います"); break; }
				if(commandSplit[1] != "ON" && commandSplit[1] != "OFF") { ViewMes("ONかOFFを選択してください"); break; }
				Cheat = commandSplit[1] == "ON" ? true : false;
				ViewMes("チートを" + commandSplit[1] + "にしました");
				break;
			case "Money":
				if(Cheat==false) { ViewMes("チートは許可されていません"); break; }
				if (commandSplit.Length != 2) { ViewMes("引数の長さが違います"); break; }
				int amount = int.Parse(commandSplit[1]);
				playerStatus.GetMoney(amount);
				ViewMes("所持金を" + playerStatus.GetMoneyValue() + "にしました");
				break;
			case "Weapon":
				if (Cheat == false) { ViewMes("チートは許可されていません"); break; }
				if (commandSplit.Length != 3) { ViewMes("引数の長さが違います"); break; }
				int haveID = int.Parse(commandSplit[1]);
				int gunID = int.Parse(commandSplit[2]);
				if (haveID != 0 && haveID != 1) { ViewMes("所持武器指定が範囲外です"); break; }
				if (gunID < 0 || gunID > 35) { ViewMes("武器IDが範囲外です"); break; }
				PlayerShotBullet.haveGunID = haveID;
				playerShotBullet.TradeWeapon(gunID);
				ViewMes("武器を切り替えました");
				break;
			default:
				ViewMes("コマンドが見つかりませんでした");
				break;
		}
	}

	void ViewMes(string mes)
	{
		if (CommandLog.childCount > 20)
		{
			for(int i = 0; CommandLog.childCount - i >= 20; i++)
			{
				Destroy(CommandLog.transform.GetChild(i).gameObject);
			}
		}
		GameObject ins = Instantiate(ConsoleCommandMes, CommandLog);
		ins.GetComponent<Text>().text = mes;
	}
	public void ResetConsole()
	{
		for(int i = 0; i < CommandLog.childCount; i++)
		{
			Destroy(CommandLog.transform.GetChild(i).gameObject);
		}
	}

	public void LabelView()
	{
		if (Label.isOn == true)
		{
			//上る
			rectTransform.offsetMin = new Vector2(0, 0);
			rectTransform.offsetMax = new Vector2(450, 585);
		}
		else
		{
			//下る
			rectTransform.offsetMin = new Vector2(0, -565);
			rectTransform.offsetMax = new Vector2(450, 15);
		}
	}
}
