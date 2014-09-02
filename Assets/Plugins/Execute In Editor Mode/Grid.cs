using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    [ExecuteInEditMode]
    public class Grid : MonoBehaviour
    {
        [SerializeField] private float _cellsWidth;
        [SerializeField] private float _cellsHeight;
        [SerializeField] private int _columnLimiter;
        [SerializeField] private bool _isDownDirection = true;

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
                gameObject.GetComponentsInChildren(typeof (Transform)).Where(
                    a => a.transform != transform && a.transform.parent == transform).ToList();

            var curentPos = Vector2.zero;

            for (int i = 0; i < childGameObjects.Count; i++)
            {
                var a = (i % _columnLimiter);
                var b = (int) (i / _columnLimiter);

                curentPos.x = a * _cellsWidth;
                curentPos.y = b * _cellsHeight * (_isDownDirection ? -1 : 1);

                childGameObjects[i].transform.localPosition = curentPos;
            }
        }
    }
}