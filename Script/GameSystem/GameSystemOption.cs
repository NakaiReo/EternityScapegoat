using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSystemOption : MonoBehaviour
{
	[SerializeField] Camera MainCameraObject;
	[SerializeField] Text FOV_Text;
	[SerializeField] Slider FOV_Slider;
	[SerializeField] Text OffsetX_Text;
	[SerializeField] Slider OffsetX_Slider;
	[SerializeField] Text OffsetY_Text;
	[SerializeField] Slider OffsetY_Slider;

	CameraController cameraController;

	void Start()
    {
		LoadOption();
		if (MainCameraObject) cameraController = MainCameraObject.GetComponent<CameraController>();
    }

    public void LoadOption()
	{
		SaveDataSystem.load();
		FOV_Slider.value = SaveDataSystem.saveData.cameraFov;
		OffsetX_Slider.value = SaveDataSystem.saveData.cameraOffsetX;
		OffsetY_Slider.value = SaveDataSystem.saveData.cameraOffsetY;
		TextValueChange();
	}

	public void CallSliderValueChange()
	{
		TextValueChange();
		SaveDataSystem.saveData.cameraFov = FOV_Slider.value;
		SaveDataSystem.saveData.cameraOffsetX = OffsetX_Slider.value;
		SaveDataSystem.saveData.cameraOffsetY = OffsetY_Slider.value;
		SaveDataSystem.Save();
		if (MainCameraObject)
		{
			cameraController = MainCameraObject.GetComponent<CameraController>();
			MainCameraObject.orthographicSize = FOV_Slider.value;
			cameraController.offsetX = OffsetX_Slider.value;
			cameraController.offsetY = OffsetY_Slider.value;
		}
	}

	public void TextValueChange()
	{
		FOV_Text.text = "FOV : " + FOV_Slider.value.ToString("F1");
		OffsetX_Text.text = "Offset X : " + OffsetX_Slider.value.ToString("F1");
		OffsetY_Text.text = "Offset Y : " + OffsetY_Slider.value.ToString("F1");
	}
}
