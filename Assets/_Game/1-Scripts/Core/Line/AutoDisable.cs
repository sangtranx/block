using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Line
{
    public class AutoDisable : MonoBehaviour
    {
        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
