using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPoolUser<T> where T : UnityEngine.Object
{
    void GetFromPool(T obj);
    void ReturnToPool(T obj);
}
public interface IObjectPoolUser : IObjectPoolUser<GameObject>
{

}
public class ObjectPoolUser : IObjectPoolUser
{
    public void GetFromPool(GameObject obj)
    {

    }

    public void ReturnToPool(GameObject obj)
    {

    }
}