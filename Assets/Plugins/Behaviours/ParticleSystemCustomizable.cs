using UnityEngine;

namespace Assets.Plugins.Behaviours
{
    [RequireComponent(typeof (ParticleSystem))]
    public class ParticleSystemCustomizable : MonoBehaviour
    {
        [SerializeField] private bool _isAutodestruct;
        [SerializeField] private float _playbackSpeed = 1.0f;

        private void Awake()
        {
            particleSystem.playbackSpeed = _playbackSpeed;
        }

        private void Update()
        {
            if (_isAutodestruct && !particleSystem.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}