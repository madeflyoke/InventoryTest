using System;
using System.Collections.Generic;
using Core.Services.PlayerData.Inventory;
using Core.Utils.PlayerData;

namespace Core.Services.PlayerData
{
    public class PlayerDataService : IDisposable
    {
        private const string PlayerDataKey = "PlayerData";
        private PlayerDataContainer _playerDataContainer;

        public InventoryModelMediator InventoryModelMediator { get; private set; }
        
        public PlayerDataService()
        {
            _playerDataContainer = JsonSaver.Load<PlayerDataContainer>(PlayerDataKey);
            
            if (_playerDataContainer == null)
            {
                HandleNewPlayer();
            }

            InventoryModelMediator = new InventoryModelMediator(_playerDataContainer.InventoryModel);
            InventoryModelMediator.SaveRequested += Save;
        }

        private void HandleNewPlayer()
        {
             _playerDataContainer = new PlayerDataContainer(new InventoryModel(){ItemsData = new Dictionary<int, int>()});
             Save();
        }
        
        public void Save()
        {
            JsonSaver.Save(_playerDataContainer, PlayerDataKey);
        }

        public void Dispose()
        {
            InventoryModelMediator.SaveRequested -= Save;
        }
    }
}
