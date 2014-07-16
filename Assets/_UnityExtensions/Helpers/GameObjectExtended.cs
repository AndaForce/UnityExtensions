using UnityEngine;
using System;
using System.Collections;

public class GameObjectExtended
{

	/// <summary>
	/// Instantiates GameObject as child.
	/// </summary>
	/// <returns>GameObject</returns>
	/// <param name="parent">Parent.</param>
	/// <param name="objectToInstantiate">Object to instantiate. Can be null for emty GameObject</param>
	/// <param name="nameOfGameObject">Name of instantiated GameObject</param>
	public static GameObject InstantiateAsChild(Transform parent, 
	                                            GameObject objectToInstantiate,
	                                            string nameOfGameObject)
	{
		return InstantiateAsChild(parent, objectToInstantiate, nameOfGameObject, Vector3.zero, Vector3.one, Quaternion.identity);
	}

	/// <summary>
	/// Instantiates GameObject as child.
	/// </summary>
	/// <returns>The as child.</returns>
	/// <param name="parent">Parent.</param>
	/// <param name="objectToInstantiate">Object to instantiate. Can be null for emty GameObject</param>
	/// <param name="nameOfGameObject">Name of instantiated GameObject</param>
	/// <param name="localPosition">Local position.</param>
	/// <param name="localScale">Local scale.</param>
	/// <param name="localRotation">Local rotation.</param>
	public static GameObject InstantiateAsChild(Transform parent, 
	                                            GameObject objectToInstantiate, 
	                                            string nameOfGameObject,
	                                            Vector3 localPosition,
	                                            Vector3 localScale, 
	                                            Quaternion localRotation)
	{
		GameObject go = null;
		if (objectToInstantiate != null)
			go = GameObject.Instantiate(objectToInstantiate) as GameObject;
		else
			go = new GameObject();
		go.name = nameOfGameObject;
		go.transform.parent = parent;
		go.transform.localPosition = localPosition;
		go.transform.localScale = localScale;
		go.transform.localRotation = localRotation;

		return go;
	}
}
