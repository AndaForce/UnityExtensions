using System;
using UnityEngine;

namespace Assets.Plugins
{
    [ExecuteInEditMode]
    public class AutoNameChild : MonoBehaviour
    {
        [SerializeField] private String _prefix = "Item";
        [SerializeField] private String _affix = "";
        [SerializeField] private int _startWith = 0;
        [SerializeField] private int _digits = 6;
        [SerializeField] private bool _renameNow = false;

        private int _i = 0;

        // Update is called once per frame
        private void Update()
        {
            if (_renameNow)
            {
                _renameNow = false;
                _i = _startWith;
                RenameChild();
            }
        }

        private void RenameChild()
        {
            Component[] childGameObjects = gameObject.GetComponentsInChildren(typeof (Transform));
            foreach (var childGameObject in childGameObjects)
            {
                if (childGameObject.transform.parent == transform)
                {
                    childGameObject.gameObject.name =
                        String.Format("{0}{1:d" + _digits + "}{2}", _prefix, _i, _affix);

                    _i += 1;
                }
            }
        }
    }
}