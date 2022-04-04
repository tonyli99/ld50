using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace __Game.Scripts
{

    public class Pool<T> where T : HealthEntity
    {

        private T prefab;
        private List<T> instances = new List<T>();
        
        public Pool(T initialPrefab)
        {
            prefab = initialPrefab;
        }

        public T Spawn(Vector3 pos, Quaternion rot)
        {
            T entity = null;
            foreach (var instance in instances)
            {
                if (!instance.gameObject.activeSelf)
                {
                    entity = instance;
                    break;
                }
            }
            if (entity == null)
            {
                entity = GameObject.Instantiate(prefab, pos, rot);
                entity.IsPooled = true;
                instances.Add(entity);
            }
            entity.gameObject.SetActive(true);
            entity.transform.position = pos;
            entity.Initialize();
            return entity;
        }
    }
}