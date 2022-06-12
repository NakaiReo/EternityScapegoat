using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseCursorChange { 

	static Texture2D cursorTexture;
	static CursorMode cursorMode = CursorMode.Auto;
	static Vector2 hotSpot = new Vector2(31, 31);

	public static void ChangeMouseScope()
	{
		Cursor.visible = false;
		Cursor.SetCursor(null, Vector2.zero, cursorMode);
	}

	public static void ChangeDefaultMouse()
	{
		Cursor.visible = true;
		Cursor.SetCursor(null, Vector2.zero, cursorMode);
	}
}
