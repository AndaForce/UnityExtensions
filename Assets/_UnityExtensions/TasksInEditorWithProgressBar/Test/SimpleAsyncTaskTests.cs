using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._UnityExtensions.TasksInEditorWithProgressBar.Test
{
    public class SimpleAsyncTaskTests : MonoBehaviour {

        private void SimpleTask()
        {
            var k = Mathf.Sqrt(123321231312312434645634513124f);
        }
        private void ComplicatedAsyncTask(object inp)
        {
            var k = Mathf.Sqrt(int.Parse(inp.ToString()));
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Test Simple"))
            {
                var sp = new List<Action>();

                for (int i = 0; i < 10000; i++)
                {
                    sp.Add(SimpleTask);
                }

                SimpleAsyncTask.DoTaskWithProgressBar(sp);
            }

            if (GUILayout.Button("Test Advanced"))
            {
                var sp = new Action<object>[1000000];
                var obj = new object[1000000];
			
                for (int i = 0; i < 1000000; i++)
                {
                    sp[i] = ComplicatedAsyncTask;
                    obj[i] = i;
                }
			
                SimpleAsyncTask.DoTaskWithProgressBarWithParameters(sp, obj);
            }
        }
    }
}
