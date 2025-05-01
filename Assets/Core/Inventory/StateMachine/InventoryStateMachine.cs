using System;
using System.Collections.Generic;
using System.Linq;
using Core.Inventory.StateMachine.States;
using Core.StateMachines.Interfaces;
using UnityEngine;

namespace Core.Inventory.StateMachine
{
    public class InventoryStateMachine : IStateMachine, IDisposable
    {
        private readonly Dictionary<Type, IState> _states;
        private InventoryStatesContext _context;
        private IState _currentState;

        public InventoryStateMachine(InventoryStatesContext context)
        {
            _states = new Dictionary<Type, IState>();
            _context = context;

            AddState<IdleState>();
            AddState<DragState>();
            AddState<DropState>();
            
            AddState<StackGetState>();
            AddState<StackMoveState>();
            
            SwitchState<IdleState>();
        }

        private void AddState<T>() where T : IState
        {
            _states.Add(typeof(T), (T)Activator.CreateInstance(typeof(T), args: new object[]{this,_context}));
        }
        
        public void SwitchState<T>() where T :IState
        {
            _currentState?.Exit();
            _currentState = _states[typeof(T)];
            _currentState?.Enter();
        }

        public void Dispose()
        {
            foreach (var state in _states.Values)
            {
                if (state is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}