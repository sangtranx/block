using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseFactory<T1, T2> where T1 : Enum
{
    protected Dictionary<T1, T2> stateByType = new Dictionary<T1, T2>();

    protected BaseFactory()
    {
        Initialize();
    }

    public abstract void Initialize();
    public T2 Get(T1 type)
    {
        if (stateByType.ContainsKey(type))
        {
            return stateByType[type];
        }
        return default(T2);
    }
}
