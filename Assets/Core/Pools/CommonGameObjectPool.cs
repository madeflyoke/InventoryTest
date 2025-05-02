using System.Collections.Generic;
using UnityEngine;

namespace Core.Pools
{
    public class CommonGameObjectPool : MonoBehaviour
    {
        public static CommonGameObjectPool Instance { get; private set; }
        
        [SerializeField] private List<GameObject> _prefabs;
        [SerializeField] private int _preloadCount;

        private Dictionary<GameObject, PoolableGameObject> _instancesMap = new();
        private Dictionary<GameObject, Queue<GameObject>> _pools = new();
        private Transform _poolsParent;
        
        public void Awake()
        {
            _poolsParent = new GameObject().transform;
            _poolsParent.gameObject.SetActive(false);
#if UNITY_EDITOR
            _poolsParent.name = $"GameObjectPoolsParent";
#endif

            foreach (var prefab in _prefabs)
            {
                _pools.Add(prefab,new Queue<GameObject>());

                for (int i = 0; i < _preloadCount; i++)
                {
                    AddNew(prefab);
                }
            }
            
            Instance = this;
        }
        
        public GameObject Spawn(GameObject prefab, Transform parent, bool worldPositionStays = false)
        {
            var targetPool = _pools[prefab];
            if (targetPool.TryPeek(out var result) == false || result.gameObject.activeSelf)
            {
                AddNew(prefab);
            }

            var target = targetPool.Dequeue();
            target.transform.SetParent(parent);
            if (worldPositionStays == false)
            {
                target.transform.localPosition = Vector3.zero;
            }

            target.gameObject.SetActive(true);
            return target;
        }

        public void Despawn(GameObject item)
        {
            item.gameObject.SetActive(false);
            _pools[_instancesMap[item].Id].Enqueue(item);
            item.transform.SetParent(_poolsParent);
        }

        private void AddNew(GameObject key)
        {
            var instance = Instantiate(key, parent:_poolsParent);
            var poolable = instance.AddComponent<PoolableGameObject>();
            poolable.SetId(key);
            instance.gameObject.SetActive(false);
            _instancesMap.Add(instance, poolable);
            _pools[key].Enqueue(instance);
        }
    }
}