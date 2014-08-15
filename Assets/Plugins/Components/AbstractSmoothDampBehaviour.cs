using System.Collections;
using UnityEngine;

namespace Assets.Plugins.Components
{
    public abstract class AbstractSmoothDampBehaviour : MonoBehaviour
    {
        private const float Deltha = 1.0f / 60.0f;

        protected float CurrentSmoothedValue { get; private set; }
        protected float TargetValue { get; private set; }
        protected float SmoothTime = 1.0f;
        private float _chaseVelocity;

        protected void Start()
        {
            CurrentSmoothedValue = 0.0f;
            TargetValue = 0.0f;

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
                        SmoothTime,
                        Mathf.Infinity,
                        Deltha);


                yield return 0;
            }
        }

        public void SmoothToTargetValue(float newValue)
        {
            TargetValue = newValue;
        }
    }
}