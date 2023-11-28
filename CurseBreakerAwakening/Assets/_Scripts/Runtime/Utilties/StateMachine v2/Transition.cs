using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Transition
{
    public Action Callback;

    public abstract bool Evaluate();

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void Update() 
    {
        if (!Evaluate())
            return;

        if (Callback != null)
        {
            Callback?.Invoke();
        }
        else
        {
            Enter();
        }
    }

    public virtual void FixedUpdate() { }
}
