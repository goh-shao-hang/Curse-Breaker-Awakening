using CBA;
using CBA.Core;
using CBA.Entities;
using CBA.Modules;
using GameCells.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeState : EnemyState
{
    private readonly ExplosiveModule _explosive;
    private readonly AudioEmitterModule _audioEmitterModule;
    private readonly AINavigationModule _navigationModule;

    public ExplodeState(Entity entity, EnemyStateMachine context) : base(entity, context)
    {
        this._navigationModule = _context.GetModule<AINavigationModule>();
        this._audioEmitterModule = _context.GetModule<AudioEmitterModule>();
        this._explosive = _context.GetModule<ExplosiveModule>();
    }

    public override void Enter()
    {
        base.Enter();

        _navigationModule?.Disable();
        _context.Hurtbox.Disable();

        //Debug.LogError("EXPLODED");

        _explosive.TriggerExplosion();

        _audioEmitterModule.UnparentAndDelayedDestroyEmitter();
        _audioEmitterModule?.PlaySfx("Beetle_Explosion");

        _entity.Die();

        //Disable immediately
        _context.gameObject.SetActive(false);
    }
}
