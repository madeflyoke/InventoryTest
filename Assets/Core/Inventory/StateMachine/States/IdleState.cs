using Core.Inventory.View;
using Core.StateMachines.Interfaces;
using UnityEngine.EventSystems;

namespace Core.Inventory.StateMachine.States
{
    public class IdleState : IState
    {
        private IStateMachine _stateMachine;
        private InventoryStatesContext _context;
    
        public IdleState(IStateMachine stateMachine, InventoryStatesContext context)
        {
            _stateMachine = stateMachine;
            _context = context;
        }
        
        public void Enter()
        {
            foreach (var slot in _context.AllSlots)
            {
                slot.OnBeginDragEvent += OnBeginDrag;
                slot.OnClickEvent += OnClick;
            }
        }

        private void OnClick(ItemSlot itemSlot, PointerEventData pointerEventData)
        {
            if (pointerEventData.button == PointerEventData.InputButton.Right)
            {
                _context.SourceSlot = itemSlot;
                _stateMachine.SwitchState<StackGetState>();
            }
        }

        private void OnBeginDrag(ItemSlot itemSlot, PointerEventData pointerEventData)
        {
            if (pointerEventData.button == PointerEventData.InputButton.Left)
            {
                _context.SourceSlot = itemSlot;
                _stateMachine.SwitchState<DragState>();
            }
        }

        public void Exit()
        {
            foreach (var slot in _context.AllSlots)
            {
                slot.OnBeginDragEvent -= OnBeginDrag;
                slot.OnClickEvent -= OnClick;
            }
        }
    }
}
