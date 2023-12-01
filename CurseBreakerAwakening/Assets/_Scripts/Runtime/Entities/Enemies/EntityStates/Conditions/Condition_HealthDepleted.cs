using CBA.Entities;
using GameCells.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_HealthDepleted : Condition
{
    private HealthModule _healthModule;
    private bool _healthDepleted;

    public Condition_HealthDepleted(HealthModule healthModule)
    {
        this._healthModule = healthModule;
    }

    public override void Enter()
    {
        base.Enter();

        this._healthModule.OnHealthDepleted.AddListener(OnHealthDepleted);

        _healthDepleted = false;
    }

    public override void Exit()
    {
        base.Exit();

        this._healthModule.OnHealthDepleted.RemoveListener(OnHealthDepleted);

        _healthDepleted = false;
    }

    private void OnHealthDepleted()
    {
        _healthDepleted = true;
    }

    public override bool Evaluate()
    {
        return _healthDepleted;
    }
}
