using System;
using Core.Inventory.Data;
using Core.Inventory.StateMachine;
using Core.Pools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Inventory.View
{
    public class ItemSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public event Action OnBeginDragEvent;
        public event Action<PointerEventData> OnEndDragEvent;
        public event Action<PointerEventData> OnDragEvent;

        public bool IsEmpty => CurrentItemPackage == null;
        public ItemPackageData CurrentItemPackage { get; private set; }

        [SerializeField] private RectTransform _itemViewParent;
        [SerializeField] private ItemView _itemView;
        private ItemSlotStateMachine _stateMachine;

        public void Initialize(IPool<ItemView> itemViewPool, RectTransform movablesParent)
        {
            _stateMachine = new ItemSlotStateMachine(new ItemSlotStatesContext(this, movablesParent,itemViewPool));
        }
        
        public void SetItemPackage(ItemPackageData itemPackage)
        {
            CurrentItemPackage = itemPackage;
            _itemView.Setup(CurrentItemPackage.ItemSetup.Icon, CurrentItemPackage.Count);
            SetViewActive(true);
        }

        public void SetViewActive(bool value)
        {
            _itemView.gameObject.SetActive(value);
        }
        
        public void Clear()
        {
            CurrentItemPackage = null;
            _itemView.Clear();
            SetViewActive(true);
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (IsEmpty)
            {
                return;
            }
            OnBeginDragEvent?.Invoke();
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDragEvent?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDragEvent?.Invoke(eventData);
        }
    }
}
