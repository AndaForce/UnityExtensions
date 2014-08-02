using UnityEngine;

namespace Assets.Plugins.Components.Test
{
    public class GestureInputTest : MonoBehaviour
    {
        private void Update()
        {
            if (GestureInput.IsGesture(GestureDirection.Down)) Debug.Log("Down");
            if (GestureInput.IsGesture(GestureDirection.Left)) Debug.Log("Left");
            if (GestureInput.IsGesture(GestureDirection.Up)) Debug.Log("Up");
            if (GestureInput.IsGesture(GestureDirection.Right)) Debug.Log("Right");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Create Gesture Input"))
            {
                for (int i = 0; i < 100; i++)
                {
                    GestureInput.CreateInputObject();
                }
            }
        }
    }
}