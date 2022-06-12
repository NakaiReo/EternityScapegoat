using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryObject : MonoBehaviour
{
	[SerializeField] float DestoryTime;
    void Start()
    {
		Destroy(gameObject, DestoryTime);
    }
}
