using UnityEngine;

namespace Assets.Plugins.Helpers.GameObjectPull.Test
{
    public class PullElement : AbstractPullElement
    {
        public void SetAtPosition(Vector3 position)
        {
            CachedTransform.position = position;
        }

        public void MoveTo(Vector3 target, float moveTime)
        {
            iTween.iTween.Stop(gameObject);
            iTween.iTween.MoveTo(
                gameObject,
                iTween.iTween.Hash(
                    "position", target,
                    "time", moveTime,
                    "easetype", iTween.iTween.EaseType.linear,
                    "oncomplete", "DeactivateElement"));
        }

        public override void OnActivateElement()
        {
            renderer.enabled = true;
        }

        public override void OnDeactivateElement()
        {
            renderer.enabled = false;
        }
    }
}