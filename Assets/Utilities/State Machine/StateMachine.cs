using System;
using System.Collections.Generic;

public class StateMachine {

    IState currentState;

    readonly Dictionary<IState, List<Transition>> stateTransitions = new Dictionary<IState, List<Transition>>();
    readonly List<Transition> anyTransitions = new List<Transition>();
    List<Transition> currentTransitions = new List<Transition>();

    static readonly List<Transition> EmptyTransitions = new List<Transition>(capacity:0);

    public string GetCurrentStateName() => currentState.Name;
    public bool IsCurrentState(IState state) => currentState == state;

    public void Update() {
        CheckForValidTransitionAndFollowItIfExists();
        currentState?.OnUpdate();
    }

    public void SetState(IState state) {
        if (state != currentState) {
            currentState?.OnExit();
            currentState = state;

            if (currentState == null || stateTransitions.TryGetValue(currentState, out currentTransitions) == false)
                currentTransitions = EmptyTransitions;

            currentState?.OnEnter();
        }
    }

    public void AddTransition(IState from, IState to, Func<bool> predicate) {
        if (!stateTransitions.ContainsKey(from))
            stateTransitions[from] = new List<Transition>();
        stateTransitions[from].Add(new Transition(to, predicate));
    }

    public void AddAnyTransition(IState to, Func<bool> predicate) {
        anyTransitions.Add(new Transition(to, predicate));
    }

    class Transition {
        public IState To { get; }
        public Func<bool> Condition { get; }

        public Transition(IState to, Func<bool> condition) {
            To = to;
            Condition = condition;
        }
    }

    void CheckForValidTransitionAndFollowItIfExists() {
        var transition = GetValidTransition();
        if (transition != null)
            SetState(transition.To);
    }

    Transition GetValidTransition() {
        foreach (var transition in anyTransitions) {
            if (transition.Condition())
                return transition;
        }
        foreach (var transition in currentTransitions) {
            if (transition.Condition())
                return transition;
        }
        return null;
    }
}
