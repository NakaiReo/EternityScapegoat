using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickParticle : MonoBehaviour
{
	[SerializeField] GameObject Particle;

    void Update()
    {
		Vector2 ClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}
