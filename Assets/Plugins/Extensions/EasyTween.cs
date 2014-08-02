using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Plugins.Extensions
{
    public class EasyTween : MonoBehaviour 
    {
        public class TweenEasyHash
        {
            public GameObject objectToMove;

            public string name, axis;

            public Vector3? position;

            public float? x, y, z, time , speed ;

            public List<Transform> pathTR;
            public List<Vector3> pathV3;

            public Hashtable ToHashtable()
            {
                Hashtable hash = new Hashtable();
                return hash;
            }
        }

        public static void StartTween(System.Action<GameObject, Hashtable> tweenFunc, TweenEasyHash hash)
        {
            tweenFunc(hash.objectToMove, hash.ToHashtable());
        }

        //testing
        private void Start()
        {
            StartTween(iTween.iTween.MoveTo, new TweenEasyHash() { objectToMove = gameObject });
        }
    }
}
