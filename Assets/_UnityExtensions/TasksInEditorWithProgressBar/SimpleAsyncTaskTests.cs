using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleAsyncTaskTests : MonoBehaviour {

	private void SimpleTask()
	{
		var k = Mathf.Sqrt(123321231312312434645634513124f);
	}
	private void ComplicatedAsyncTask(object inp)
	{
		var k = Mathf.Sqrt(int.Parse(inp.ToString()));
	}

	public class obj1
	{
		public int k = 0;
	}

	private void OnGUI()
	{
		if (GUILayout.Button("test simple"))
		{
			List<SimpleAsyncTask.SimpleDelegate> sp = new List<SimpleAsyncTask.SimpleDelegate>();

			for (int i = 0; i < 1000000; i++)
				sp.Add(SimpleTask);

			SimpleAsyncTask.DoTaskWithProgressBar(sp);
		}

		if (GUILayout.Button("testadvanced"))
		{
			List<SimpleAsyncTask.CustomDelegate> sp = new List<SimpleAsyncTask.CustomDelegate>();
			List<object> obj = new List<object>();
			
			for (int i = 0; i < 1000000; i++)
			{
				sp.Add(ComplicatedAsyncTask);
				object kkk = i;
				obj.Add(kkk);
			}
			
			SimpleAsyncTask.DoTaskWithProgressBarWithParameters(sp, obj);
		}
	}
}
