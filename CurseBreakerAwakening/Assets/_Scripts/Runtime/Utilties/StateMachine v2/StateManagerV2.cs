using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public abstract class StateManagerV2<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, State<EState>> _StatesDict = new Dictionary<EState, State<EState>>();
    protected State<EState> _currentState;

    private EState _nextStateKey;

    protected void Initialize(EState initialState, Dictionary<EState, Dictionary<Transition, EState>> statesWithTransitions)
    {
        foreach (var stateWithTransitions in statesWithTransitions) 
        {
            foreach (var transition in stateWithTransitions.Value /*Transitions*/)
            {
                transition.Key.Callback = () => _nextStateKey = transition.Value;
                GetState(stateWithTransitions.Key).AddTransition(transition.Key);
            }
        }
    }

    protected virtual void Start()
    {
        _currentState.Enter();
    }

    protected virtual void Update()
    {
        if (_nextStateKey.Equals(_currentState.StateKey))
        {
            _currentState.Update();
        }
        else
        {
            SwitchState(_nextStateKey);

            //TODO return here my cause problem, check later
            return;
        }
    }

    protected virtual void FixedUpdate()
    {
        _currentState.FixedUpdate();
    }

    protected virtual void SwitchState(EState newStateKey)
    {
        _currentState.Exit();
        _currentState = _StatesDict[newStateKey];
        _currentState.Enter();
    }

    public State<EState> GetState(EState stateKey) => _StatesDict[stateKey];
}
