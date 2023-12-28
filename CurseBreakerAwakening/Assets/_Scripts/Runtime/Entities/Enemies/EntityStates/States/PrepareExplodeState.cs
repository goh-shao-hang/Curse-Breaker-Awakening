using CBA;
using CBA.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareExplodeState : EnemyState
{
    public PrepareExplodeState(Entity entity, EnemyStateMachine context) : base(entity, context)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Debug.LogWarning("IM GONNA EXPLODE");
        _context.Animator.SetTrigger(GameData.PREPAREEXPLODE_HASH);
    }
}
