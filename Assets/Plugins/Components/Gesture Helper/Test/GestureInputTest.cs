using System;
using UnityEngine;

namespace Assets.Plugins.Components.Test
{
    public class GestureInputTest : MonoBehaviour
    {
        private bool _isCreatedPressed;
        private String _gestureString = String.Empty;

        private void OnGUI()
        {
            if (GUILayout.Button("Create Gesture Input"))
            {
                // Only one can be on scene at time
                for (int i = 0; i < 100; i++)
                {
                    GestureInput.CreateInputObject(); 
                }

                // Setup limiters
                GestureInput.SetupLimiters(6.0f, 6.0f); 

                _isCreatedPressed = true;
            }

            // Check gesture input to find your gesture
            if (_isCreatedPressed && !GestureInput.IsGesture(GestureDirection.None))
            {
                _gestureString = "Last gesture was: ";

                if (GestureInput.IsGesture(GestureDirection.Down)) _gestureString += "Down";
                if (GestureInput.IsGesture(GestureDirection.Left)) _gestureString += "Left";
                if (GestureInput.IsGesture(GestureDirection.Up)) _gestureString += "Up";
                if (GestureInput.IsGesture(GestureDirection.Right)) _gestureString += "Right";
            }

            GUILayout.Label(_gestureString);
        }
    }
}