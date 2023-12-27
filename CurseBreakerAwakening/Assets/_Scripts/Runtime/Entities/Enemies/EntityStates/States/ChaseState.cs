using CBA;
using CBA.Entities;
using CBA.Modules;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : EnemyState
{
    private readonly AINavigationModule _navigationModule;

    public ChaseState(Entity entity, EnemyStateMachine context) : base(entity, context)
    {
        _navigationModule = _context.ModuleManager.GetModule<AINavigationModule>();
    }

    public override void Enter()
    {
        base.Enter();

        _navigationModule.SetFollowPosition(_entity._playerPos);
    }

}
