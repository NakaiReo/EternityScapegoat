using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinChange : MonoBehaviour
{
	[SerializeField] public bool loadCostume;
	[SerializeField] public int hatID;
	[SerializeField] public int hairID;
	[SerializeField] public int bodyID;

	[Space(25)]
	[SerializeField] SpriteRenderer baseSpriteRenderer;
	[SerializeField] SpriteRenderer hatSpriteRenderer;
	[SerializeField] SpriteRenderer hairSpriteRenderer;
	[SerializeField] SpriteRenderer bodySpriteRenderer;

	Sprite[] baseSprite = new Sprite[0];
	Sprite[] hatSprite = new Sprite[0];
	Sprite[] hairSprite = new Sprite[0];
	Sprite[] bodySprite = new Sprite[0];

	static Color32 spriteColor = new Color32(255, 255, 255, 255);

	void Awake()
	{
		SpriteChange();
		if (loadCostume) LoadCostumeData();
	}

	public void SpriteAnimation(int id)
	{
		DebugEX.LogMultiple(hatSprite[0], hairSprite[0], bodySprite[0]);
		DebugEX.LogMultiple(hatSprite.Length, hairSprite.Length, bodySprite.Length);

		baseSpriteRenderer.sprite = baseSprite[id];
		hatSpriteRenderer.sprite = hatSprite[id];
		hairSpriteRenderer.sprite = hairSprite[id];
		bodySpriteRenderer.sprite = bodySprite[id];

		baseSpriteRenderer.color = spriteColor;
		hatSpriteRenderer.color = spriteColor;
		hairSpriteRenderer.color = spriteColor;
		bodySpriteRenderer.color = spriteColor;
	}

	public void LoadCostumeData()
	{
		CostumeSaveData.load();
		hatID = CostumeSaveData.costumeData.hatData;
		hairID = CostumeSaveData.costumeData.hairData;
		bodyID = CostumeSaveData.costumeData.bodyData;
		SpriteChange();
	}

	public void SpriteChange()
	{
		baseSprite = Resources.LoadAll<Sprite>("PlayerSkin/BaseSkin");
		hatSprite = Resources.LoadAll<Sprite>("PlayerSkin/Hat/Hat_" + hatID);
		hairSprite = Resources.LoadAll<Sprite>("PlayerSkin/Hair/Hair_" + hairID);
		bodySprite = Resources.LoadAll<Sprite>("PlayerSkin/Body/Body_" + bodyID);
	}

	public void PlayerColor(Color32 color)
	{
		baseSpriteRenderer.color = color;
		hatSpriteRenderer.color = color;
		hairSpriteRenderer.color = color;
		bodySpriteRenderer.color = color;
	}

	private void OnValidate()
	{
		SpriteChange();
		baseSpriteRenderer.sprite = baseSprite[1];
		hatSpriteRenderer.sprite = hatSprite[1];
		hairSpriteRenderer.sprite = hairSprite[1];
		bodySpriteRenderer.sprite = bodySprite[1];
	}

	public void SpriteRendererEnabled(bool enabled)
	{
		baseSpriteRenderer.enabled = enabled;
		hatSpriteRenderer.enabled = enabled;
		hairSpriteRenderer.enabled = enabled;
		bodySpriteRenderer.enabled = enabled;
	}

	public void SpriteColor(Color32 color)
	{
		spriteColor = color;
	}
}