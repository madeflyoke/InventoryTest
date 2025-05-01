using System;
using System.Collections.Generic;
using Core.Pools.Interfaces;
using UnityEngine;

namespace Core.Pools
{
    public class CommonPool : MonoBehaviour, IMonoPool
    {
        public static IMonoPool Instance { get; private set; }
        
        [SerializeField] private List<MonoBehaviour> _componentsPrefabs;
        [SerializeField] private int _preloadCount;

        private Dictionary<Type, (MonoBehaviour, Queue<MonoBehaviour>)> _pools = new();
        private Transform _poolsParent;
        
        public void Awake()
        {
            _poolsParent = new GameObject().transform;
            _poolsParent.gameObject.SetActive(false);
#if UNITY_EDITOR
            _poolsParent.name = $"PoolsParent";
#endif

            foreach (var component in _componentsPrefabs)
            {
                var type = component.GetType();
                _pools.Add(type, (component, new Queue<MonoBehaviour>()));

                for (int i = 0; i < _preloadCount; i++)
                {
                    AddNew(type);
                }
            }
            
            Instance = this;
        }
        

        public T Spawn<T>(Transform parent, bool worldPositionStays = false) where T: MonoBehaviour
        {
            var targetPool = _pools[typeof(T)].Item2;
            if (targetPool.TryPeek(out var result) == false || result.gameObject.activeSelf)
            {
                AddNew(typeof(T));
            }

            var target = targetPool.Dequeue();
            target.transform.SetParent(parent);
            if (worldPositionStays == false)
            {
                target.transform.localPosition = Vector3.zero;
            }

            target.gameObject.SetActive(true);
            return (T)target;
        }

        public void Despawn<T>(T item) where T : MonoBehaviour
        {
            item.gameObject.SetActive(false);
            _pools[typeof(T)].Item2.Enqueue(item);
            item.transform.SetParent(_poolsParent);
        }

        private void AddNew(Type key)
        {
            var prefab = _pools[key].Item1;
            var instance = Instantiate(prefab, parent:_poolsParent);
            instance.gameObject.SetActive(false);
            _pools[key].Item2.Enqueue(instance);
        }
    }
}