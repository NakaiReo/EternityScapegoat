using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class KeyConfig : MonoBehaviour {

	[SerializeField]
	GameObject KeyConfigObject;

	Text MessageBoxText;

	public static KeyCode Use;
	public static KeyCode Reload;
	public static KeyCode WeaponChange;
	public static KeyCode Dash;
	public static KeyCode Item;

	public static KeyCode C_Use;
	public static KeyCode C_Reload;
	public static KeyCode C_WeaponChange;
	public static KeyCode C_Dash;
	public static KeyCode C_Item;

	public static KeyCode[] DisableKeys = {KeyCode.Escape,KeyCode.Mouse0,KeyCode.Mouse1};

	int keyID = -1;

	void Start()
	{
		MessageBoxText = KeyConfigObject.transform.Find("MessageBox").transform.Find("Text").GetComponent<Text>();
		KeySet();
	}

	private void Update()
	{
		if (keyID != -1)
		{
			if (Input.anyKeyDown)
			{
				foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
				{
					if (Input.GetKeyDown(code))
					{
						for (int i = 0; i < DisableKeys.Length; i++)
						{
							if (DisableKeys[i] == code)
							{
								return;
							}
						}

						KeyChange(code);
					}
				}
			}

		}
	}

	void KeyChange(KeyCode code)
	{
		for(int i = 0; i < 5; i++)
		{
			if(code == SaveDataSystem.saveData.keyCodes[i])
			{
				KeyCode temp;
				temp = SaveDataSystem.saveData.keyCodes[i];
				SaveDataSystem.saveData.keyCodes[i] = SaveDataSystem.saveData.keyCodes[keyID];
				code = temp;
				break;
			}
		}

		SaveDataSystem.saveData.keyCodes[keyID] = code;
		SaveDataSystem.Save();
		MessageBoxText.text = "キーを変更しました";
		KeyConfigButton(true);
		KeyConfigMes();
		KeySet();
		keyID = -1;
	}

	void KeySet()
	{
		Use = SaveDataSystem.saveData.keyCodes[0];
		Reload = SaveDataSystem.saveData.keyCodes[1];
		WeaponChange = SaveDataSystem.saveData.keyCodes[2];
		Dash = SaveDataSystem.saveData.keyCodes[3];
		Item = SaveDataSystem.saveData.keyCodes[4];
	}

	public void KeyConfigLoadMenu()
	{
		MessageBoxText.text = "変更するキーを選択してください";
		KeyConfigButton(true);
		KeyConfigMes();
	}

	public void KeyInputSetup(int keyID)
	{
		this.keyID = keyID;
		MessageBoxText.text = "キーを入力してください";
		KeyConfigButton(false);
	}

	public void KeyConfigButton(bool enable)
	{
		KeyConfigObject.transform.Find("UseKeyButton").GetComponent<Button>().interactable = enable;
		KeyConfigObject.transform.Find("ReloadKeyButton").GetComponent<Button>().interactable = enable;
		KeyConfigObject.transform.Find("WeaponChangeKeyButton").GetComponent<Button>().interactable = enable;
		KeyConfigObject.transform.Find("DashKeyButton").GetComponent<Button>().interactable = enable;
		KeyConfigObject.transform.Find("ItemKeyButton").GetComponent<Button>().interactable = enable;
	}
	public void KeyConfigMes()
	{
		KeyConfigObject.transform.Find("UseKeyButton").transform.Find("Text").GetComponent<Text>().text = "  Use :   " + SaveDataSystem.saveData.keyCodes[0];
		KeyConfigObject.transform.Find("ReloadKeyButton").transform.Find("Text").GetComponent<Text>().text = "  Reload :   " + SaveDataSystem.saveData.keyCodes[1];
		KeyConfigObject.transform.Find("WeaponChangeKeyButton").transform.Find("Text").GetComponent<Text>().text = "  Weapon Change :   " + SaveDataSystem.saveData.keyCodes[2];
		KeyConfigObject.transform.Find("DashKeyButton").transform.Find("Text").GetComponent<Text>().text = "  Dash :   " + SaveDataSystem.saveData.keyCodes[3];
		KeyConfigObject.transform.Find("ItemKeyButton").transform.Find("Text").GetComponent<Text>().text = "  Item :   " + SaveDataSystem.saveData.keyCodes[4];
	}
}
