using System;
using System.Collections;
using UnityEngine;

namespace Assets.Plugins.Components.ElapsedTimerComponent.Test
{
    public class ElapsedTimerTest : MonoBehaviour
    {
        [SerializeField] private TextMesh _meshLog;
        private int _log1Value;


        private void Start()
        {
            ElapsedTimer.RegisterNewTask("LogValue", 1.0f, LogValue);
            ElapsedTimer.RegisterNewTask("MeshTimeLog", 1.0f,
                () => _meshLog.text = DateTime.Now.ToLongTimeString());

            StartCoroutine(SwitchOffTimer());
        }

        private IEnumerator SwitchOffTimer()
        {
            yield return new WaitForSeconds(2.0f);

            ElapsedTimer.RemoveTask("LogValue");
        }

        private void LogValue()
        {
            _log1Value += 1;
            Debug.Log(String.Format("[Log1]: {0}", _log1Value));
        }
    }
}