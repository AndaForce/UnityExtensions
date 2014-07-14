using UnityEngine;

namespace Assets.Scripts
{
    public class SomeScript : MonoBehaviour {

        private void OnGUI()
        {
            if (GUILayout.Button("PRESS ME"))
            {
                for (int i = 0; i < 100; i++)
                {
                    GestureInput.CreateInputObject();
                }
            }
        }

        private void Update()
        {
            if (GestureInput.IsGesture(GestureDirection.Down)) Debug.Log("Down");
            if (GestureInput.IsGesture(GestureDirection.Left)) Debug.Log("Left");
            if (GestureInput.IsGesture(GestureDirection.Up)) Debug.Log("Up");
            if (GestureInput.IsGesture(GestureDirection.Right)) Debug.Log("Right");
        }
    }
}
