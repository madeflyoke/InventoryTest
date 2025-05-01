using Core.Inventory.View;
using Core.StateMachines.Interfaces;

namespace Core.Inventory.StateMachine.States
{
    public class IdleState : IState
    {
        private IStateMachine _stateMachine;
        private ItemSlotStatesContext _context;
    
        public IdleState(IStateMachine stateMachine, ItemSlotStatesContext context)
        {
            _stateMachine = stateMachine;
            _context = context;
        }
        
        public void Enter()
        {
            _context.RelatedSlot.OnBeginDragEvent += OnBeginDrag;
        }
        
        private void OnBeginDrag()
        {
            _stateMachine.SwitchState<DragState>();
        }

        public void Exit()
        {
            _context.RelatedSlot.OnBeginDragEvent -= OnBeginDrag;
        }
    }
}
