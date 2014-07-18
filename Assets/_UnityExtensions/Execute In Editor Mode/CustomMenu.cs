using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class CustomMenu
{
	[MenuItem("Common Utils/Create Child Object #&n")]
	private static void CreateChildbject()
	{
		if (Selection.gameObjects.Length == 0)
		{
			Debug.LogWarning("empty selection for created childs");
			return;
		}
		
		List<GameObject> selectionAfter = new List<GameObject>();
		for (int i = 0; i < Selection.gameObjects.Length; i++)
		{
			var go = new GameObject(Selection.activeTransform == null ? "GameObject" : "ChildGameObject");
			go.transform.parent = Selection.gameObjects[i].transform;
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;
			selectionAfter.Add(go);
		}
		
		Selection.objects = selectionAfter.ToArray();
	}
}
