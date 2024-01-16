using CBA.Entities;
using GameCells.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_Parried : Condition
{
    private bool _parried = false;
    private readonly EnemyHurtbox _enemyHurtbox;

    public Condition_Parried(EnemyHurtbox enemyHurtbox)
    {
        this._enemyHurtbox = enemyHurtbox;
        _enemyHurtbox.OnParried += OnParried;
    }

    private void OnParried()
    {
        _parried = true;
    }
    
    public override bool Evaluate()
    {
        return _parried;
    }
}
