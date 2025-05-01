using System.Collections.Generic;
using Core.Inventory.Data;
using Core.Inventory.View;
using Core.Pools;
using Core.Pools.Interfaces;
using UnityEngine;

namespace Core.Inventory.StateMachine
{
    public class InventoryStatesContext
    {
        public ItemSlot SourceSlot;
        public ItemSlot TargetSlot;
        public ItemView MovableView;
        public ItemPackageData MovablePackageData; //combine with view?
        
        public RectTransform MovablesParent { get; private set; }
        public List<ItemSlot> AllSlots { get; private set; }

        public InventoryStatesContext(RectTransform movablesParent, List<ItemSlot> allSlots)
        {
            MovablesParent = movablesParent;
            AllSlots = allSlots;
        }
    }
}
