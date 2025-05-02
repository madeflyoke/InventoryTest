using System;
using Core.Pools;
using Core.StateMachines.Interfaces;

namespace Core.Inventory.StateMachine.States
{
    public class DropState : IState
    {
        private IStateMachine _stateMachine;
        private InventoryStatesContext _context;

        public DropState(IStateMachine stateMachine, InventoryStatesContext context)
        {
            _stateMachine = stateMachine;
            _context = context;
        }

        public void Enter()
        {
            var source = _context.SourceSlot;
            var target = _context.TargetSlot;

            if (_context.MovablePackageData==null)
            {
                throw new Exception("No movable package data"); //shouldnt be at all
            }

            if (target!=null && target!=source)
            {
                if (target.IsEmpty) //replace to empty
                {
                    ReplaceItemsPackages(true);
                }
                else if(_context.MovablePackageData.Equals(target.CurrentItemPackage)) //same items
                {
                    ReplaceItemsPackages(false);
                }
                else
                {
                    ReturnToSource();
                }
            }
            else
            {
                ReturnToSource();
            }
            _stateMachine.SwitchState<IdleState>();
        }
        
        private void ReplaceItemsPackages(bool toEmpty)
        {
            var sourceSlot = _context.SourceSlot;
            var targetSlot = _context.TargetSlot;
            
            var sourceItemPackage = sourceSlot.CurrentItemPackage;
            sourceItemPackage.SetCount(sourceItemPackage.Count - _context.MovablePackageData.Count);
            sourceSlot.SetItemPackage(sourceItemPackage);

            var resultCount = _context.MovablePackageData.Count;
            if (toEmpty == false)
                resultCount += targetSlot.CurrentItemPackage.Count;
            
            _context.MovablePackageData.SetCount(resultCount);
            targetSlot.SetItemPackage(_context.MovablePackageData);
        }

        private void ReturnToSource()
        {
            _context.SourceSlot.UpdateViewToData();
        }

        private void Reset()
        {
            _context.SourceSlot =null;
            _context.TargetSlot = null;
            _context.MovablePackageData = null;
            CommonMonoPool.Instance.Despawn(_context.MovableView);
        }

        public void Exit()
        {
            Reset();
        }
    }
}