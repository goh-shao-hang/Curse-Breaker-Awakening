using CBA;
using CBA.Entities;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareExplodeState : EnemyState
{
    private readonly Material _material;
    private readonly float _timer;
    private Tween _explodeMaterialTween = null;

    public PrepareExplodeState(Entity entity, EnemyStateMachine context, float timer) : base(entity, context)
    {
        this._material = _context.MeshRenderer.material;
        this._timer = timer;
    }

    public override void Enter()
    {
        base.Enter();

        _context.Animator.SetBool(GameData.PREPARINGEXPLODE_HASH, true);

        if (_material != null)
        {
            _explodeMaterialTween = _material?.DOFloat(1f, GameData.GLOW_STRENGTH, _timer).SetEase(Ease.Linear);
        }
    }

    public override void Exit()
    {
        base.Exit();

        _context.Animator.SetBool(GameData.PREPARINGEXPLODE_HASH, false);

        _explodeMaterialTween?.Kill();
        _material?.SetFloat(GameData.GLOW_STRENGTH, 0f);
    }
}
