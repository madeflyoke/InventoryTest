using System;
using System.Collections.Generic;
using Core.Inventory.Data;
using Core.Inventory.StateMachine;
using Core.Inventory.View;
using Core.Pools;
using UnityEngine;

namespace Core.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private int _slotsCount;
        [SerializeField] private RectTransform _slotsContainer;
        [SerializeField] private RectTransform _itemsMovablesParent;
        [SerializeField] private ItemsConfig _itemsConfig;
        private InventoryStateMachine _stateMachine;
        private List<ItemSlot> _slots = new List<ItemSlot>();
        
        private void Start()
        {
            for (int i = 0; i < _slotsCount; i++)
            {
                var instance = CommonPool.Instance.Spawn<ItemSlot>(_slotsContainer);
                instance.gameObject.SetActive(true);
                _slots.Add(instance);
            }
            _slots[0].SetItemPackage(new ItemPackageData(_itemsConfig.GetItemSetupById(0), 5));
            _slots[1].SetItemPackage(new ItemPackageData(_itemsConfig.GetItemSetupById(2), 6));
            _slots[2].SetItemPackage(new ItemPackageData(_itemsConfig.GetItemSetupById(0), 9));
            
            _stateMachine = new InventoryStateMachine(new InventoryStatesContext(_itemsMovablesParent, _slots));
        }

        private void OnDisable()
        {
            _stateMachine?.Dispose();
        }
    }
}