using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core.Inventory.View;
using Core.StateMachines.Interfaces;
using Core.Utils;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Inventory.StateMachine.States
{
    public class StackMoveState : IState, IDisposable
    {
        private IStateMachine _stateMachine;
        private InventoryStatesContext _context;
        private CancellationTokenSource _cts;

        private readonly PointerEventData _clickEventData;
        private readonly List<RaycastResult> _raycastResults;
        
    
        public StackMoveState(IStateMachine stateMachine, InventoryStatesContext context)
        {
            _stateMachine = stateMachine;
            _context = context;
            
            _clickEventData = new PointerEventData(EventSystem.current);
            _clickEventData.button = PointerEventData.InputButton.Left;
            _raycastResults = new List<RaycastResult>();
        }
        
        public void Enter()
        {
            _cts = new CancellationTokenSource();

            foreach (var slot in _context.AllSlots)
            {
                slot.OnClickEvent += OnClick;
            }

            _context.InputHelper.LeftMouseClick += CheckInvalidClick;
            UpdatePosition().Forget();
        }

        private void OnClick(ItemSlot itemSlot, PointerEventData data)
        {
            if (data.button == PointerEventData.InputButton.Left)
            {
                _context.TargetSlot = itemSlot;
                _stateMachine.SwitchState<DropState>();
            }
        }

        private async UniTaskVoid UpdatePosition()
        {
            while (_cts.IsCancellationRequested == false)
            {
                var canceled = await UniTask.Yield(cancellationToken: _cts.Token).SuppressCancellationThrow();
                if (canceled)
                {
                    return;
                }
                _context.MovableView.transform.position = _context.InputHelper.GetWorldMousePosition();
            }   
        }

        private void CheckInvalidClick()
        {
            _clickEventData.position = Input.mousePosition;
            EventSystem.current.RaycastAll(_clickEventData, _raycastResults);
            if (_raycastResults.Count>0)
            {
                for (int i = 0; i < _raycastResults.Count; i++)
                {
                    if (_raycastResults[i].gameObject.TryGetComponent(out ItemSlot result))
                    {
                        _context.TargetSlot = result;
                        break;
                    }
                }
            }
            else
            {
                _context.TargetSlot = null;
            }
            _stateMachine.SwitchState<DropState>();
        }

        public void Exit()
        {
            _cts?.Cancel();
            foreach (var slot in _context.AllSlots)
            {
                slot.OnClickEvent -= OnClick;
            }
            _context.InputHelper.LeftMouseClick -= CheckInvalidClick;
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}
