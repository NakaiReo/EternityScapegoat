using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonImage : MonoBehaviour {

	Toggle toggle;
	Image toggleImage;
	

	[SerializeField]
	Sprite OnImage;
	[SerializeField]
	Sprite OffImage;

	void Start () {
	}
	
	public void ChangeImage() {
		toggle = GetComponent<Toggle>();
		toggleImage = toggle.transform.GetChild(0).GetComponent<Image>();
		toggleImage.sprite = toggle.isOn ? OnImage : OffImage;
	}
}
