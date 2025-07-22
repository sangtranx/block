using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool<T> where T : UnityEngine.Object
{
    T Get();
    void Release(T obj);
    void Destroy(T obj);
}
public class ObjectPool<T> : IObjectPool<T> where T : UnityEngine.Object
{
    Func<T> onCreateObject;
    Action<T> onGetFromPool;
    Action<T> onReturnToPool;
    Action<T> onDestroyObject;
    List<T> lstObjectPooled;
    Stack<T> lstObjectPrepare;
    public ObjectPool(Func<T> onCreateObject, Action<T> onGetFromPool, Action<T> onReturnToPool, Action<T> onDestroyObject)
    {
        this.onCreateObject = onCreateObject;
        this.onGetFromPool = onGetFromPool;
        this.onReturnToPool = onReturnToPool;
        this.onDestroyObject = onDestroyObject;
        lstObjectPooled = new List<T>();
        lstObjectPrepare = new Stack<T>();
    }

    public void Destroy(T obj)
    {
        if (!lstObjectPooled.Contains(obj))
        {
            throw new CanNotDestroyException();
        }
        if (lstObjectPooled.Remove(obj))
        {
            onDestroyObject?.Invoke(obj);
        }
    }

    public T Get()
    {
        if (lstObjectPrepare.Count == 0)
        {
            lstObjectPrepare.Push(onCreateObject?.Invoke());
        }
        var obj = lstObjectPrepare.Pop();
        lstObjectPooled.Add(obj);
        onGetFromPool?.Invoke(obj);
        return obj;
    }

    public void Release(T obj)
    {
        if (!lstObjectPooled.Contains(obj))
        {
            throw new CanNotReleaseException();
        }
        if (lstObjectPooled.Remove(obj))
        {
            lstObjectPrepare.Push(obj);
            onReturnToPool?.Invoke(obj);
        }
    }
    #region Exception
    public class CanNotReleaseException : Exception
    {
        const string errorMessage = "Can not release the element!!!";
        public CanNotReleaseException() : base(errorMessage)
        {
        }
    }
    public class CanNotDestroyException : Exception
    {
        const string errorMessage = "Can not destroy the element!!!";
        public CanNotDestroyException() : base(errorMessage)
        {
        }
    }
    #endregion
}
