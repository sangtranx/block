using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#region GameObject
public class Pooler
{
    IObjectPool<GameObject> pool;
    public Transform parentObject;
    public GameObject gameObject;
    public Pooler(GameObject gameObject, Transform parentObject, IObjectPoolUser objectPoolUser)
    {
        this.gameObject = gameObject;
        this.parentObject = parentObject;
        pool = new ObjectPool<GameObject>(CreateObject, objectPoolUser.GetFromPool, objectPoolUser.ReturnToPool, OnDestroyInPool);
    }
    public Pooler(GameObject gameObject, Transform parentObject, Action<GameObject> onGetFromPool, Action<GameObject> onReturnToPool)
    {
        this.gameObject = gameObject;
        this.parentObject = parentObject;
        pool = new ObjectPool<GameObject>(CreateObject, onGetFromPool, onReturnToPool, OnDestroyInPool);
    }
    public IObjectPool<GameObject> Pool => pool;
    GameObject CreateObject()
    {
        GameObject tmp = GameObject.Instantiate<GameObject>(gameObject, parentObject);
        var releaseAction = tmp.GetComponent<IObjectPoolRelease<GameObject>>();
        if (releaseAction != null)
        {
            releaseAction.Initialize(tmp, Pool);
        }
        else
        {
            Debug.LogWarning($"The pooling object {gameObject.name} does not have release action");
        }
        return tmp;
    }
    void OnDestroyInPool(GameObject obj)
    {
        GameObject.Destroy(obj);
    }

}
#endregion
#region Component Generic
public class Pooler<T> where T : Component
{
    IObjectPool<T> pool;
    public Transform parentObject;
    public T gameObject;
    public Pooler(T gameObject, Transform parentObject, IObjectPoolUser<T> objectPoolUser)
    {
        this.gameObject = gameObject;
        this.parentObject = parentObject;
        pool = new ObjectPool<T>(CreateObject, objectPoolUser.GetFromPool, objectPoolUser.ReturnToPool, OnDestroyInPool);
    }
    public Pooler(T gameObject, Transform parentObject, Action<T> onGetFromPool, Action<T> onReturnToPool)
    {
        this.gameObject = gameObject;
        this.parentObject = parentObject;
        pool = new ObjectPool<T>(CreateObject, onGetFromPool, onReturnToPool, OnDestroyInPool);
    }
    public IObjectPool<T> Pool => pool;
    T CreateObject()
    {
        T tmp = GameObject.Instantiate<T>(gameObject, parentObject);
        var releaseAction = tmp.GetComponent<IObjectPoolRelease<T>>();
        if (releaseAction != null)
        {
            releaseAction.Initialize(tmp, Pool);
        }
        else
        {
            Debug.LogWarning($"The pooling object {gameObject.name} does not have release action");
        }
        return tmp;
    }
    void OnDestroyInPool(T obj)
    {
        GameObject.Destroy(obj);
    }

}
#endregion