using System.Collections;
using UnityEngine;

namespace Assets.Plugins.Behaviours
{
    public class SmoothDampBehaviour : MonoBehaviour
    {
        protected float CurrentSmoothedValue { get; private set; }
        protected float TargetValue { get; private set; }
        protected float CurrentSmoothTime { get; private set; }

        private float _chaseVelocity;
        private const float Deltha = 1.0f / 60.0f;

        protected void Start()
        {
            StartCoroutine(MoveAnimatorSpeedValue());
        }

        private IEnumerator MoveAnimatorSpeedValue()
        {
            while (true)
            {
                CurrentSmoothedValue =
                    Mathf.SmoothDamp(
                        CurrentSmoothedValue,
                        TargetValue,
                        ref _chaseVelocity,
                        CurrentSmoothTime,
                        Mathf.Infinity,
                        Deltha);

                yield return 0;
            }
        }

        public void SmoothToTargetValue(float newSpeed)
        {
            TargetValue = newSpeed;
        }

        public void SetSmoothTime(float smoothTime)
        {
            CurrentSmoothTime = smoothTime;
        }
    }
}