using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Plugins.Components.ElapsedTimerComponent
{
    public class ElapsedTimer : MonoBehaviour
    {
        private bool _isEnabled = true;
        private static ElapsedTimer _instance;
        private Dictionary<String, ElapsedTimerTask> _tasks = new Dictionary<string, ElapsedTimerTask>();

        #region Public methods

        public static void SetTimerEnabled(bool enableState)
        {
            GetInstance()._isEnabled = enableState;
        }

        public static void RegisterNewTask(String identifier, ElapsedTimerTask task)
        {
            if (GetInstance()._tasks.ContainsKey(identifier))
            {
                Debug.LogError(
                    String.Format(
                        "[ElapsedTimer] You are trying to register task with identifier which are already presented: [{0}]",
                        identifier));

                return;
            }

            GetInstance()._tasks.Add(identifier, task);

            Debug.Log(
                String.Format(
                    "[ElapsedTimer] Registered new {0}: [{1}]",
                    task.GetType(),
                    identifier));
        }

        public static void RegisterNewTask(String identifier, float timerInterval, Action timedAction)
        {
            RegisterNewTask(identifier, new ElapsedTimerTask(timerInterval, timedAction, 0.0f, 0));
        }

        public static void RegisterNewTask(String identifier, float timerInterval, Action timedAction, float delay,
            int firingRestriction)
        {
            RegisterNewTask(identifier, new ElapsedTimerTask(timerInterval, timedAction, delay, firingRestriction));
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

        public static ElapsedTimerTask GetTask(String identifier)
        {
            return GetInstance().Contains(identifier) 
                ? GetInstance()._tasks[identifier] 
                : null;
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

            if (_tasks.Any(a => !a.Value.IsAlive))
            {
                _tasks = _tasks.Where(a => a.Value.IsAlive).ToDictionary(a => a.Key, b => b.Value);
            }
        }

        #endregion
    }

    public class ElapsedTimerTask
    {
        public bool IsActive = true;
        public float TimeInterval;
        public Action TimedAction;
        public float DelayTime { get; private set; }
        public float ElapsedTime { get; private set; }
        public bool IsAlive { get; private set; }
        public int FiringRestrictionCount { get; private set; }
        public int FiresCount { get; private set; }

        public ElapsedTimerTask(float timeInterval, Action timedAction, float delayTime, int restrictionCountCount)
        {
            TimeInterval = timeInterval;
            TimedAction = timedAction;
            DelayTime = delayTime;
            FiringRestrictionCount = restrictionCountCount;

            IsAlive = true;
        }

        public ElapsedTimerTask(float timeInterval, Action timedAction)
        {
            TimeInterval = timeInterval;
            TimedAction = timedAction;
            DelayTime = 0.0f;
            FiringRestrictionCount = 0;

            IsAlive = true;
        }


        public virtual bool UpdateTime(float delthaTime)
        {
            ElapsedTime += delthaTime;
            if ((ElapsedTime >= DelayTime && DelayTime != 0.0f)
                || (ElapsedTime >= TimeInterval && DelayTime == 0.0f))
            {
                ElapsedTime = 0.0f;
                DelayTime = 0.0f;

                FiresCount += 1;
                if (FiresCount >= FiringRestrictionCount && FiringRestrictionCount != 0)
                {
                    IsAlive = false;
                }

                InvokeAction();
                return true;
            }

            return false;
        }

        private void InvokeAction()
        {
            if (TimedAction != null)
            {
                TimedAction.Invoke();
            }
        }
    }

    public enum ElapsedTimerTaskType
    {
        Endless,
        Restricted
    }
}