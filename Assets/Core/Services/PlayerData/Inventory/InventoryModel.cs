using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Services.PlayerData.Inventory
{
    [Serializable]
    public class InventoryModel
    {
        public Dictionary<int, int> ItemsData;
    }
}
