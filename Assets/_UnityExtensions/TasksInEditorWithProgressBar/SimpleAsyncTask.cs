using System;
using System.Collections.Generic;
using UnityEditor;

namespace Assets._UnityExtensions.TasksInEditorWithProgressBar
{
    public class SimpleAsyncTask: Editor 
    {
        public static void DoTaskWithProgressBar(List<Action> delegateList) 
        {
            for (int i = 0; i < delegateList.Count; i++)
            {
                EditorUtility.DisplayProgressBar("Do Some Work", "wait a little", i / ((float)delegateList.Count - 1));
			
                delegateList[i].Invoke();
            }
		
            EditorUtility.ClearProgressBar();
        }

        public static void DoTaskWithProgressBarWithParameters<T>(Action<T>[] delegateList, T[] parametersList)
        {
            for (int i = 0; i < delegateList.Length; i++)
            {
                EditorUtility.DisplayProgressBar("Do Some Work", "wait a little", i / ((float)delegateList.Length - 1));

                delegateList[i].Invoke(parametersList[i]);
            }
		
            EditorUtility.ClearProgressBar();
        }
    }
}
