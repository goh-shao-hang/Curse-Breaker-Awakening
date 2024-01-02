using GameCells.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_SpellCastCompleted : Condition
{
    private readonly Spell _spell;

    private bool _castCompleted = false;

    public Condition_SpellCastCompleted(Spell spell)
    {
        this._spell = spell;
    }

    public override void Enter()
    {
        base.Enter();

        _castCompleted = false;
        this._spell.OnCastCompleted += SetCastComplete;
    }

    private void SetCastComplete()
    {
        _castCompleted = true;
    }

    public override void Exit()
    {
        base.Exit();

        this._spell.OnCastCompleted -= SetCastComplete;
    }

    public override bool Evaluate()
    {
        return this._castCompleted;
    }
}
