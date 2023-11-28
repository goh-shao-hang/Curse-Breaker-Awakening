using CBA.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EntityState
{
    public IdleState(Entity entity) : base(entity)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _entity.NavMeshAgentModule.StopFollow();
    }
}
