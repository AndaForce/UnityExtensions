using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

class SimpleAsyncTask: Editor 
{

	public delegate void SimpleDelegate();
	public delegate void CustomDelegate(object obj);



	public static void DoTaskWithProgressBar(List<SimpleDelegate> delegateList) 
	{
		for (int i = 0; i < delegateList.Count; i++)
		{
			EditorUtility.DisplayProgressBar("Do Some Work", "wait a little", (float)i / ((float)delegateList.Count - 1f));
			
			delegateList[i]();
		}
		
		EditorUtility.ClearProgressBar();
	}

	public static void DoTaskWithProgressBarWithParameters(List<CustomDelegate> delegateList, List<object> parametersList)
	{
		for (int i = 0; i < delegateList.Count; i++)
		{
			EditorUtility.DisplayProgressBar("Do Some Work", "wait a little", (float)i / ((float)delegateList.Count - 1f));
			
			delegateList[i](parametersList[i]);
		}
		
		EditorUtility.ClearProgressBar();
	}
}