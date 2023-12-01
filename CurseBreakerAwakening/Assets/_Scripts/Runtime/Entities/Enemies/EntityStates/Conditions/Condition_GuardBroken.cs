using CBA.Entities;
using GameCells.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_GuardBroken : Condition
{
    private GuardModule _guardModule;
    private bool _guardBroken;

    public Condition_GuardBroken(GuardModule guardModule)
    {
        this._guardModule = guardModule;
    }

    public override void Enter()
    {
        base.Enter();

        _guardModule.OnGuardBroken.AddListener(OnGuardBroken);
    }

    public override void Exit()
    {
        base.Exit();

        _guardModule.OnGuardBroken.RemoveListener(OnGuardBroken);
        _guardBroken = false;
    }

    private void OnGuardBroken()
    {
        _guardBroken = true;
    }

    public override bool Evaluate()
    {
        return _guardBroken;
    }

}
