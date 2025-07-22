using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Helpers
{
    public enum TypeEffect
    {
        Highlight,
        Merge,
        Boom,
        Move,
        BoomMerge,
    }
    public class EffectPooler : BaseGamePooler<TypeEffect>
    {
        
    }
}
