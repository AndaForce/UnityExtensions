﻿using System.Collections.Generic;
using System.Linq;
using Assets.Plugins.Helpers;
using UnityEngine;

namespace Assets.Plugins.Components
{
    public class GestureInput : MonoBehaviour
    {
        [SerializeField] private float _horizontalLimiterPercent = 5.0f;
        [SerializeField] private float _verticalLimiterPercent = 5.0f;
        private Vector3 _anchorPosition;
        private static readonly List<GestureDirection> Gestures = new List<GestureDirection>();
        private static float _screenPercentHeight;
        private static float _screenPercentWidth;

        private static bool _isInitialized;

        #region GameCycle

        private void Awake()
        {
            CheckUniqueExsistance();

            CalcScreenPercentLimiters();
        }

        private void Update()
        {
            UpdateGestures();
        }

        #endregion

        #region Public methods

        public static void CreateInputObject(float horizontalLimiterPercent = 5.0f, float verticalLimitePercent = 5.0f)
        {
            if (!_isInitialized)
            {
                var go = new GameObject();
                go.AddComponent(typeof (GestureInput));
                go.name = "Gesture Input";
            }
        }

        public static bool IsGesture(GestureDirection gesture)
        {
            return Gestures.Contains(gesture);
        }

        public void SetupLimiters(float horizontalPercent, float verticalPercent)
        {
            _horizontalLimiterPercent = horizontalPercent;
            _verticalLimiterPercent = verticalPercent;

            CalcScreenPercentLimiters();
        }

        public static GestureDirection DetermineGesture(Vector3 from, Vector3 to, float limiterPercent)
        {
            var result = GestureDirection.None;
            if ((from.y - to.y) >= _screenPercentHeight)
            {
                result = GestureDirection.Down;
            }
            else if ((from.y - to.y) <= -_screenPercentHeight)
            {
                result = GestureDirection.Up;
            }
            else if ((from.x - to.x) >= _screenPercentWidth)
            {
                result = GestureDirection.Right;
            }
            else if ((from.x - to.x) <= -_screenPercentWidth)
            {
                result = GestureDirection.Left;
            }

            return result;
        }

        #endregion

        #region Private methods

        private void CheckUniqueExsistance()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void CalcScreenPercentLimiters()
        {
            _screenPercentWidth = Screen.width * (_horizontalLimiterPercent / 100.0f);
            _screenPercentHeight = Screen.height * (_verticalLimiterPercent / 100.0f);
        }

        private void UpdateGestures()
        {
            Gestures.Clear();

            if (MouseOrTouchInput.IsFirstDown())
            {
                _anchorPosition = MouseOrTouchInput.GetCursorPosition();
            }
            else
            {
                if (MouseOrTouchInput.IsDown())
                {
                    // Vertical
                    if ((_anchorPosition.y - MouseOrTouchInput.GetCursorPosition().y) >= _screenPercentHeight)
                    {
                        Gestures.Add(GestureDirection.Down);
                    }
                    else if ((_anchorPosition.y - MouseOrTouchInput.GetCursorPosition().y) <= -_screenPercentHeight)
                    {
                        Gestures.Add(GestureDirection.Up);
                    }

                    // Horizontal
                    if ((_anchorPosition.x - MouseOrTouchInput.GetCursorPosition().x) >= _screenPercentWidth)
                    {
                        Gestures.Add(GestureDirection.Left);
                    }
                    else if ((_anchorPosition.x - MouseOrTouchInput.GetCursorPosition().x) <= -_screenPercentWidth)
                    {
                        Gestures.Add(GestureDirection.Right);
                    }

                    if (Gestures.Any())
                    {
                        _anchorPosition = MouseOrTouchInput.GetCursorPosition();
                    }
                }
            }

            if (!Gestures.Any())
            {
                Gestures.Add(GestureDirection.None);
            }
        }

        #endregion
    }

    public enum GestureDirection
    {
        Down,
        Up,
        Left,
        Right,
        None
    }
}