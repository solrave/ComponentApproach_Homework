using System;
using UnityEngine;

namespace Game.Scripts
{
    public class ApplicationQuit : MonoBehaviour
    {
        public static bool AppIsClosing;

        private void OnApplicationQuit()
        {
            AppIsClosing = true;
        }
    }
}