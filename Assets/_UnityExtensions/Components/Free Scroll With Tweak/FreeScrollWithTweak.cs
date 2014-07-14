using System;
using System.Collections.Generic;
using System.Linq;
using Assets._UnityExtensions.Helpers;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    public class FreeScrollWithTweak : MonoBehaviour
    {
        [SerializeField] protected Transform CameraTransform;
        [SerializeField] protected List<float> PositionSteps = new List<float>();
        [SerializeField] private float _userScrollSpeedMultiplier = 0.5f;
        [SerializeField] private float _tweakSpeed = 2.0f;
        [SerializeField] private iTween.EaseType _easeType = iTween.EaseType.easeOutBack;
        [SerializeField] private float _timeThresholdForQuickSwap = 0.3f;
        [SerializeField] private float _gestureThresholdPercent = 5.0f;
        public Action<float> OnMove;

        private Vector3 _basePosition;
        private Vector3 _oldPosition;
        private Vector3 _newPosition;

        // static для переноса значения между сценами
        private int _currentPositionInArray = 0;
        private bool _isDrag;
        private float _elapsedTime;

        public bool IsUserScrollEnabled;

        #region Game Cycle

        protected void Update()
        {
            if (IsUserScrollEnabled)
            {
                UpdateUserScroll();

                _elapsedTime += Time.deltaTime;
            }
        }

        #endregion

        #region Scroll Logic Methods

        private void UpdateUserScroll()
        {
            // First down
            if (MouseOrTouchInput.IsFirstDown())
            {
                _basePosition = CameraTransform.localPosition;
                _oldPosition = MouseOrTouchInput.GetCursorPosition();

                _isDrag = true;
                _elapsedTime = 0;

                iTween.Stop(gameObject);
            }

            // Up
            if (MouseOrTouchInput.IsUp() && _isDrag)
            {
                _isDrag = false;

                if (_elapsedTime < _timeThresholdForQuickSwap)
                {
                    GestureDirection gd =
                        GestureInput.DetermineGesture(
                            new Vector3(_oldPosition.x, 0, 0),
                            new Vector3(MouseOrTouchInput.GetCursorPosition().x, 0, 0),
                            _gestureThresholdPercent);

                    switch (gd)
                    {
                        case GestureDirection.Left:
                            _currentPositionInArray = Mathf.Clamp(_currentPositionInArray - 1, 0, PositionSteps.Count - 1);
                            break;
                        case GestureDirection.Right:
                            _currentPositionInArray = Mathf.Clamp(_currentPositionInArray + 1, 0, PositionSteps.Count - 1);
                            break;
                    }
                    TweakPositionToId(_currentPositionInArray);
                }
                else
                {
                    // Доводит до ближайшей. 
                    // Важно: доводит даже до той ближайшей, которая дальше чем следующая/предыдущая (если слишком сильный рывок)
                    // Можно запретить, но не стоит, т.к. осуществить такой рывок невозможно с _userScrollSpeedMultiplier == 1
                    // И доводка до другого объекта не является ошибкой
                    TweakPosition(CameraTransform.localPosition.x);
                }
            }

            // Down move
            if (MouseOrTouchInput.IsDown() && _isDrag)
            {
                _newPosition = (_basePosition -
                                (MouseOrTouchInput.GetCursorPosition() - _oldPosition) * _userScrollSpeedMultiplier);

                UpdateCameraMove(_newPosition.x);
            }
        }

        private void TweakPosition(float currentValue)
        {
            //var closestPoint = PositionSteps.FindFirstClosest(currentValue);
            var closestPoint = FindClosestValuePair(PositionSteps, CameraTransform.localPosition.x).Value;

            iTween.Stop(gameObject);
            iTween.ValueTo(gameObject,
                iTween.Hash(
                    "from", CameraTransform.localPosition.x,
                    "to", closestPoint,
                    "easetype", _easeType,
                    "_speed", _tweakSpeed,
                    "onupdate", "UpdateCameraMove",
                    "oncomplete", "FinishCameraMove"));
        }

        private void TweakPositionToId(int positionId)
        {
            iTween.Stop(gameObject);
            iTween.ValueTo(gameObject,
                iTween.Hash(
                    "from", CameraTransform.localPosition.x,
                    "to", PositionSteps[positionId],
                    "easetype", _easeType,
                    "_speed", _tweakSpeed,
                    "onupdate", "UpdateCameraMove"));
        }

        protected int FindClosestPoint()
        {
            //return PositionSteps.FindFirstClosestId(CameraTransform.localPosition.x);
            return FindClosestValuePair(PositionSteps, CameraTransform.localPosition.x).Id;
        }

        protected void SetCurrentPosition(int position)
        {
            _currentPositionInArray = position;
        }

        #region iTween callbacks

        private void UpdateCameraMove(float value)
        {
            if (OnMove != null)
            {
                OnMove.Invoke(value);
            }

            CameraTransform.localPosition =
                new Vector3(value, CameraTransform.localPosition.y, CameraTransform.localPosition.z);
        }

        private void FinishCameraMove()
        {
            _currentPositionInArray = FindClosestPoint();
        }

        #endregion

        #endregion

        public static ValuePair FindClosestValuePair(List<float> list, float value)
        {
            var sortedList =
                list
                    .Select((a, position) => new ValuePair() {Value = a, Distance = Math.Abs(a - value), Id = position})
                    .ToList();

            sortedList.Sort(delegate(ValuePair x, ValuePair y)
            {
                if (x.Distance < y.Distance) return -1;
                if (x.Distance > y.Distance) return 1;
                return 0;
            });

            return sortedList[0];
        }

        public class ValuePair
        {
            public float Value;
            public int Id;
            public float Distance;
        }
    }
}