using System;
using System.Collections.Generic;
using Core.Inventory.StateMachine.States;
using Core.StateMachines.Interfaces;

namespace Core.Inventory.StateMachine
{
    public class ItemSlotStateMachine:IStateMachine
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _currentState;

        public ItemSlotStateMachine(ItemSlotStatesContext context)
        {
            _states = new Dictionary<Type, IState>();

            _states.Add(typeof(IdleState),new IdleState(this,context));
            _states.Add(typeof(DragState),new DragState(this,context));
            _states.Add(typeof(DropState),new DropState(this,context));
            SwitchState<IdleState>();
        }
        
        public void SwitchState<T>() where T :IState
        {
            _currentState?.Exit();
            _currentState = _states[typeof(T)];
            _currentState?.Enter();
        }
    }
}