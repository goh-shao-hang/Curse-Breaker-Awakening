using GameCells.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_Timer : Condition
{
    private float _startTime;
    private float _duration;

    public Condition_Timer(float duration)
    {
        this._duration = duration;
    }

    public override void Enter()
    {
        base.Enter();

        _startTime = Time.time;
    }

    public override bool Evaluate()
    {
        return Time.time > _startTime + _duration;
    }
}
