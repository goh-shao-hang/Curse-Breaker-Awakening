using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class MeleeAttackState : EnemyState
    {
        private AttackData _attackData;

        public MeleeAttackState(Entity entity, EnemyStateMachine context, AttackData attackData) : base(entity, context)
        {
            this._attackData = attackData;
        }

        public override void Enter()
        {
            base.Enter();

            _context.NavMeshAgentModule.StopFollow();
            _context.Animator.SetTrigger(GameData.ATTACK_HASH);
            _attackData.Hitbox.SetDamage(_attackData.Damage);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}