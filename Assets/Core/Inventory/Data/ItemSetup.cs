using UnityEngine;

namespace Core.Inventory.Data
{
    [CreateAssetMenu(fileName = "ItemSetup", menuName = "Game/Inventory/ItemSetup")]
    public class ItemSetup : ScriptableObject
    {
        [field: SerializeField] public int Id { get;private set; }
        [field: SerializeField] public Sprite Icon {get; private set;}
        [field: SerializeField] public string DisplayedName {get; private set;}
    }
}
