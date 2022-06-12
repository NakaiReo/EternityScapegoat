using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderToFillImage : MonoBehaviour {

	[SerializeField]
	Slider slider;
	Image image;

	private void Start()
	{
		image = GetComponent<Image>();
	}
	void Update () {
		image.fillAmount = slider.value;
	}
}
