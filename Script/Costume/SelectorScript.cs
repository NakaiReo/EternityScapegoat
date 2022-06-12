using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorScript : MonoBehaviour
{
	[SerializeField] int maxType;
	[SerializeField] public int selectID;

	[Space(15)]
	[SerializeField] CostumePreview costumePreview;

	public void ChangeID(int amount)
	{
		selectID += amount;
		if (selectID < 1) selectID = maxType;
		if (selectID > maxType) selectID = 1;
		transform.Find("SelectID").GetComponent<Text>().text = selectID.ToString();
		ChangeCostumePreview();
	}

	public void SetID(int amount)
	{
		selectID = amount;
		if (selectID < 1) selectID = maxType;
		if (selectID > maxType) selectID = 1;
		transform.Find("SelectID").GetComponent<Text>().text = selectID.ToString();
		ChangeCostumePreview();
	}

	public void ChangeCostumePreview()
	{
		costumePreview.ChangeCostume();
	}
}
