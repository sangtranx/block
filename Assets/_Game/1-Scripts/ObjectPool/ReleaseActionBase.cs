using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRelease
{
    void Release();
}
public interface IObjectPoolRelease<T> : IRelease where T : Object
{
    void Initialize(T gameObject, IObjectPool<T> pool);
}
public class ReleaseActionBase<T> : MonoBehaviour, IObjectPoolRelease<T> where T : Object
{
    protected new T gameObject;
    protected IObjectPool<T> pool;
    protected bool isReleased = false;
    public virtual void Initialize(T gameObject, IObjectPool<T> pool)
    {
        this.gameObject = gameObject;
        this.pool = pool;
    }
    public virtual void Release()
    {
        if(!isReleased)
        {
            isReleased = true;
            pool.Release(gameObject);
        }
    }
}
