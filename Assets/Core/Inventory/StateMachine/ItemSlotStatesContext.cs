using Core.Inventory.View;
using Core.Pools;
using UnityEngine;

namespace Core.Inventory.StateMachine
{
    public class ItemSlotStatesContext
    {
        public ItemView MovableView;
        public ItemSlot TargetSlot;
        
        public ItemSlot RelatedSlot { get; private set; }
        public RectTransform MovablesParent { get; private set; }
        public IPool<ItemView> ItemViewPool { get; private set; }

        public ItemSlotStatesContext(ItemSlot relatedSlot, RectTransform movablesParent, IPool<ItemView> itemViewPool)
        {
            RelatedSlot = relatedSlot;
            MovablesParent = movablesParent;
            ItemViewPool = itemViewPool;
        }
    }
}
