using Codice.Client.BaseCommands.Ls;
using GameCells;
using GameCells.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public List<Transition> _transitions = new();

    public virtual void Enter() 
    {
        foreach (var transition in _transitions) 
        {
            transition.Condition.Enter();
        }
    }

    public virtual void Update() 
    {
        foreach (var transition in _transitions)
        {
            transition.Condition.Update();
        }
    }

    public virtual void FixedUpdate() 
    {
        foreach (var transition in _transitions)
        {
            transition.Condition.FixedUpdate();
        }
    }

    public virtual void Exit() 
    {
        foreach (var transition in _transitions)
        {
            transition.Condition.Exit();
        }
    }

    /// <summary>
    /// Add a transition this state can transition to upon meeting certain condition
    /// </summary>
    /// <param name="targetState"></param>
    /// <param name="condition"></param>
    public void AddTransition(State targetState, Condition condition) => _transitions.Add(new Transition(targetState, condition));

    public void AddTransition(Transition transition) => _transitions.Add(transition);
}
