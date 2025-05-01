using System.Collections.Generic;
using Core.Inventory.Data;
using Core.Inventory.View;
using Core.Pools;
using UnityEngine;

namespace Core.Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] List<ItemSlot> _slots;
        [SerializeField] private RectTransform _itemsMovablesParent;
        [SerializeField] private ItemsConfig _itemsConfig;
        [SerializeField] private CommonItemViewPool _itemViewPool;

        private void Start()
        {
            _slots.ForEach(x=>x.Initialize(_itemViewPool, _itemsMovablesParent));
            
            _slots[0].SetItemPackage(new ItemPackageData(_itemsConfig.GetItemSetupById(0), 5));
            _slots[1].SetItemPackage(new ItemPackageData(_itemsConfig.GetItemSetupById(2), 6));
            _slots[2].SetItemPackage(new ItemPackageData(_itemsConfig.GetItemSetupById(0), 9));
        }
    }
}