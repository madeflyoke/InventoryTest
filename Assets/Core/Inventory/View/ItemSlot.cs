using System;
using Core.Inventory.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Inventory.View
{
    public class ItemSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        public event Action<ItemSlot,PointerEventData> OnBeginDragEvent;
        public event Action<ItemSlot,PointerEventData> OnEndDragEvent;
        public event Action<ItemSlot,PointerEventData> OnDragEvent;
        public event Action<ItemSlot,PointerEventData> OnClickEvent;

        public bool IsEmpty => CurrentItemPackage == null;
        public ItemPackageData CurrentItemPackage { get; private set; }

        [SerializeField] private RectTransform _itemViewParent;
        [SerializeField] private ItemView _itemView;
        
        public void SetItemPackage(ItemPackageData itemPackage)
        {
            CurrentItemPackage = itemPackage.Count==0? null:itemPackage;
            UpdateViewToData();
        }

        public void UpdateViewToData()
        {
            if (IsEmpty==false)
            {
                _itemView.Setup(CurrentItemPackage.ItemSetup.Icon, CurrentItemPackage.Count);
            }
            else
            {
                Clear();
            }
            SetViewActive(true);
        }

        public void SetViewActive(bool value)
        {
            _itemView.gameObject.SetActive(value);
        }

        public void ChangeViewData(int newCount)
        {
            _itemView.SetCountText(newCount);
        }
        
        private void Clear()
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
            OnBeginDragEvent?.Invoke(this, eventData);
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            OnEndDragEvent?.Invoke(this,eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDragEvent?.Invoke(this,eventData);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsEmpty)
            {
                return;
            }
            OnClickEvent?.Invoke(this,eventData);
        }
    }
}
