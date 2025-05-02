using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Inventory.View
{
    public class ItemSlotView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private ItemSlot _relatedSlot;
        [SerializeField] private Image _targetGraphic;
        [SerializeField] private Color _emptyColor;
        [SerializeField] private Color _filledColor;
        [SerializeField] private GameObject _selectedObject;

        private void OnEnable()
        {
            _relatedSlot.ItemPackageUpdated += RefreshView;
        }
        private void OnDisable()
        {
            _relatedSlot.ItemPackageUpdated -= RefreshView;
        }
        
        private void RefreshView(ItemSlot _)
        {
            _targetGraphic.color = _relatedSlot.IsEmpty? _emptyColor: _filledColor;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _selectedObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _selectedObject.SetActive(false);
            RefreshView(_relatedSlot);
        }
    }
}
