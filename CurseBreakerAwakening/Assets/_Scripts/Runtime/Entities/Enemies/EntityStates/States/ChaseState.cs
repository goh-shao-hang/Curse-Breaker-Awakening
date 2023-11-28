using CBA.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : EntityState
{
    public ChaseState(Entity entity) : base(entity)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _entity.NavMeshAgentModule.SetFollowPosition(_entity._playerPos);
    }

}
