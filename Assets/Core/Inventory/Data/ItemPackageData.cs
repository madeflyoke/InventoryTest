using System;

namespace Core.Inventory.Data
{
    public class ItemPackageData : IEquatable<ItemPackageData>
    {
        public ItemSetup ItemSetup { get; private set; }
        public int Count { get; private set; }

        public ItemPackageData(ItemSetup itemSetup, int count)
        {
            ItemSetup = itemSetup;
            Count = count;
        }

        public void SetCount(int count) => Count=count;

        public void SetItemSetup(ItemSetup itemSetup)=> ItemSetup = itemSetup;
        
        public bool Equals(ItemPackageData other)
        {
            return other != null && Equals(ItemSetup.Id, other.ItemSetup.Id);
        }

        public ItemPackageData Clone()
        {
            return new ItemPackageData(ItemSetup, Count);
        }
    }
}
