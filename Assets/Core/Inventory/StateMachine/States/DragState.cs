using Core.Inventory.Data;
using Core.Inventory.View;
using Core.Pools;
using Core.StateMachines.Interfaces;
using Core.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Inventory.StateMachine.States
{
    public class DragState : IState
    {
        private IStateMachine _stateMachine;
        private InventoryStatesContext _context;
        
        public DragState(IStateMachine stateMachine, InventoryStatesContext context)
        {
            _stateMachine = stateMachine;
            _context = context;
        }

        public void Enter()
        {
            _context.SourceSlot.OnDragEvent += OnDrag;
            _context.SourceSlot.OnEndDragEvent += OnEndDrag;
            
            _context.SourceSlot.SetViewActive(false);
            _context.MovablePackageData = _context.SourceSlot.CurrentItemPackage.Clone();
            _context.MovableView = CreateVisualPackage();
        }

        private void OnDrag(ItemSlot itemSlot, PointerEventData pointerEventData)
        {
            _context.MovableView.transform.position = _context.InputHelper.GetWorldMousePosition();
        }

        private ItemView CreateVisualPackage()
        {
            var packageItemView = CommonMonoPool.Instance.Spawn<ItemView>(_context.MovablesParent);
            packageItemView.Setup(_context.MovablePackageData.ItemSetup.Icon, _context.MovablePackageData.Count);
            packageItemView.transform.position = _context.InputHelper.GetWorldMousePosition();
            return packageItemView;
        }
        
        private void OnEndDrag(ItemSlot itemSlot, PointerEventData pointerEventData)
        {
            var target = pointerEventData.pointerCurrentRaycast.gameObject;
            _context.TargetSlot = target?.GetComponent<ItemSlot>();
            _stateMachine.SwitchState<DropState>();
        }
        
        public void Exit()
        {
            _context.SourceSlot.OnDragEvent -= OnDrag;
            _context.SourceSlot.OnEndDragEvent -= OnEndDrag;
        }
    }
}
