using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostumePreview : MonoBehaviour
{
	[SerializeField] SelectorScript hatSelector;
	[SerializeField] SelectorScript hairSelector; 
	[SerializeField] SelectorScript bodySelector;
	[SerializeField] Image baseImage;
	[SerializeField] Image hatImage;
	[SerializeField] Image hairImage;
	[SerializeField] Image bodyImage;
	[HideInInspector] public Sprite[] baseSprites = new Sprite[0];
	[HideInInspector] public Sprite[] hatSprites = new Sprite[0];
	[HideInInspector] public Sprite[] hairSprites = new Sprite[0];
	[HideInInspector] public Sprite[] bodySprites = new Sprite[0];

	[HideInInspector] public int rotation = 0;
	static int[] rotationSpriteID = new int[8] { 1, 10, 13, 22, 19, 16, 7, 4 };

	void Start()
    {
		CostumeSaveData.load();
		hatSelector.SetID(CostumeSaveData.costumeData.hatData);
		hairSelector.SetID(CostumeSaveData.costumeData.hairData);
		bodySelector.SetID(CostumeSaveData.costumeData.bodyData);

		ChangeCostume();
	}

	public void LoadSprite()
	{
		baseSprites = Resources.LoadAll<Sprite>("PlayerSkin/BaseSkin");
		hatSprites  = Resources.LoadAll<Sprite>("PlayerSkin/Hat/Hat_" + hatSelector.selectID);
		hairSprites = Resources.LoadAll<Sprite>("PlayerSkin/Hair/Hair_" + hairSelector.selectID);
		bodySprites = Resources.LoadAll<Sprite>("PlayerSkin/Body/Body_" + bodySelector.selectID);
	}

	public void ChangeCostume()
	{
		LoadSprite();
		int SpriteID = rotationSpriteID[rotation];
		baseImage.sprite = baseSprites[SpriteID];
		hatImage.sprite = hatSprites[SpriteID];
		hairImage.sprite = hairSprites[SpriteID];
		bodyImage.sprite = bodySprites[SpriteID];
	}

	public void RotationPreview(int amount)
	{
		rotation += amount;
		if (rotation < 0) rotation = 7;
		if (rotation > 7) rotation = 0;
		ChangeCostume();
	}

	public void SaveCostume()
	{
		CostumeSaveData.costumeData.hatData = hatSelector.selectID;
		CostumeSaveData.costumeData.hairData = hairSelector.selectID;
		CostumeSaveData.costumeData.bodyData = bodySelector.selectID;
		CostumeSaveData.Save();
		Debug.Log("Save Costume!!");
	}
}
