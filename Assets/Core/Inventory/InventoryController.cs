using System;
using System.Collections.Generic;
using System.Threading;
using Core.Inventory.Data;
using Core.Inventory.StateMachine;
using Core.Inventory.View;
using Core.Pools;
using Core.Services;
using Core.Services.PlayerData.Inventory;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private int _slotsCount;
        [SerializeField] private GridLayoutGroup _slotsContainer;
        [SerializeField] private RectTransform _itemsMovablesParent;
        [SerializeField] private ItemsConfig _itemsConfig;
        private InventoryStateMachine _stateMachine;
        private List<ItemSlot> _slots = new List<ItemSlot>();

        private InventoryModelMediator _inventoryModelMediator;
        private CancellationTokenSource _cts= new CancellationTokenSource();
        
        private async void Start()
        {
            _inventoryModelMediator = ServiceLocator.Instance.PlayerDataService.InventoryModelMediator;
            for (int i = 0; i < _slotsCount; i++)
            {
                var instance = CommonPool.Instance.Spawn<ItemSlot>(_slotsContainer.transform);
                instance.Initialize(i);
                instance.SetItemPackage(_inventoryModelMediator.GetItemsDataBySlot(i, out var itemId, out var count)
                    ? new ItemPackageData(_itemsConfig.GetItemSetupById(itemId), count)
                    : null);

                instance.gameObject.SetActive(true);
                instance.ItemPackageUpdated += SaveInventorySlotState;
                _slots.Add(instance);
            }
            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate, cancellationToken: _cts.Token);
            DisableLayouts();

            _stateMachine = new InventoryStateMachine(new InventoryStatesContext(_itemsMovablesParent, _slots));
        }

        private void SaveInventorySlotState(ItemSlot slot)
        {
            if (slot.IsEmpty)
            {
                _inventoryModelMediator.ClearItemsPackageSlot(slot.SlotId);
            }
            else
            {
                _inventoryModelMediator.SetItemsPackageSlot(slot.SlotId, slot.CurrentItemPackage.ItemSetup.Id,
                    slot.CurrentItemPackage.Count);
            }
        }

        private void DisableLayouts()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_slotsContainer.transform as RectTransform);
            if (_slotsContainer.TryGetComponent(out ContentSizeFitter fitter))
            {
                fitter.enabled = false;
            }
            _slotsContainer.enabled = false;
        }

        private void OnDisable()
        {
            foreach (var slot in _slots)
            {
                slot.ItemPackageUpdated -= SaveInventorySlotState;
            }
            _stateMachine?.Dispose();
        }
    }
}