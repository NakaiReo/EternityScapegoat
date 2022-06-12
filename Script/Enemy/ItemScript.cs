using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour {

	Sprite[] ItemSpriteData;
	SpriteRenderer ItemSprite;

	int itemID = 0;
	float[] ItemLootTable = { 33.3f, 33.3f, 33.4f };

	float itemRemoveTime = 25f;

	void Start () {
		ItemSpriteData = Resources.LoadAll<Sprite>("Item");
		ItemSprite = GetComponent<SpriteRenderer>();

		float percentRand = Random.Range(0f, 100f);
		float percentSum = 0;
		for(int i = 0; i < ItemLootTable.Length; i++)
		{
			percentSum += ItemLootTable[i];
			if(percentRand <= percentSum)
			{
				itemID = i;
				break;
			}
		}

		ItemSprite.sprite = ItemSpriteData[itemID];
	}
	
	void Update () {
		if(Mathf.Approximately(Time.timeScale, 0f)) return;

		itemRemoveTime -= Time.deltaTime;
		transform.Rotate(0, 0.5f, 0);
		if(itemRemoveTime <= 7.5f)
		{
			float few = itemRemoveTime % 1.0f;
			if(few >= 0.5f)
			{
				ItemSprite.enabled = false;
			}
			else
			{
				ItemSprite.enabled = true;
			}
		}
		if(itemRemoveTime <= 0)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Player")
		{
			ItemEffect(itemID);
			Destroy(gameObject);
		}
	}

	private void ItemEffect(int id)
	{
		switch (id)
		{
			case 0:
				GameObject.Find("Player").GetComponent<PlayerShotBullet>().MaxAmmo();
				break;
			case 1:
				ItemUI.SetDamagePlusTime(25);
				break;
			case 2:
				ItemUI.SetPointPlusTime(25);
				break;
		}
	}
}