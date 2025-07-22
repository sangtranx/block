using System.Collections;
using System.Collections.Generic;
using Game.Core.Line;
using UnityEngine;

namespace Game.Core.Line
{
    public class LineBlockController : MonoBehaviour
    {
        [SerializeField] private LineBlockView lineBlockView;

        public void InitLineSize(float x)
        {
            lineBlockView.InitLineSize(x);
        }
        
        public void EnableLineBlockByIdAndPos(Vector2 pos, int id)
        {
            lineBlockView.SetStatusLine(true);
            lineBlockView.SetPosLine(pos);
            lineBlockView.SetColorLineById(id);
        }

        public void DisableBlockLine()
        {
            lineBlockView.PlayAnimationShot();
        }

        public void DisableStatusBlockLine()
        {
            lineBlockView.SetStatusLine(false);
        }
    }
}