using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> _StatesDict = new Dictionary<EState, BaseState<EState>>();

    protected BaseState<EState> _currentState;

    protected bool _isSwitchingState = false; //Prevents repeated state change due to high frame rate

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        _currentState.Enter();
    }

    protected virtual void Update()
    {
        EState nextStateKey = _currentState.GetNextState();

        if (!_isSwitchingState && nextStateKey.Equals(_currentState.StateKey))
        {
            _currentState.Update();
        }
        else if (!_isSwitchingState)
        {
            SwitchState(nextStateKey);

            return;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (!_isSwitchingState)
            _currentState.FixedUpdate();
    }

    protected virtual void SwitchState(EState newStateKey)
    {
        _isSwitchingState = true;

        _currentState.Exit();
        _currentState = _StatesDict[newStateKey];
        _currentState.Enter();

        _isSwitchingState = false;
    }
}
