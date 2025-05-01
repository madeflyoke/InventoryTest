using Core.Inventory.Data;
using Core.Inventory.View;
using Core.Pools;
using Core.StateMachines.Interfaces;
using UnityEngine;

namespace Core.Inventory.StateMachine.States
{
    public class StackGetState : IState
    {
        private const int StackCount = 1;
        
        private IStateMachine _stateMachine;
        private InventoryStatesContext _context;
    
        public StackGetState(IStateMachine stateMachine, InventoryStatesContext context)
        {
            _stateMachine = stateMachine;
            _context = context;
        }
        
        public void Enter()
        {
            var stackCount = StackCount;
            var countDiff = _context.SourceSlot.CurrentItemPackage.Count - stackCount;
            
            if (countDiff<=0)
            {
                stackCount += countDiff;
                _context.SourceSlot.SetViewActive(false);
            }
            else
            {
                _context.SourceSlot.ChangeViewData(countDiff);
            }

            _context.MovablePackageData = _context.SourceSlot.CurrentItemPackage.Clone();
            _context.MovablePackageData.SetCount(stackCount);
            _context.MovableView = CreateVisualPackage();
            
            _stateMachine.SwitchState<StackMoveState>();
        }
        
        private ItemView CreateVisualPackage()
        {
            var packageItemView = CommonPool.Instance.Spawn<ItemView>(_context.MovablesParent);
            packageItemView.Setup(_context.MovablePackageData.ItemSetup.Icon, _context.MovablePackageData.Count);
            packageItemView.transform.position = Input.mousePosition;
            return packageItemView;
        }

        public void Exit()
        {
           
        }
    }
}
