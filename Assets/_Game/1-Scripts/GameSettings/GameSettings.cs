using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameSettings : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
            Input.multiTouchEnabled = false;
        }
    }
}
