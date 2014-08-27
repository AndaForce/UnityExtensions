using UnityEngine;

namespace Assets.Plugins.Helpers.GameObjectPull.Test
{
    public class PullController : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _initialCount = 10;
        [SerializeField] private int _maximumCount = 10;
        [SerializeField] private Vector3 _spawnPoint;
        [SerializeField] private Vector3 _movementPoint;
        private CubePull _cubePull;

        private void Start()
        {
            _cubePull = new CubePull(transform, _prefab);
            _cubePull.ReinitializePull(_initialCount, _maximumCount);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var obj = _cubePull.GetObjectFromPull();
                if (obj != null)
                {
                    obj.SetAtPosition(_spawnPoint);
                    obj.MoveTo(_movementPoint, 3.0f);
                }
            }
        }

        public class CubePull : AbstractPull<PullElement>
        {
            public CubePull(Transform parentTransform, GameObject prefab) : base(parentTransform, prefab)
            {
            }

            public override AbstractPullElement CreateGameObject(GameObject prefab)
            {
                var go = (GameObject) Instantiate(prefab);
                return go.GetComponent<PullElement>();
            }
        }
    }
}