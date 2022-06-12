using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButttonMove : MonoBehaviour
{
	Button button;

    void Start()
    {
		button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
		button.Select();
    }
}
