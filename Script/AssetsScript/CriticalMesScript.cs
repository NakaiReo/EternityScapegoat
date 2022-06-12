using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalMesScript : MonoBehaviour {

	[SerializeField]
	bool Move;

	SpriteRenderer spriteRenderer;

	Vector2 startPos;
	float colorAlpha = 1f;

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		startPos = transform.position;
	}

	void Update () {
		if (Move == true)
		{
			transform.Translate(0, 2f * Time.deltaTime, 0);
			if (transform.position.y - startPos.y > 1.0f)
			{
				colorAlpha -= 0.02f;
			}
			if (colorAlpha <= 0)
			{
				Destroy(gameObject);
			}
		}
		else
		{
			colorAlpha -= 0.25f * Time.deltaTime;
			if (colorAlpha <= 0)
			{
				Destroy(gameObject);
			}
		}
		spriteRenderer.color = new Color(1, 1, 1, colorAlpha);
	}
}
