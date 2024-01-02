using GameCells.StateMachine;
using UnityEngine;

public class Condition_Timer_Random : Condition
{
    private float _startTime;
    public float Duration;
    private readonly float _minDuration;
    private readonly float _maxDuration;

    public Condition_Timer_Random(float minDuration, float maxDuration)
    {
        _minDuration = minDuration;
        _maxDuration = maxDuration;
    }

    public override void Enter()
    {
        base.Enter();

        this.Duration = Random.Range(_minDuration, _maxDuration);

        _startTime = Time.time;
    }

    public override bool Evaluate()
    {
        return Time.time > _startTime + Duration;
    }
}
