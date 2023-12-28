using GameCells.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Condition_Timer : Condition
{
    private float _startTime;
    public readonly float Duration;

    private Action _startCallback = null;
    private Action _endCallback = null;


    public Condition_Timer(float duration, Action startCallback = null, Action endCallback = null)
    {
        this.Duration = duration;

        this._startCallback = startCallback;
        this._endCallback = endCallback;
    }

    public override void Enter()
    {
        base.Enter();

        _startTime = Time.time;
        _startCallback?.Invoke();
    }

    public override void Exit()
    {
        base.Exit();

        _endCallback?.Invoke();
    }

    public override bool Evaluate()
    {
        return Time.time > _startTime + Duration;
    }
}
