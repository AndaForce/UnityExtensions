using UnityEngine;

namespace Assets.Plugins.Extensions.Vectors
{
    public static class Vector2Extension
    {
        public static Vector2 WithX(this Vector2 source, float newX)
        {
            return new Vector2(newX, source.y);
        }

        public static Vector2 WithY(this Vector2 source, float newY)
        {
            return new Vector2(source.x, newY);
        }
    }
}