using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFade : MonoBehaviour
{
	[SerializeField] GameObject fadeSet;
	Fade fade;

	void Start()
	{
		fade = GameObject.Find("FadeCanvas").GetComponent<Fade>();

		fadeSet.SetActive(true);
		fade.FadeIn(0f, () =>
		{
			fade.FadeOut(3);
			fadeSet.SetActive(false);
		});
		Invoke("FadeSet", 1.5f);
	}

	public void FadeSet()
	{
		fadeSet.SetActive(false);
	}

	public void QuickFadeOut()
	{
		fade.FadeIn(0f, () =>
		{
			fade.FadeOut(0);
			fadeSet.SetActive(false);
		});
	}

	public void FadeIn(int time)
	{
		fade.FadeIn(time);
	}
}
