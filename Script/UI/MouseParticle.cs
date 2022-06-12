using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseParticle : MonoBehaviour
{

	[SerializeField] GameObject particleObject;
	static TrailRenderer trailRenderer;

	private void Start()
	{
		trailRenderer = particleObject.GetComponent<TrailRenderer>();
	}

	private void Update()
	{
		Vector3 ScreenWorldMousePosition = Input.mousePosition;
		ScreenWorldMousePosition.z = 10f;
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint(ScreenWorldMousePosition);

		particleObject.transform.position = mousePosition;
	}

	public static void Active(bool active)
	{
		trailRenderer.enabled = active;
	}
}
