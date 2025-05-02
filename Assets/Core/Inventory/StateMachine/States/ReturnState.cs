using System;
using Core.Pools;
using Core.StateMachines.Interfaces;
using DG.Tweening;

namespace Core.Inventory.StateMachine.States
{
    public class ReturnState : IState, IDisposable
    {
        private IStateMachine _stateMachine;
        private InventoryStatesContext _context;
        private Tween _moveTween;
        
        public ReturnState(IStateMachine stateMachine, InventoryStatesContext context)
        {
            _stateMachine = stateMachine;
            _context = context;
        }

        public void Enter()
        {
            _moveTween = _context.MovableView.transform.DOMove(_context.SourceSlot.transform.position, 0.2f) //visuals config
                .SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    _context.SourceSlot.UpdateViewToData();
                    Reset();
                    _stateMachine.SwitchState<IdleState>();
                });
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
            _moveTween?.Kill(true);
        }

        public void Dispose()
        {
            _moveTween?.Kill(true);
        }
    }
}
