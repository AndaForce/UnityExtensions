using System.Collections.Generic;
using System.Linq;
using Assets.Plugins.Helpers;
using UnityEngine;

namespace Assets.Plugins.Components
{
    public class GestureInput : MonoBehaviour
    {
        [SerializeField] private float _horizontalLimiterPercent = 5.0f;
        [SerializeField] private float _verticalLimiterPercent = 5.0f;
        private readonly List<GestureDirection> _gestures = new List<GestureDirection>();
        private Vector3 _anchorPosition;
        private float _screenPercentHeight;
        private float _screenPercentWidth;

        private static bool _isInitialized;
        private static GestureInput _instance;

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
            return _instance._gestures.Contains(gesture);
        }

        public static void SetupLimiters(float horizontalPercent, float verticalPercent)
        {
            _instance._horizontalLimiterPercent = horizontalPercent;
            _instance._verticalLimiterPercent = verticalPercent;

            _instance.CalcScreenPercentLimiters();
        }

        public static GestureDirection DetermineGesture(Vector3 from, Vector3 to, float limiterPercent)
        {
            var result = GestureDirection.None;
            if ((from.y - to.y) >= _instance._screenPercentHeight)
            {
                result = GestureDirection.Down;
            }
            else if ((from.y - to.y) <= -_instance._screenPercentHeight)
            {
                result = GestureDirection.Up;
            }
            else if ((from.x - to.x) >= _instance._screenPercentWidth)
            {
                result = GestureDirection.Right;
            }
            else if ((from.x - to.x) <= -_instance._screenPercentWidth)
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
                _instance = this;

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
            _gestures.Clear();

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
                        _gestures.Add(GestureDirection.Down);
                    }
                    else if ((_anchorPosition.y - MouseOrTouchInput.GetCursorPosition().y) <= -_screenPercentHeight)
                    {
                        _gestures.Add(GestureDirection.Up);
                    }

                    // Horizontal
                    if ((_anchorPosition.x - MouseOrTouchInput.GetCursorPosition().x) >= _screenPercentWidth)
                    {
                        _gestures.Add(GestureDirection.Left);
                    }
                    else if ((_anchorPosition.x - MouseOrTouchInput.GetCursorPosition().x) <= -_screenPercentWidth)
                    {
                        _gestures.Add(GestureDirection.Right);
                    }

                    if (_gestures.Any())
                    {
                        _anchorPosition = MouseOrTouchInput.GetCursorPosition();
                    }
                }
            }

            if (!_gestures.Any())
            {
                _gestures.Add(GestureDirection.None);
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