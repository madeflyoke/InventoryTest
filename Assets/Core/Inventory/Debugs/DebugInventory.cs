using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Inventory.Data;
using Core.Inventory.View;
using Core.Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Inventory.Debugs
{
    public class DebugInventory : MonoBehaviour
    {
        [SerializeField] private ItemsConfig _itemsConfig;
        private InventoryController _controller;
        private List<ItemSlot> _slots;

        private void Awake()
        {
            _controller = FindObjectOfType<InventoryController>();
            _slots = typeof(InventoryController).GetField("_slots", BindingFlags.Instance | BindingFlags.NonPublic).
                GetValue(_controller) as List<ItemSlot>;
        }

        public void AddRandomItem()
        {
            var count = Random.Range(1, 6);
            var emptySlots = _slots.Where(x => x.IsEmpty).ToList();
            if (emptySlots.Count == 0)
            {
                return;
            }
            var slotId = Random.Range(0, emptySlots.Count);
            var packageData = new ItemPackageData(_itemsConfig.GetRandomItemSetup(), count);
            
            ServiceLocator.Instance.PlayerDataService.InventoryModelMediator.AddItem(packageData.ItemSetup.Id, packageData.Count);
            emptySlots[slotId].SetItemPackage(new ItemPackageData(_itemsConfig.GetRandomItemSetup(), count));;
        }

        public void RemoveRandomItem()
        {
            var filledSlots = _slots.Where(x => x.IsEmpty==false).ToList();
            if (filledSlots.Count == 0)
            {
                return;
            }
            var slotId = Random.Range(0, filledSlots.Count);
            
            var target = filledSlots[slotId];
            var resultCount = target.CurrentItemPackage.Count - 1;
            target.CurrentItemPackage.SetCount(resultCount);
            target.UpdateViewToData();
            if (resultCount<=0)
            {
                ServiceLocator.Instance.PlayerDataService.InventoryModelMediator.ClearItemsPackageSlot(target.SlotId);
            }
            else
            {
                ServiceLocator.Instance.PlayerDataService.InventoryModelMediator.SetItemsPackageSlot(target.SlotId,
                    target.CurrentItemPackage.ItemSetup.Id, target.CurrentItemPackage.Count);
            }
        }
    }
}
