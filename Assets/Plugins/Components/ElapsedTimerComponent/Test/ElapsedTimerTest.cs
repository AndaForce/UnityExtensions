using System;
using UnityEngine;

namespace Assets.Plugins.Components.ElapsedTimerComponent.Test
{
    public class ElapsedTimerTest : MonoBehaviour
    {
        [SerializeField] private TextMesh _meshLog;
        private int _log1Value;


        private void Start()
        {
            // Обычное бесконечное задание
            ElapsedTimer.RegisterNewTask("MeshTimeLog", 1.0f,
                () => _meshLog.text = DateTime.Now.ToLongTimeString());

            // Задание, отменяющееся после 10 срабатываний
            ElapsedTimer.RegisterNewTask("Restricted", new RestrictedTimerTask(1.0f, LogValue, 10, 0.0f));

            // Задание, срабатывающее 2 раза через 5 секунд
            ElapsedTimer.RegisterNewTask("Delayed",
                new RestrictedTimerTask(1.0f, () => Debug.Log("Delayed task"), 3, 5.0f));
        }

        private void LogValue()
        {
            _log1Value += 1;
            Debug.Log(String.Format("[Log1]: {0}", _log1Value));
        }
    }
}