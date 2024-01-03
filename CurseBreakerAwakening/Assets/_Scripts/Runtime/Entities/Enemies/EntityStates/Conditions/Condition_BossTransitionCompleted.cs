using CBA.Entities.Player.Weapons;
using GameCells.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_BossTransitionCompleted : Condition
{
    private CombatAnimationEventHander _combatAnimationEventHander;

    private bool _transitionCompleted = false;

    public Condition_BossTransitionCompleted(CombatAnimationEventHander combatAnimationEventHander)
    {
        _combatAnimationEventHander = combatAnimationEventHander;
    }

    public override void Enter()
    {
        base.Enter();

        _transitionCompleted = false;
        this._combatAnimationEventHander.OnTransitionComplete += SetTransitionComplete;
    }

    private void SetTransitionComplete()
    {
        _transitionCompleted = true;
    }

    public override void Exit()
    {
        base.Exit();

        this._combatAnimationEventHander.OnTransitionComplete -= SetTransitionComplete;
    }

    public override bool Evaluate()
    {
        return this._transitionCompleted;
    }
}
