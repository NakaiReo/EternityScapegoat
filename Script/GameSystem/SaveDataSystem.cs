using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Data
{
	public KeyCode[] keyCodes = new KeyCode[5];
	public KeyCode[] C_keyCodes = new KeyCode[5];
	public float[] soundVolume = new float[2];
	public bool soundMute;
	public float cameraFov;
	public float cameraOffsetX;
	public float cameraOffsetY;
}

public static class SaveDataSystem {

	public static Data saveData = new Data();

	public static void Save()
	{
		StreamWriter writer;

		string jsonData = JsonUtility.ToJson(saveData);

		writer = new StreamWriter(Application.dataPath + "/SaveData.json", false);
		writer.Write(jsonData);
		writer.Flush();
		writer.Close();
	}
	public static void load()
	{
		string dataStr = "";
		StreamReader reader;
		if(!File.Exists(Application.dataPath + "/SaveData.json"))
		{
			Reset();
			Save();
		}
		reader = new StreamReader(Application.dataPath + "/SaveData.json");
		dataStr = reader.ReadToEnd();
		reader.Close();
		saveData = JsonUtility.FromJson<Data>(dataStr);
	}
	public static void Reset()
	{
		saveData.keyCodes[0] = KeyCode.E;
		saveData.keyCodes[1] = KeyCode.R;
		saveData.keyCodes[2] = KeyCode.Q;
		saveData.keyCodes[3] = KeyCode.LeftShift;
		saveData.keyCodes[4] = KeyCode.Space;

		saveData.C_keyCodes[0] = KeyCode.E;
		saveData.C_keyCodes[1] = KeyCode.R;
		saveData.C_keyCodes[2] = KeyCode.Q;
		saveData.C_keyCodes[3] = KeyCode.LeftShift;
		saveData.C_keyCodes[4] = KeyCode.Space;

		saveData.soundVolume[0] = 0.5f;
		saveData.soundVolume[1] = 0.5f;
		saveData.soundMute = false;

		saveData.cameraFov = 7.0f;
		saveData.cameraOffsetX = 0.0f;
		saveData.cameraOffsetY = 0.0f;
	}
}
