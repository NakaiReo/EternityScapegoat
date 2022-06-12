using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingStart : MonoBehaviour
{
	static bool b;

	[SerializeField] Animator TitleAnimator;
	[SerializeField] Animator animator;

	public void StartRanking()
	{
		b = true;
		animator.SetBool("Run", true);
	}

	public void load()
	{
		if (TopScore.LoadCheck())
		{
			TitleAnimator.SetTrigger("LoadFin");
		}
	}
}
