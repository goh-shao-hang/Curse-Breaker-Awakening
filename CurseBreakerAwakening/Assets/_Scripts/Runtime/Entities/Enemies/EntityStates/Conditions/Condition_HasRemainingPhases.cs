using CBA.Entities;
using GameCells.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_HasRemainingPhases : Condition
{
    private bool _hasRemainingPhases = true;
    private int _currentPhase;

    public Condition_HasRemainingPhases(int phaseCount, HealthModule healthModule)
    {
        _currentPhase = phaseCount;
        healthModule.OnHealthDepleted.AddListener(NextPhase);
    }

    public void NextPhase()
    {
        _currentPhase--;
        if (_currentPhase == 0) //More phases left
        {
            _hasRemainingPhases = false;
        }
    }

    public override bool Evaluate()
    {
        return this._hasRemainingPhases;
    }
}
