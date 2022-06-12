using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTime : MonoBehaviour
{
	[SerializeField] bool Play;
	public static int minB = 0;
	public static int min = 0;

    void FixedUpdate()
    {
		min = Mathf.CeilToInt(Time.time / 60);
		//DebugEX.LogMultiple(minB, min, Time.time);
		if (minB != min)
		{
			if (!Play)
				GetComponent<DiscordController>().Title();
			else
				GetComponent<DiscordController>().View(GameDirector.GetWaveNow());
			minB = min;
		}
    }
}
