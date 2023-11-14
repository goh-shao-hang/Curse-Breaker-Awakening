using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<EState> where EState : Enum
{
    public EState StateKey { get; private set; }

    public BaseState(EState key)
    {
        StateKey = key;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void FixedUpdate() { }
    public virtual void Update() { }
    public abstract EState GetNextState(); //Return the next state to go to based on some condition. Return this.StateKey to remain in current state
}
