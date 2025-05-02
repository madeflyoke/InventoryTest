using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Inventory.Data
{
    [CreateAssetMenu(fileName = "ItemsConfig", menuName = "Game/Inventory/ItemsConfig")]
    public class ItemsConfig : ScriptableObject
    {
        [SerializeField] private ItemSetup[] _itemsSetups;

        public ItemSetup GetItemSetupById(int id)
        {
            return _itemsSetups.FirstOrDefault(x=>x.Id == id);
        }

        public ItemSetup GetRandomItemSetup()
        {
            return _itemsSetups[Random.Range(0, _itemsSetups.Length)];
        }

        public void OnValidate()
        {
            var sameGroups = _itemsSetups.GroupBy(x => x.Id).Where(x=>x.Count() > 1).ToList();
            if (sameGroups.Any())
            {
                string msg = $"One or more items have the same ID, ids: ";
                foreach (var g in sameGroups)
                {
                    msg += $"/{g.Key}/";
                }
                Debug.LogWarning(msg);
            }
        }
    }
}
