using CBA.Entities;
using CBA.Modules;
using CBA;
using DG.Tweening;

public class RangedAttackState : EnemyState
{
    private readonly AINavigationModule _navigationModule;
    private readonly ProjectileShooter _rangedAttack;
    private readonly float _delay;
    private Tween _fireTween = null;

    public RangedAttackState(Entity entity, EnemyStateMachine context, ProjectileShooter rangedAttack, float delay) : base(entity, context)
    {
        this._rangedAttack = rangedAttack;
        this._navigationModule = _entity.ModuleManager.GetModule<AINavigationModule>();
        this._delay = delay;
    }

    public override void Enter()
    {
        base.Enter();

        _navigationModule.StopFollow();
        _context.Animator.SetTrigger(GameData.RANGEDATTACK_HASH);

        _fireTween = DOVirtual.DelayedCall(_delay, () => _rangedAttack.FireProjectile(_entity.transform.forward));
    }

    public override void Exit()
    {
        base.Exit();

        _fireTween.Kill();
    }
}
