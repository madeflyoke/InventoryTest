using Core.Inventory.Data;
using Core.Inventory.View;
using Core.StateMachines.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Inventory.StateMachine.States
{
    public class DragState : IState
    {
        private IStateMachine _stateMachine;
        private ItemSlotStatesContext _context;
        
        public DragState(IStateMachine stateMachine, ItemSlotStatesContext context)
        {
            _stateMachine = stateMachine;
            _context = context;
        }

        public void Enter()
        {
            _context.RelatedSlot.OnDragEvent += OnDrag;
            _context.RelatedSlot.OnEndDragEvent += OnEndDrag;
            
            _context.RelatedSlot.SetViewActive(false);
            _context.MovableView = CreateVisualPackage(_context.RelatedSlot.CurrentItemPackage);
        }

        private void OnDrag(PointerEventData eventData)
        {
            _context.MovableView.transform.position = eventData.position;
        }

        private ItemView CreateVisualPackage(ItemPackageData itemPackageData)
        {
            var packageItemView = _context.ItemViewPool.Spawn(_context.MovablesParent);
            packageItemView.Setup(itemPackageData.ItemSetup.Icon, itemPackageData.Count);
            
            return packageItemView;
        }
        
        private void OnEndDrag(PointerEventData pointerEventData)
        {
            var target = pointerEventData.pointerCurrentRaycast.gameObject;
            _context.TargetSlot = target?.GetComponent<ItemSlot>();
            _stateMachine.SwitchState<DropState>();
        }
        
        public void Exit()
        {
            _context.RelatedSlot.OnDragEvent -= OnDrag;
            _context.RelatedSlot.OnEndDragEvent -= OnEndDrag;
        }
    }
}
