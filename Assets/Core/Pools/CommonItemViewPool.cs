using System.Collections.Generic;
using Core.Inventory.View;
using EasyButtons;
using UnityEngine;

namespace Core.Pools
{
    public class CommonItemViewPool : MonoBehaviour, IPool<ItemView>
    {
        [SerializeField] public ItemView _prefab;
        [SerializeField] private int _preloadCount;
        private Queue<ItemView> _itemPool = new Queue<ItemView>();
        private Transform _poolParent;

        private void Awake()
        {
            _poolParent = new GameObject().transform;
#if UNITY_EDITOR
            _poolParent.name = "ItemViewPool";
#endif
            for (int i = 0; i < _preloadCount; i++)
            {
                AddNew();
            }
        }

        [Button]
        public ItemView Spawn(Transform parent, bool worldPositionStays = false)
        {
            if (_itemPool.TryPeek(out var result) == false || result.gameObject.activeSelf)
            {
                AddNew();
            }

            var target = _itemPool.Dequeue();
            target.transform.SetParent(parent);
            if (worldPositionStays == false)
            {
                target.transform.localPosition = Vector3.zero;
            }

            target.gameObject.SetActive(true);
            return target;
        }

        [Button]
        public void Despawn(ItemView item)
        {
            item.gameObject.SetActive(false);
            _itemPool.Enqueue(item);
            item.transform.SetParent(_poolParent);
        }

        private void AddNew()
        {
            var instance = Instantiate(_prefab, parent:_poolParent);
            instance.gameObject.SetActive(false);
            _itemPool.Enqueue(instance);
        }
    }
}