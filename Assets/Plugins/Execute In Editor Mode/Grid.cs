using System.Linq;
using UnityEngine;

namespace Assets.Plugins
{
    [ExecuteInEditMode]
    public class Grid : MonoBehaviour
    {
        [SerializeField] private Vector3 _vector;
        [SerializeField] private bool _useSort;
        [SerializeField] private bool _moveNow;

        private void Update()
        {
            if (_moveNow)
            {
                _moveNow = false;
                MoveChild();
            }
        }

        private void MoveChild()
        {
            var childGameObjects =
                gameObject.GetComponentsInChildren(typeof (Transform)).Where(a => a.transform != transform).ToList();
            
            if (_useSort)
            {
                childGameObjects.Sort((a, b) => a.name.CompareTo(b.name));
            }

            for (int i = 0; i < childGameObjects.Count; i++)
            {
                childGameObjects[i].transform.localPosition = _vector * i;
            }
        }
    }
}


