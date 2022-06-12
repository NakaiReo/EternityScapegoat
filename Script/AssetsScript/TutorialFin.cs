using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialFin : MonoBehaviour
{
	[SerializeField] Fade fade;
	GameObject Player;

    void Start()
    {
		Player = GameObject.Find("Player");
    }
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			fade.FadeIn(5f, () =>
			{
				fade.FadeIn(5);
			});
			Invoke("GoTitle", 5);
		}
	}

	void GoTitle()
	{
		SceneManager.LoadScene("Title");
	}
}
