using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Plugins.Components.ElapsedTimerComponent
{
    public class ElapsedTimer : MonoBehaviour
    {
        private bool _isEnabled = true;
        private static ElapsedTimer _instance;
        private Dictionary<String, ElapsedTimerTask> _tasks = new Dictionary<string, ElapsedTimerTask>();

        #region Public methods

        public static void SetEnabled(bool enableState)
        {
            GetInstance()._isEnabled = enableState;
        }

        public static void RegisterNewTask(String identifier, float timerInterval, Action timedAction)
        {
            if (GetInstance()._tasks.ContainsKey(identifier))
            {
                Debug.LogError(
                    String.Format(
                        "[ElapsedTimer] You are trying to register task with identifier which are already presented: [{0}]",
                        identifier));

                return;
            }

            GetInstance()._tasks.Add(identifier, new ElapsedTimerTask(timerInterval, timedAction));

            Debug.Log(
                String.Format(
                    "[ElapsedTimer] Registered new timed task: [{0}]",
                    identifier));
        }

        public static void ChangeTaskActivity(String identifier, bool newActiveState)
        {
            if (GetInstance().Contains(identifier))
            {
                GetInstance()._tasks[identifier].IsActive = newActiveState;
            }
        }

        public static void ChangeTask(String identifier, float newTimerInterval, Action newTimedAction)
        {
            if (GetInstance().Contains(identifier))
            {
                GetInstance()._tasks[identifier].TimeInterval = newTimerInterval;
                GetInstance()._tasks[identifier].TimedAction = newTimedAction;

                Debug.Log(
                    String.Format(
                        "[ElapsedTimer] Timed task changed: [{0}]",
                        identifier));
            }
        }

        public static void RemoveTask(String identifier)
        {
            if (GetInstance().Contains(identifier))
            {
                GetInstance()._tasks.Remove(identifier);

                Debug.Log(
                    String.Format(
                        "[ElapsedTimer] Timed task removed: [{0}]",
                        identifier));
            }
        }

        #endregion

        #region Private methods

        private static ElapsedTimer GetInstance()
        {
            if (_instance != null)
            {
                return _instance;
            }
            throw new Exception("No ElapsedTimer game object presented");
        }

        private bool Contains(String identifier)
        {
            if (_tasks.ContainsKey(identifier))
            {
                return true;
            }

            Debug.LogError(
                String.Format(
                    "[ElapsedTimer] Identifier [{0}] is not presented",
                    identifier));

            return false;
        }

        #endregion

        #region GameCycle

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (!_isEnabled) return;

            foreach (var elapsedTimerTask in _tasks)
            {
                if (elapsedTimerTask.Value.IsActive)
                {
                    elapsedTimerTask.Value.UpdateTime(Time.deltaTime);
                }
            }
        }

        #endregion
    }

    public class ElapsedTimerTask
    {
        public bool IsActive = true;
        public float ElapsedTime { get; private set; }
        public float TimeInterval;
        public Action TimedAction;

        public ElapsedTimerTask(float timeInterval, Action timedAction)
        {
            TimeInterval = timeInterval;
            TimedAction = timedAction;
        }

        public void UpdateTime(float delthaTime)
        {
            ElapsedTime += delthaTime;
            if (ElapsedTime >= TimeInterval)
            {
                ElapsedTime = 0;
                if (TimedAction != null)
                {
                    TimedAction.Invoke();
                }
            }
        }

        public bool IsTimeHasCome()
        {
            return ElapsedTime >= TimeInterval;
        }
    }
}