using CBA.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeState : EnemyState
{
    public ExplodeState(Entity entity, EnemyStateMachine context) : base(entity, context)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Debug.LogError("EXPLODED");
    }
}
