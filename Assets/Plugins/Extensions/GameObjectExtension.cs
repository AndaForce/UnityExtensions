using UnityEngine;

namespace Assets.Plugins.Extensions
{
    public static class GameObjectExtension
    {
        /// <summary>
        /// Adds component to game object. Returns this component from game object if already exists
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="gameObject"></param>
        /// <returns>New component instance or existed component instance</returns>
        public static T AddComponentIfAbsent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
        }
    }
}