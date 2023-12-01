using CBA.Entities;
using GameCells.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : State
{
    protected Entity _entity;
    protected EnemyStateMachine _context;

    public EnemyState(Entity entity, EnemyStateMachine context)
    {
        this._entity = entity;
        this._context = context;
    }
}
