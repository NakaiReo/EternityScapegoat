using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundDirector : MonoBehaviour {

	AudioClipLoad audioClipLoad;

	[SerializeField]
	AudioMixer audioMixer;
	AudioSource[] audioSources = new AudioSource[2];
	static AudioSource BGM;
	static AudioSource SE;

	[SerializeField]
	GameObject SoundParent;

	Text BGM_Text;
	Text SE_Text;
	Text Mute_Text;
	Slider BGM_Slider;
	Slider SE_Slider;
	Toggle Mute_Toggle;

	void Start () {
		audioClipLoad = GetComponent<AudioClipLoad>();
		audioSources = GetComponents<AudioSource>();
		BGM = audioSources[0];
		SE = audioSources[1];

		BGM_Text = SoundParent.transform.Find("BGM_Text").GetComponent<Text>();
		SE_Text = SoundParent.transform.Find("SE_Text").GetComponent<Text>();
		Mute_Text = SoundParent.transform.Find("Mute_Text").GetComponent<Text>();
		BGM_Slider = SoundParent.transform.Find("BGM_Slider").GetComponent<Slider>();
		SE_Slider = SoundParent.transform.Find("SE_Slider").GetComponent<Slider>();
		Mute_Toggle = SoundParent.transform.Find("Mute_Toggle").GetComponent<Toggle>();

		Load();
		BGM.Play();
	}
	
	// Update is called once per frame
	void Update () {
		audioMixer.SetFloat("Master", Mute_Toggle.isOn ? -80f : 0f);
		audioMixer.SetFloat("BGM", AudioMixerVolumeChange(BGM_Slider.value));
		audioMixer.SetFloat("SE", AudioMixerVolumeChange(SE_Slider.value));

		BGM_Text.text = "BGM : " + AudioMixerValuePercent(BGM_Slider.value) + "%";
		SE_Text.text = "SE : " + AudioMixerValuePercent(SE_Slider.value) + "%";
		Mute_Text.text = "Mute : " + (Mute_Toggle.isOn ? "ON" : "OFF");
	}

	public void PlaySE_NotStatic(string SEname)
	{
		PlaySE(SEname);
	}

	public static void PlaySE(string SEname)
	{
		AudioClip audioClip = Resources.Load("Sounds/SE/" + SEname) as AudioClip;
		if(audioClip == null)
		{
			Debug.LogWarning("SEを読み込めませんでした!");
			return;
		}
		SE.PlayOneShot(audioClip);
	}

	float AudioMixerVolumeChange(float value)
	{
		if (value <= 0.01f)
		{
			return -100;
		}
		return 20 * Mathf.Log10(value);
	}

	int AudioMixerValuePercent(float value)
	{
		return (int)(value * 100);
	}

	public void Save()
	{
		SaveDataSystem.saveData.soundVolume[0] = BGM_Slider.value;
		SaveDataSystem.saveData.soundVolume[1] = SE_Slider.value;
		SaveDataSystem.saveData.soundMute = Mute_Toggle.isOn;
		SaveDataSystem.Save();
	}



	public void SoundTest()
	{
		SE.PlayOneShot(SE.clip);
	}

	public void Load()
	{
		SaveDataSystem.load();
		bool mute = SaveDataSystem.saveData.soundMute;
		BGM_Slider.value = SaveDataSystem.saveData.soundVolume[0];
		SE_Slider.value = SaveDataSystem.saveData.soundVolume[1];
		SaveDataSystem.saveData.soundMute = mute;
		Mute_Toggle.isOn = SaveDataSystem.saveData.soundMute;
		Mute_Toggle.GetComponent<ToggleButtonImage>().ChangeImage();
	}
}
