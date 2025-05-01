using Core.Inventory.Data;
using Core.Inventory.View;
using Core.StateMachines.Interfaces;
using UnityEngine;

namespace Core.Inventory.StateMachine.States
{
    public class DropState : IState
    {
        private IStateMachine _stateMachine;
        private ItemSlotStatesContext _context;

        public DropState(IStateMachine stateMachine, ItemSlotStatesContext context)
        {
            _stateMachine = stateMachine;
            _context = context;
        }

        public void Enter()
        {
            var source = _context.RelatedSlot;
            var target = _context.TargetSlot;

            if (target == null || target == source)
            {
                ReturnToSource();
            }
            else
            {
                if (target.IsEmpty)
                {
                    MoveItemToTarget(source.CurrentItemPackage);
                }
                else
                {
                    HandleNonEmptyTarget(source, target);
                }
            }
            
            _stateMachine.SwitchState<IdleState>();
        }

        private void MoveItemToTarget(ItemPackageData itemPackage)
        {
            SetToTarget(itemPackage);
        }

        private void HandleNonEmptyTarget(ItemSlot source, ItemSlot target)
        {
            if (source.CurrentItemPackage.Equals(target.CurrentItemPackage))
            {
                MergeItemPackages(source.CurrentItemPackage, target.CurrentItemPackage);
            }
            else
            {
                ReturnToSource();
            }
        }

        private void MergeItemPackages(ItemPackageData sourcePackage, ItemPackageData targetPackage)
        {
            sourcePackage.SetCount(sourcePackage.Count + targetPackage.Count);
            SetToTarget(sourcePackage);
        }

        private void SetToTarget(ItemPackageData itemPackage)
        {
            _context.TargetSlot.SetItemPackage(itemPackage);
            _context.RelatedSlot.Clear();
        }

        private void ReturnToSource()
        {
            _context.RelatedSlot.SetViewActive(true);
        }

        private void Reset()
        {
            _context.TargetSlot = null;
            _context.ItemViewPool.Despawn(_context.MovableView);
        }

        public void Exit()
        {
            Reset();
        }
    }
}