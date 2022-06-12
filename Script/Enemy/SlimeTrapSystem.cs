using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTrapSystem : MonoBehaviour
{
	GameObject Player;
	PlayerController playerController;

	float size;
	bool d;

    void Start()
    {
		Player = GameObject.Find("Player");
		playerController = Player.GetComponent<PlayerController>();
		size = 2;
    }

	private void Update()
	{
		transform.localScale = new Vector3(1, 1, 1) * size;
		size -= 0.05f * Time.deltaTime;
		if (size < 0f) Destroy(gameObject);
		return;
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			playerController.Slowness(1f);
		}
	}
	void dOn()
	{
		d = true;
	}
}
