using System;
using Core.Inventory.Data;

namespace Core.Services.PlayerData.Inventory
{
    public class InventoryModelMediator
    {
        public event Action SaveRequested;

        private InventoryModel _inventoryModel;
        private InventoryViewStateModel _inventoryViewStateModel;

        public InventoryModelMediator(InventoryModel inventoryModel, InventoryViewStateModel inventoryViewStateModel)
        {
            _inventoryModel = inventoryModel;
            _inventoryViewStateModel = inventoryViewStateModel;
        }

        public void AddItem(int itemId, int amount)
        {
            SetItemCount(itemId, GetItemCount(itemId) + amount);
        }

        public void RemoveItem(int itemId, int amount)
        {
            SetItemCount(itemId, GetItemCount(itemId) - amount);
        }

        public int GetItemCount(int itemId)
        {
            ValidateId(itemId);
            return _inventoryModel.ItemsData[itemId];
        }

        private void SetItemCount(int itemId, int count)
        {
            _inventoryModel.ItemsData[itemId] = count;
            SaveRequested?.Invoke();
        }

        private void ValidateId(int itemId)
        {
            if (_inventoryModel.ItemsData.ContainsKey(itemId) == false)
            {
                _inventoryModel.ItemsData.Add(itemId, 0);
            }
        }

        #region InventorySlots

        public bool GetItemsDataBySlot(int slotId, out int itemId, out int count)
        {
            if (_inventoryViewStateModel.ItemsSlotsIds.ContainsKey(slotId) == false)
            {
                itemId = -1;
                count = -1;
                return false;
            }
            var target = _inventoryViewStateModel.ItemsSlotsIds[slotId];
            itemId = target.ItemId;
            count = target.Count;
            return true;
        }

        public void SetItemsPackageSlot(int slotId, int itemId, int count)
        {
            ValidateSlotId(slotId);
            var target = _inventoryViewStateModel.ItemsSlotsIds[slotId];
            target.ItemId = itemId;
            target.Count = count;
            _inventoryViewStateModel.ItemsSlotsIds[slotId] = target;
            SaveRequested?.Invoke();
        }
        
        public void ClearItemsPackageSlot(int slotId)
        {
            ValidateSlotId(slotId);
            _inventoryViewStateModel.ItemsSlotsIds[slotId] = new InventoryViewStateModel.InventoryViewStateData();
            SaveRequested?.Invoke();
        }

        private void ValidateSlotId(int slotId)
        {
            if (_inventoryViewStateModel.ItemsSlotsIds.ContainsKey(slotId) == false)
            {
                _inventoryViewStateModel.ItemsSlotsIds.Add(slotId, new InventoryViewStateModel.InventoryViewStateData());
            }
        }

        #endregion
    }
}