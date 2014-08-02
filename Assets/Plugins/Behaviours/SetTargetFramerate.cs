using UnityEngine;

namespace Assets.Plugins.Behaviours
{
    public class SetTargetFramerate : MonoBehaviour
    {
        [SerializeField] private int _targetFramerate = 60;

        private void Awake()
        {
            Application.targetFrameRate = _targetFramerate;
        }
    }
}