public class SubStateMachine : IState {

    public string Name => $"[{substateMachineName}]: {stateMachine.GetCurrentStateName()}";

    string substateMachineName;
    StateMachine stateMachine;
    IState defaultState;

    public SubStateMachine() {
        SetupSubstateMachine(null, null, null);
    }

    public SubStateMachine(string name, StateMachine stateMachine, IState defaultState) {
        SetupSubstateMachine(name, stateMachine, defaultState);
    }

    protected void SetupSubstateMachine(string name, StateMachine stateMachine, IState defaultState) {
        substateMachineName = name;
        this.stateMachine = stateMachine;
        this.defaultState = defaultState;
    }

    virtual public void OnEnter() => stateMachine.SetState(defaultState);
    virtual public void OnUpdate() => stateMachine.Update();
    virtual public void OnExit() => stateMachine.SetState(null);
}
