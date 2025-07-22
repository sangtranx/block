using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AudioSettings
{
    [Range(0, 1)] public float volume;
    public bool loop;
}
