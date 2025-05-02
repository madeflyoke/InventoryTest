using System;
using Core.Services.PlayerData.Inventory;
using Newtonsoft.Json;

namespace Core.Services.PlayerData
{
    [Serializable]
    public class PlayerDataContainer
    {
        public InventoryModel InventoryModel { get; private set; }
        public InventoryViewStateModel InventoryViewStateModel { get; private set; }
        
        [JsonConstructor]
        public PlayerDataContainer(InventoryModel inventoryModel, InventoryViewStateModel inventoryViewStateModel)
        {
            InventoryModel = inventoryModel;
            InventoryViewStateModel = inventoryViewStateModel;
        }
    }
}
