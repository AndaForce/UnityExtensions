using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Plugins.Components.ElapsedTimerComponent
{
	public class ElapsedTimer : MonoBehaviour
	{
		[SerializeField]private bool _isLoggingEnabled = true;
		private bool _isEnabled = true;
		private static ElapsedTimer _instance;
		private Dictionary<String, ElapsedTimerTask> _tasks = new Dictionary<string, ElapsedTimerTask>();
		private Dictionary<String, ElapsedTimerTask> _newTasks = new Dictionary<string, ElapsedTimerTask>();
		
		#region Public methods
		
		public static void SetTimerEnabled(bool enableState)
		{
			GetInstance()._isEnabled = enableState;
		}
		
		public static void SetLoggingEnabled(bool loggingState)
		{
			GetInstance()._isLoggingEnabled = loggingState;
		}
		
		public static void RegisterNewTask(String identifier, ElapsedTimerTask task)
		{
			if (GetInstance()._tasks.ContainsKey(identifier))
			{
				Log (String.Format(
					"[ElapsedTimer] You are trying to register task with identifier which are already presented: [{0}]",
					identifier), true);
				
				return;
			}
			
			GetInstance()._newTasks.Add(identifier, task);
			
			Log (String.Format(
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
			else
			{
				ReportTaskIsNotFound (identifier);
			}
		}
		
		public static void ChangeTask(String identifier, float newTimerInterval, Action newTimedAction)
		{
			if (GetInstance().Contains(identifier))
			{
				GetInstance()._tasks[identifier].TimeInterval = newTimerInterval;
				GetInstance()._tasks[identifier].TimedAction = newTimedAction;
				
				Log (String.Format(
					"[ElapsedTimer] Timed task changed: [{0}]",
					identifier));    
			}
			else
			{
				ReportTaskIsNotFound (identifier);
			}
		}
		
		public static void RemoveTask(String identifier)
		{
			if (GetInstance().Contains(identifier))
			{
				GetInstance()._tasks.First(a=>a.Key == identifier).Value.MakeDead();
				
				Log(String.Format(
					"[ElapsedTimer] Timed task removed: [{0}]",
					identifier));
			}
			else
			{
				ReportTaskIsNotFound (identifier);
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
			return _tasks.ContainsKey(identifier);
		}
		
		static void ReportTaskIsNotFound (string identifier)
		{
			throw new Exception(String.Format("Identifier [{0}] is not presented", identifier));
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
			
			// Clear
			if (_tasks.Any(a => !a.Value.IsAlive))
			{
				_tasks = _tasks.Where(a => a.Value.IsAlive).ToDictionary(a => a.Key, b => b.Value);
			}
			
			// Add new
			foreach(var elapsedTimerTask in _newTasks)
			{
				_tasks.Add(elapsedTimerTask.Key, elapsedTimerTask.Value);
			}
			_newTasks.Clear();
		}
		
		#endregion
		
		#region Logging
		
		private static void Log(String message, bool isError = false)
		{
			if (!GetInstance()._isLoggingEnabled) return;
			
			var str = String.Format("[ElapsedTime] {0}", message);
			
			if (isError)
			{
				Debug.LogError(str);
			}
			else
			{
				Debug.Log(str);
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
		
		public void MakeDead()
		{
			IsActive = false;
			IsAlive = false;
		}
	}
	
	public enum ElapsedTimerTaskType
	{
		Endless,
		Restricted
	}
}