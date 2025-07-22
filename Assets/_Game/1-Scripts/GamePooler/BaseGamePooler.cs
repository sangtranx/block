using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Helpers
{
    public class BaseGamePooler<T> : MonoBehaviour, IObjectPoolUser where T : Enum
    {
        [Serializable]
        public class PoolData
        {
            public T typePooler;
            public Transform tfmPooledObjectContainer;
            public GameObject gobjObject2Pool;
        }
        [SerializeField] List<PoolData> lstPoolerData;
        protected Dictionary<T, Pooler> dicPooler = new Dictionary<T, Pooler>();
        private void Awake()
        {
            for (int i = 0; i < lstPoolerData.Count; i++)
            {
                dicPooler.Add(lstPoolerData[i].typePooler, new Pooler(lstPoolerData[i].gobjObject2Pool, lstPoolerData[i].tfmPooledObjectContainer, this));
            }
        }

        public virtual void GetFromPool(GameObject obj)
        {
            obj.SetActive(true);
        }

        public virtual void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);
        }
        public Pooler GetPooler(T type)
        {
            if (dicPooler.ContainsKey(type))
            {
                return dicPooler[type];
            }
            return null;
        }
        public GameObject GetObjectPooled(T type)
        {
            if (dicPooler.ContainsKey(type))
            {
                return dicPooler[type].Pool.Get();
            }
            return null;
        }
    }
}
