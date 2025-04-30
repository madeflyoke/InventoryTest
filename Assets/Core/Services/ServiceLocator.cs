using Core.Services.PlayerData;
using EasyButtons;
using UnityEngine;

namespace Core.Services
{
    public class ServiceLocator : MonoBehaviour
    {
        public static ServiceLocator Instance {get; private set;}
        
        public PlayerDataService PlayerDataService {get; private set;}
        
        public void Awake()
        {
            if (Instance!=null)
            {
                Destroy(gameObject);
                return;
            }
            
            PlayerDataService = new PlayerDataService();
            
            Instance = this;
        }

        [Button]
        public void AddItem(int itemid, int count)
        {
            PlayerDataService.InventoryModelMediator.AddItem(itemid, count);
        }
        
        [Button]
        public void GetItem(int itemid)
        {
            Debug.LogWarning(PlayerDataService.InventoryModelMediator.GetItemCount(itemid));
        }
    }
}
