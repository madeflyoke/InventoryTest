using System;

namespace Core.Services.PlayerData.Inventory
{
    public class InventoryModelMediator
    {
        public event Action SaveRequested;
        
        private InventoryModel _inventoryModel;

        public InventoryModelMediator(InventoryModel inventoryModel)
        {
            _inventoryModel = inventoryModel;
        }

        public void AddItem(int itemId, int amount)
        {
            SetItemCount(itemId, GetItemCount(itemId)+amount);
        }

        public void RemoveItem(int itemId, int amount)
        {
            SetItemCount(itemId, GetItemCount(itemId)-amount);
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
            if (_inventoryModel.ItemsData.ContainsKey(itemId)==false)
            {
                _inventoryModel.ItemsData.Add(itemId, 0);
            }
        }
    }
}
