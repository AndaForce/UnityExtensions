using UnityEngine;

namespace Assets._UnityExtensions.Extensions
{
    public static class ColorExtension {

        public static Color ChangeAlpha(this Color color, float newAlpha)
        {
            return new Color(color.r, color.g, color.b, newAlpha);
        }
    }
}
