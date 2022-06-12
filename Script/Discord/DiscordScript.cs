using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscordScript : MonoBehaviour {

	GameDirector gameDirector;
	DiscordController discord;
	
	void Start () {
		gameDirector = GetComponent<GameDirector>();
		discord = GetComponent<DiscordController>();
	}
	
	public void Review()
	{
		int wave = GameDirector.GetWaveNow();
		discord.View(wave);
	}
}
