﻿using System;
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

        public static void SetEnabled(bool enableState)
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
            RegisterNewTask(identifier, new ElapsedTimerTask(timerInterval, timedAction));
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

            _tasks = _tasks.Where(a => a.Value.IsAlive).ToDictionary(a => a.Key, b => b.Value);
        }

        #endregion
    }

    public class ElapsedTimerTask
    {
        public bool IsActive = true;
        public float ElapsedTime { get; private set; }
        public float TimeInterval;
        public Action TimedAction;
        public bool IsAlive { get; protected set; }

        public ElapsedTimerTask(float timeInterval, Action timedAction)
        {
            TimeInterval = timeInterval;
            TimedAction = timedAction;

            IsAlive = true;
        }

        public virtual bool UpdateTime(float delthaTime)
        {
            ElapsedTime += delthaTime;
            if (ElapsedTime >= TimeInterval)
            {
                ElapsedTime = 0;
                if (TimedAction != null)
                {
                    TimedAction.Invoke();

                    return true;
                }
            }

            return false;
        }
    }

    public class RestrictedTimerTask : ElapsedTimerTask
    {
        private int _restrictionTargetCount;
        private int _fireCount;

        public RestrictedTimerTask(float timeInterval, Action timedAction, int restrictionTargetCount) : base(timeInterval, timedAction)
        {
            _restrictionTargetCount = restrictionTargetCount;
        }

        public override bool UpdateTime(float delthaTime)
        {
            if (base.UpdateTime(delthaTime))
            {
                _fireCount += 1;
                if (_fireCount >= _restrictionTargetCount)
                {
                    IsAlive = false;
                }

                return true;
            }

            return false;
        }
    }

    public enum ElapsedTimerTaskType
    {
        Endless,
        Restricted
    }
}