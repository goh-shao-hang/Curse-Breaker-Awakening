using CBA.Entities;
using CBA.Modules;
using CBA;
using UnityEngine;
using CBA.Entities.Player.Weapons;
using System;

public class BossPhaseTransitionState : EnemyState
{
    private readonly HealthModule _healthModule;
    private readonly AINavigationModule _navigationModule;
    private readonly CombatAnimationEventHander _combatAnimationEventHander;

    public event Action OnTransitionStart;
    public event Action OnTransitionEvent;
    public event Action OnTransitionFinish;

    public BossPhaseTransitionState(Entity entity, EnemyStateMachine context, CombatAnimationEventHander combatAnimationEventHander) : base(entity, context)
    {
        this._healthModule = _entity.GetModule<HealthModule>();
        this._navigationModule = _entity.GetModule<AINavigationModule>();
        this._combatAnimationEventHander = combatAnimationEventHander;

    }

    public override void Enter()
    {
        base.Enter();

        _navigationModule.StopFollow();
        _navigationModule.RemoveLookTarget();
        _navigationModule.SetSpeed(0);

        _context.Animator.SetTrigger(GameData.BOSSTRANSITION_HASH);

        _combatAnimationEventHander.OnTransitionEvent += TransitionEvent;

        _healthModule.SetInvincibility(true);

        OnTransitionStart?.Invoke();
        //_entity.Die();
    }

    private void TransitionEvent()
    {
        _healthModule.RestoreHealth(_healthModule.MaxHealth);
        OnTransitionEvent?.Invoke();
    }

    public override void Exit()
    {
        base.Exit();

        _combatAnimationEventHander.OnTransitionEvent -= TransitionEvent;
        _healthModule.SetInvincibility(false);

        OnTransitionFinish?.Invoke();
    }
}
