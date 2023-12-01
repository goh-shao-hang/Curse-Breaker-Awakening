using CBA.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(Entity entity, EnemyStateMachine context) : base(entity, context)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _context.NavMeshAgentModule.SetFollowPosition(_entity._playerPos);
    }

}
