using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<EState> where EState : Enum
{
    public EState StateKey { get; private set; }

    public State(EState key)
    {
        StateKey = key;
    }


    private List<Transition> _transitions = new List<Transition>();

    public void AddTransition(Transition transition) => _transitions.Add(transition);

    public virtual void Enter()
    {
        foreach (var transition in _transitions)
        {
            transition.Enter();
        }
    }

    public virtual void Exit() 
    {
        foreach (var transition in _transitions)
        {
            transition.Exit();
        }
    }

    public virtual void Update()
    {
        foreach (var transition in _transitions)
        {
            transition.Update();
        }
    }

    public virtual void FixedUpdate()
    {
        foreach (var transition in _transitions)
        {
            transition.FixedUpdate();
        }
    }
}
