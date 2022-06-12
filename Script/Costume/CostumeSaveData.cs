using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CostumeData
{
	public int hatData;
	public int hairData;
	public int bodyData;
}

public class CostumeSaveData : MonoBehaviour
{
	public static CostumeData costumeData = new CostumeData();

	public static void Save()
	{
		StreamWriter writer;

		string jsonData = JsonUtility.ToJson(costumeData);

		writer = new StreamWriter(Application.dataPath + "/CostumeData.json", false);
		writer.Write(jsonData);
		writer.Flush();
		writer.Close();
	}
	public static void load()
	{
		string dataStr = "";
		StreamReader reader;
		if (!File.Exists(Application.dataPath + "/CostumeData.json"))
		{
			Reset();
			Save();
		}
		reader = new StreamReader(Application.dataPath + "/CostumeData.json");
		dataStr = reader.ReadToEnd();
		reader.Close();
		costumeData = JsonUtility.FromJson<CostumeData>(dataStr);
	}
	public static void Reset()
	{
		costumeData.hatData = 1;
		costumeData.hairData = 1;
		costumeData.bodyData = 1;
	}
}
