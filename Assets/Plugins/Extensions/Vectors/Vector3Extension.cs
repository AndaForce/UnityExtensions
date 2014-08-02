using UnityEngine;

namespace Assets.Plugins.Extensions.Vectors
{
    public static class Vector3Extension
    {
        public static Vector3 WithX(this Vector3 source, float newX)
        {
            return new Vector3(newX, source.y, source.z);
        }

        public static Vector3 WithY(this Vector3 source, float newY)
        {
            return new Vector3(source.x, newY, source.z);
        }

        public static Vector3 WithZ(this Vector3 source, float newZ)
        {
            return new Vector3(source.x, source.y, newZ);
        }
    }
}