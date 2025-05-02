using System;
using System.Collections.Generic;
using Core.Inventory.Data;

namespace Core.Services.PlayerData.Inventory
{
    [Serializable]
    public class InventoryViewStateModel
    {
        public Dictionary<int, InventoryViewStateData> ItemsSlotsIds;

        [Serializable]
        public struct InventoryViewStateData : IEquatable<InventoryViewStateData>
        {
            public int ItemId;
            public int Count;
            
                
            public bool Equals(InventoryViewStateData other)
            {
                return ItemId == other.ItemId && Count == other.Count;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(ItemId, Count);
            }
        }
    }
}
