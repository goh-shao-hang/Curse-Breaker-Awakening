using CBA.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyState
{
    public IdleState(Entity entity, EnemyStateMachine context) : base(entity, context)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _context.NavMeshAgentModule.StopFollow();
    }
}
