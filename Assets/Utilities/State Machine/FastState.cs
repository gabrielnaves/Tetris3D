public class FastState : IState
{
    readonly string name;
    readonly System.Action enter, update, exit;

    public FastState(string name, System.Action enter = null, System.Action update = null, System.Action exit = null) {
        this.name = name;
        this.enter = enter;
        this.update = update;
        this.exit = exit;
    }

    string IState.Name => name;
    void IState.OnEnter() => enter?.Invoke();
    void IState.OnUpdate() => update?.Invoke();
    void IState.OnExit() => exit?.Invoke();
}
