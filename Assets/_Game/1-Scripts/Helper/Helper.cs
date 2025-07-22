using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Helpers
{
    public static class Helper
    {
        static Camera mainCamera;

        public static Camera MainCamera
        {
            get
            {
                if (mainCamera == null)
                {
                    mainCamera = Camera.main;
                }
                return mainCamera;
            }
        }

    }
}