using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Plugins.Helpers.GameObjectPull
{
    public abstract class AbstractPull<TConcretePullElementType> where TConcretePullElementType : AbstractPullElement
    {
        public readonly Transform ParentTransform;
        public readonly GameObject Prefab;
        public List<AbstractPullElement> ObjectsPull;
        public int MaxObjectsCount;

        protected AbstractPull(Transform parentTransform, GameObject prefab)
        {
            ParentTransform = parentTransform;
            Prefab = prefab;
            ObjectsPull = new List<AbstractPullElement>();
        }

        /// <summary>
        /// Clears old collection, destroys all objects and then creates new, with new capacity settings
        /// </summary>
        /// <param name="initialCount">Initial count of created objects in pull</param>
        /// <param name="maxCount">Maximum count of pull objects, 0 for infinity pull</param>
        public void ReinitializePull(int initialCount, int maxCount = 0)
        {
            MaxObjectsCount = maxCount;

            ObjectsPull.ForEach(a => a.DestroyElement());
            ObjectsPull.Clear();
            for (int i = 0; i < initialCount; i++)
            {
                CreateNewPullElement();
            }
        }

        private AbstractPullElement CreateNewPullElement()
        {
            if (ObjectsPull.Count > MaxObjectsCount && MaxObjectsCount != 0) return null;

            var newPullElement = CreateGameObject(Prefab);
            newPullElement.CachedTransform.parent = ParentTransform;
            newPullElement.DeactivateElement();

            ObjectsPull.Add(newPullElement);

            return newPullElement;
        }

        public TConcretePullElementType GetObjectFromPull()
        {
            var freePullElement = ObjectsPull.FirstOrDefault(a => !a.IsActive) ?? CreateNewPullElement();

            if (freePullElement != null)
            {
                freePullElement.ActivateElement();
                return freePullElement as TConcretePullElementType;
            }
            return null;
        }

        public abstract AbstractPullElement CreateGameObject(GameObject prefab);
    }

    public abstract class AbstractPullElement : MonoBehaviour
    {
        public bool IsActive { get; protected set; }
        public Transform CachedTransform { get; protected set; }
        public GameObject CachedGameObject { get; protected set; }
        public abstract void OnActivateElement();
        public abstract void OnDeactivateElement();

        private void Awake()
        {
            CachedTransform = transform;
            CachedGameObject = gameObject;
        }

        public void ActivateElement()
        {
            IsActive = true;

            OnActivateElement();
        }

        public void DeactivateElement()
        {
            IsActive = false;

            OnDeactivateElement();
        }

        public void DestroyElement()
        {
            Destroy(CachedGameObject);
        }
    }
}