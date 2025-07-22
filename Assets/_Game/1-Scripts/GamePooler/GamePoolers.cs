using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Helpers
{
    public class GamePoolers : MonoBehaviour
    {
        [SerializeField] EffectPooler effectPooler;

        public EffectPooler EffectPooler { get => effectPooler; }
    }
    public class GamePooler<T> : MonoBehaviour, IObjectPoolUser where T : Enum
    {
        [Serializable]
        public class PoolData
        {
            public T typePooler;
            public GameObject prefab;
        }
        [SerializeField] List<PoolData> lstPoolerData;
        protected Dictionary<T, Pooler> dicPooler = new Dictionary<T, Pooler>();
        private void Awake()
        {
            for (int i = 0; i < lstPoolerData.Count; i++)
            {
                dicPooler.Add(lstPoolerData[i].typePooler, new Pooler(lstPoolerData[i].prefab, new GameObject(lstPoolerData[i].typePooler.ToString()).transform, this));
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
