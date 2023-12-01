using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CBA.Entities
{
    public class MeleeAttackState : EnemyState
    {
        public MeleeAttackState(Entity entity, EnemyStateMachine context) : base(entity, context)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _context.NavMeshAgentModule.StopFollow();
            _context.Animator.SetTrigger(GameData.ATTACK_HASH);
        }
    }
}