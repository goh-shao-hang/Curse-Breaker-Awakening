using CBA;
using CBA.Entities;
using CBA.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : EnemyState
{
    public IdleState(Entity entity, EnemyStateMachine context) : base(entity, context)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _context.ModuleManager.GetModule<AINavigationModule>().StopFollow();
    }
}
