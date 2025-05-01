namespace Core.StateMachines.Interfaces
{
    public interface IStateMachine
    {
        public void SwitchState<T>() where T : IState;
    }
}
