using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Block
{
    public class BlockElementRelease : ReleaseActionBase<BlockElement>
    {
        private void OnDisable()
        {
            Release();
        }
    }
}
